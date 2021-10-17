using System.Collections.Generic;
using System.Linq;
using Service;
using UnityEngine;

namespace ESP {
  public static class Hud {
    public static List<string> GetMessage() {
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
    private static string GetSpeed() => "Speed: " + Format.Float(Player.m_localPlayer.m_currentVel.magnitude, "0.#") + " m/s";
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
    private static string GetTrackedObjects() {
      if (Settings.TrackedObjects == "") return "";
      TrackUpdateTimer += Time.deltaTime;
      TrackUpdateLongTimer += Time.deltaTime;
      var tracked = Settings.TrackedObjects.Split(',');
      if (TrackUpdateTimer >= 1f) {
        TrackUpdateTimer = 0;
        var itemsDrops = ItemDrop.m_instances;
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

  }
}