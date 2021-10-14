using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Text;
using UnityEngine;

namespace ESP {

  [HarmonyPatch(typeof(MessageHud), "UpdateMessage")]
  public class MessageHud_GetBaseMessage : MonoBehaviour {
    private static string BaseMessage = "";
    // Use state to track when a default in game message arrives.
    public static void Prefix(out string __state) {
      __state = MessageHud.instance.m_messageText.text;
    }
    public static void Postfix(MessageHud __instance, float ___m_msgQueueTimer, string __state) {
      if (__instance.m_messageText.text != __state)
        BaseMessage = __instance.m_messageText.text;
      // No more base game messages.
      if (___m_msgQueueTimer >= 4f)
        BaseMessage = "";
      __instance.m_messageText.text = BaseMessage;
    }
  }


  [HarmonyPatch(typeof(MessageHud), "Update")]
  public class MessageHud_UpdateMessage : MonoBehaviour {
    private static string GetShowHide(bool value) => value ? "Hide" : "Show";
    private static List<string> GetInfo() {
      if (!Settings.ShowHud) return new List<string>();
      var position = Player.m_localPlayer.transform.position;
      var lines = new List<string>();
      lines.Add(GetEnvironment());
      lines.Add(GetPosition(position));
      lines.Add(GetSpeed() + ", " + GetNoise());
      lines.Add(Ruler.GetText(position));
      lines.Add(GetTrackedObjects());
      return lines.Where(item => item != "").ToList();
    }
    private static List<string> GetFixedMessage() {
      var lines = new List<string>();
      // Wait for the game to load.
      if (Player.m_localPlayer == null) return lines;
      lines.AddRange(GetInfo());
      var localShip = Ship.GetLocalShip();
      if (localShip) {
        lines.Add(" ");
        lines.AddRange(Texts.Get(localShip).Split('\n').Where(line => line != ""));
      }
      return lines;
    }
    private static string GetSpeed() => "Speed: " + Format.Float(Patch.CurrentVel(Player.m_localPlayer).magnitude, "0.#") + " m/s";
    private static string GetNoise() => "Noise: " + Format.Int(Player.m_localPlayer.GetNoiseRange()) + " meters";
    private static string GetEnvironment() {
      if (!Settings.ShowTimeAndWeather) return "";
      return EnvUtils.GetTime() + ", " + EnvUtils.GetCurrentEnvironment() + " (" + EnvUtils.GetWindHud() + ")";
    }
    private static string GetPosition(Vector3 position) {
      if (!Settings.ShowPosition) return "";
      var lines = new List<string>(){
        EnvUtils.GetLocation(position),
        EnvUtils.GetAltitude(position),
        EnvUtils.GetForest(position),
        EnvUtils.GetBlocked(position)
      };
      return Format.JoinRow(lines);
    }
    private static float TrackUpdateTimer = 0;
    // ZDO tracks take a while for big worlds so have a longer timer for them.
    // More careful solution would be using sectors but more complicated.
    private static float TrackUpdateLongTimer = 0;
    private static IDictionary<string, int> trackCache = new Dictionary<string, int>();
    public static string GetTrackedObjects() {
      if (Settings.TrackedObjects == "") return "";
      TrackUpdateTimer += Time.deltaTime;
      TrackUpdateLongTimer += Time.deltaTime;
      var tracked = Settings.TrackedObjects.Split(',');
      if (TrackUpdateTimer >= 1f) {
        TrackUpdateTimer = 0;
        var itemsDrops = Patch.Instances(new ItemDrop());
        foreach (var name in tracked) {
          var prefab = ZNetScene.instance.GetPrefab(name);
          var count = 0;
          if (prefab == null)
            count = -1;
          else if (prefab.GetComponent<Character>() != null)
            count = SpawnSystem.GetNrOfInstances(prefab);
          else if (prefab.GetComponent<ItemDrop>() != null)
            count = itemsDrops.Where(item => item.name.Replace("(Clone)", "") == prefab.name).Sum(item => item.m_itemData.m_stack);
          else {
            if (TrackUpdateLongTimer < 5f) continue;
            var zdos = new List<ZDO>();
            ZDOMan.instance.GetAllZDOsWithPrefab(prefab.name, zdos);
            var position = Player.m_localPlayer.transform.position;
            count = zdos.Where(zdo => Utils.DistanceXZ(zdo.GetPosition(), position) < Settings.TrackRadius).Count();
          }
          trackCache[name] = count;
        }
        if (TrackUpdateLongTimer >= 5f) TrackUpdateLongTimer = 0f;
      }

      var tracks = tracked.Select(name => {
        if (!trackCache.ContainsKey(name))
          return "";
        if (trackCache[name] < 0)
          return name + ": " + Format.String("Invalid ID", "red");
        return name + ": " + Format.Int(trackCache[name]);
      });

      return Format.JoinRow(tracks);
    }

    public static void Postfix() {
      var hud = MessageHud.instance;
      var previousText = hud.m_messageText.text;
      var lines = GetFixedMessage();
      if (lines.Count == 0) return;

      var previousPadding = 0;
      while (previousText.StartsWith(" \n")) {
        previousPadding++;
        previousText = previousText.Substring(2);
      }
      var padding = previousPadding + lines.Count - 2;
      for (var i = 0; i < padding; i++) lines.Insert(0, " ");
      if (previousText != "") {
        lines.Insert(0, " ");
        lines.Add(" ");
        lines.Add(previousText);
      }
      hud.m_messageText.text = Format.JoinLines(lines);
      hud.m_messageText.CrossFadeAlpha(1f, 0f, true);
      // Icon is not very relevant information and will pop up over the text.
      hud.m_messageIcon.canvasRenderer.SetAlpha(0f);

    }
  }
}