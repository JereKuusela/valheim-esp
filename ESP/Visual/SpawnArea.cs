using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ESP
{
  public class SpawnAreaUtils
  {
    public static String Get(SpawnArea obj)
    {
      var spawnTimer = Patch.m_spawnTimer(obj);
      int near, total;
      Patch.SpawnArea_GetInstances(obj, out near, out total);
      var lines = new List<string>(){
        Format.ProgressPercent("Timer", spawnTimer, obj.m_spawnIntervalSec),
        "Area: " + Format.Int(obj.m_spawnRadius) + " m",
        "Level up: " + Format.Percent(obj.m_levelupChance / 100f),
        "Trigger: " + Format.Int(obj.m_triggerDistance) + " m",
        "Near limit: " + Format.Progress(near, obj.m_maxNear) + " within " + Format.Int(obj.m_nearRadius) + " m",
        "Far limit: " +  Format.Progress(total, obj.m_maxTotal) + " within " + Format.Int(obj.m_farRadius) + " m"
      };
      if (obj.m_onGroundOnly)
      {
        lines.Add("Only on ground");
      }

      lines.Add("");
      var totalWeight = 0f;
      foreach (var data in obj.m_prefabs)
        totalWeight += data.m_weight;
      foreach (var data in obj.m_prefabs)
      {
        var level = Format.Range(data.m_minLevel, data.m_maxLevel);
        lines.Add(data.m_prefab.name + ": " + Format.Percent(data.m_weight / totalWeight) + " with level " + level);
      }
      return lines.Join(null, "\n");
    }
  }

  [HarmonyPatch(typeof(SpawnArea), "Awake")]
  public class SpawnArea_Awake
  {
    public static void Postfix(SpawnArea __instance, float ___m_spawnTimer)
    {
      if (!Settings.showSpawnAreas)
        return;
      var obj = Drawer.DrawSphere(__instance.gameObject, __instance.m_triggerDistance, Color.red, 0.5f);
      obj.AddComponent<SpawnAreaText>().spawnArea = __instance;
      obj = Drawer.DrawSphere(__instance.gameObject, __instance.m_nearRadius, Color.white, 0.5f);
      obj.AddComponent<SpawnAreaText>().spawnArea = __instance;
      obj = Drawer.DrawSphere(__instance.gameObject, __instance.m_spawnRadius, Color.cyan, 0.5f);
      obj.AddComponent<SpawnAreaText>().spawnArea = __instance;
    }
  }

  public class SpawnAreaText : MonoBehaviour, Hoverable
  {
    public string GetHoverText() => GetHoverName() + "\n" + SpawnAreaUtils.Get(spawnArea);
    public string GetHoverName() => Format.String(spawnArea.name);
    public SpawnArea spawnArea;
  }
}