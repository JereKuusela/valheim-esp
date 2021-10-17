using System;
using System.Collections.Generic;
using HarmonyLib;
using Service;
using UnityEngine;
using Visualization;

namespace ESP {
  public class SpawnAreaUtils {
    public static String Get(SpawnArea obj) {
      int near, total;
      obj.GetInstances(out near, out total);
      var lines = new List<string>(){
        Format.ProgressPercent("Timer", obj.m_spawnTimer, obj.m_spawnIntervalSec),
        "Area: " + Format.Int(obj.m_spawnRadius) + " m",
        "Level up: " + Format.Percent(obj.m_levelupChance / 100f),
        "Trigger: " + Format.Int(obj.m_triggerDistance) + " m",
        "Near limit: " + Format.Progress(near, obj.m_maxNear) + " within " + Format.Int(obj.m_nearRadius) + " m",
        "Far limit: " +  Format.Progress(total, obj.m_maxTotal) + " within " + Format.Int(obj.m_farRadius) + " m"
      };
      if (obj.m_onGroundOnly) {
        lines.Add("Only on ground");
      }

      lines.Add("");
      var totalWeight = 0f;
      foreach (var data in obj.m_prefabs)
        totalWeight += data.m_weight;
      foreach (var data in obj.m_prefabs) {
        var level = Format.Range(data.m_minLevel, data.m_maxLevel);
        lines.Add(data.m_prefab.name + ": " + Format.Percent(data.m_weight / totalWeight) + " with level " + level);
      }
      return lines.Join(null, "\n");
    }
  }

  [HarmonyPatch(typeof(SpawnArea), "Awake")]
  public class SpawnArea_Awake {
    public static void Postfix(SpawnArea __instance, float ___m_spawnTimer) {
      if (Settings.SpawnAreasLineWidth == 0)
        return;
      var obj = Draw.DrawSphere(Tag.Spawner, __instance, __instance.m_triggerDistance, Settings.SpawnAreaTriggerColor, Settings.SpawnAreasLineWidth);
      obj.AddComponent<SpawnAreaText>().spawnArea = __instance;
      obj = Draw.DrawSphere(Tag.Spawner, __instance, __instance.m_nearRadius, Settings.SpawnAreaNearColor, Settings.SpawnAreasLineWidth);
      obj.AddComponent<SpawnAreaText>().spawnArea = __instance;
      obj = Draw.DrawSphere(Tag.Spawner, __instance, __instance.m_spawnRadius, Settings.SpawnAreaSpawnColor, Settings.SpawnAreasLineWidth);
      obj.AddComponent<SpawnAreaText>().spawnArea = __instance;
    }
  }

  public class SpawnAreaText : MonoBehaviour, Hoverable {
    public string GetHoverText() => GetHoverName() + "\n" + SpawnAreaUtils.Get(spawnArea);
    public string GetHoverName() => Format.String(spawnArea.name);
    public SpawnArea spawnArea;
  }
}