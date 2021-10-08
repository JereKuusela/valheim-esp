using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Text;
using UnityEngine;
using Visualization;

namespace ESP {

  [HarmonyPatch(typeof(MessageHud), "UpdateMessage")]
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
      lines.Add(GetVisualSettings());
      lines.Add(GetOtherSettings());
      return lines.Where(item => item != "").ToList();
    }
    private static List<string> GetFixedMessage() {
      var lines = new List<string>();
      // Wait for the game to load.
      if (Player.m_localPlayer == null) return lines;
      lines.AddRange(GetInfo());
      var dps = DPSMeter.Get();
      if (dps != null) {
        lines.Add(" ");
        lines.AddRange(dps);
      }
      var eps = ExperienceMeter.Get();
      if (eps != null) {
        lines.Add(" ");
        lines.AddRange(eps);
      }
      var localShip = Ship.GetLocalShip();
      if (localShip) {
        lines.Add(" ");
        lines.AddRange(Texts.Get(localShip).Split('\n').Where(line => line != ""));
      }
      return lines;
    }
    private static string GetVisualSettings() {
      var text = Format.String("Y") + ": " + GetShowHide(Visibility.IsGroup(Group.Zone)) + " zones, ";
      text += Format.String("U") + ": " + GetShowHide(Visibility.IsGroup(Group.Creature)) + " creatures, ";
      text += Format.String("I") + ": " + GetShowHide(Visibility.IsGroup(Group.Other)) + " other";
      return text;
    }
    private static string GetOtherSettings() {
      var text = Format.String("O") + ": " + GetShowHide(Text.extraInfo) + " extra info, ";
      text += Format.String("P") + ": " + GetShowHide(Settings.ShowDPS) + " DPS, ";
      text += Format.String("L") + ": " + GetShowHide(Settings.ShowExperienceMeter) + " experience";
      return text;
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
    public static string GetTrackedObjects() {
      if (Settings.TrackedObjects == "") return "";
      var tracked = Settings.TrackedObjects.Split(',');
      var itemsDrops = Patch.Instances(new ItemDrop());
      var tracks = tracked.Select(name => {
        var prefab = ZNetScene.instance.GetPrefab(name);
        if (prefab == null) return name + ": " + Format.String("Invalid ID", "red");
        var creatures = SpawnSystem.GetNrOfInstances(prefab);
        var drops = itemsDrops.Where(item => item.name.Replace("(Clone)", "") == prefab.name).Sum(item => item.m_itemData.m_stack);
        return name + ": " + Format.Int(creatures + drops);
      });

      return Format.JoinRow(tracks);
    }

    private static string baseGameMessage = "";

    // Use state to track when a default in game message arrives.
    public static void Prefix(out string __state) {
      __state = MessageHud.instance.m_messageText.text;
    }
    // Keeps the message always visible and shows any base game messages.
    public static void Postfix(float ___m_msgQueueTimer, string __state) {
      var hud = MessageHud.instance;
      var lines = GetFixedMessage();
      var isCustomMessage = lines.Count > 0;
      // New base game message.
      if (hud.m_messageText.text != __state)
        baseGameMessage = hud.m_messageText.text;
      if (baseGameMessage != "") {
        // No more base game messages.
        if (___m_msgQueueTimer >= 4f)
          baseGameMessage = "";
      }
      if (baseGameMessage != "") {
        lines.Add("");
        lines.Add(baseGameMessage);
      }
      if (lines.Count == 0) return;
      var padding = lines.Count - 2;
      for (var i = 0; i < padding; i++) lines.Insert(0, " ");
      hud.m_messageText.CrossFadeAlpha(1f, 0f, true);
      hud.m_messageText.text = Format.JoinLines(lines);
      // Icon is not very relevant information and will pop up over the text.
      if (isCustomMessage)
        hud.m_messageIcon.canvasRenderer.SetAlpha(0f);
    }
  }
}