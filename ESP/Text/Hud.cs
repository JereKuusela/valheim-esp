using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Service;
using UnityEngine;
namespace ESP;
public static class Hud {
  public static List<string> GetMessage() {
    List<string> lines = new();
    lines.AddRange(GetInfo());
    var localShip = Ship.GetLocalShip();
    if (localShip) {
      lines.Add(" ");
      lines.AddRange(Texts.Get(localShip).Split('\n').Where(line => line != ""));
    }
    return lines;
  }
  private static List<string> GetInfo() {
    if (!Settings.ShowHud) return new();
    var position = Player.m_localPlayer.transform.position;
    List<string> lines = new();
    lines.Add(GetEnvironment());
    lines.Add(GetPosition(position));
    lines.Add(GetSpeed() + ", " + GetNoise() + ", " + GetLight());
    lines.Add(GetTrackedObjects());
    lines.Add(GetStaggerTracker());
    return lines.Where(item => item != "").ToList();
  }
  private static string GetSpeed() => "Speed: " + Format.Float(Player.m_localPlayer.m_currentVel.magnitude, "0.#") + " m/s";
  private static string GetNoise() => "Noise: " + Format.Int(Player.m_localPlayer.GetNoiseRange()) + " meters";
  private static string GetLight() => "Light: " + Format.Percent(StealthSystem.instance.GetLightFactor(Player.m_localPlayer.GetCenterPoint()));
  private static string GetEnvironment() {
    if (!Settings.ShowTimeAndWeather) return "";
    return EnvUtils.GetTime() + ", " + EnvUtils.GetCurrentEnvironment() + " (" + EnvUtils.GetWindHud() + ")";
  }
  private static string GetStaggerTracker() {
    if (CreatureStagger.StaggerDuration == 0)
      return "Stagger: ...";
    return "Stagger: " + CreatureStagger.StaggerDuration;
  }
  private static string GetPosition(Vector3 position) {
    if (!Settings.ShowPosition) return "";
    List<string> lines = new(){
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
  private static Dictionary<string, int> trackCache = new();
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
          List<ZDO> zdos = new();
          ZDOMan.instance.GetAllZDOsWithPrefab(prefab.name, zdos);
          var position = Player.m_localPlayer.transform.position;
          if (Settings.TrackRadius < 0)
            count = zdos.Count();
          else
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




  [HarmonyPatch(typeof(Character), nameof(Character.FixedUpdate))]
  public class CreatureStagger {

    private static long StaggerStart = 0;
    private static ZDOID Id = ZDOID.None;
    public static double StaggerDuration = 0;

    static void Postfix(Character __instance) {
      bool isStaggering = __instance.IsStaggering();
      if (StaggerStart == 0 && isStaggering) {
        StaggerStart = ZNet.instance.GetTime().Ticks;
        StaggerDuration = 0;
        Id = __instance.GetZDOID();
      }
      var staggered = (ZNet.instance.GetTime() - new DateTime(StaggerStart)).TotalSeconds;
      if (Id == __instance.GetZDOID() && !isStaggering) {
        StaggerDuration = staggered;
        StaggerStart = 0;
        Id = ZDOID.None;
      }
      // If target dies while staggering.
      if (staggered > 10) {
        StaggerStart = 0;
        Id = ZDOID.None;
      }
    }
  }
}
