using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ESP
{
  public class SpawnAreaUtils
  {
    public static String GetText(SpawnArea instance, float spawnTimer)
    {
      int near, total;
      Patch.SpawnArea_GetInstances(instance, out near, out total);
      var lines = new List<string>(){
        TextUtils.String(instance.name),
        TextUtils.ProgressPercent("Timer", spawnTimer, instance.m_spawnIntervalSec),
        "Area: " + TextUtils.Int(instance.m_spawnRadius) + " m",
        "Level up: " + TextUtils.Percent(instance.m_levelupChance / 100f),
        "Trigger: " + TextUtils.Int(instance.m_triggerDistance) + " m",
        "Near limit: " + TextUtils.Progress(near, instance.m_maxNear) + " within " + TextUtils.Int(instance.m_nearRadius) + " m",
        "Far limit: " +  TextUtils.Progress(total, instance.m_maxTotal) + " within " + TextUtils.Int(instance.m_farRadius) + " m"
      };
      if (instance.m_onGroundOnly)
      {
        lines.Add("Only on ground");
      }

      lines.Add("");
      var totalWeight = 0f;
      foreach (var data in instance.m_prefabs)
        totalWeight += data.m_weight;
      foreach (var data in instance.m_prefabs)
      {
        var level = TextUtils.Range(data.m_minLevel, data.m_maxLevel);
        lines.Add(data.m_prefab.name + ": " + TextUtils.Percent(data.m_weight / totalWeight) + " with level " + level);
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
      var text = SpawnAreaUtils.GetText(__instance, ___m_spawnTimer);
      var obj = Drawer.DrawSphere(__instance.gameObject, __instance.m_triggerDistance, Color.red, 0.5f);
      Drawer.AddText(obj, text);
      obj = Drawer.DrawSphere(__instance.gameObject, __instance.m_nearRadius, Color.white, 0.5f);
      Drawer.AddText(obj, text);
      obj = Drawer.DrawSphere(__instance.gameObject, __instance.m_spawnRadius, Color.cyan, 0.5f);
      Drawer.AddText(obj, text);
    }
  }

  [HarmonyPatch(typeof(SpawnArea), "UpdateSpawn")]
  public class SpawnArea_UpdateSpawn
  {
    public static void Postfix(SpawnArea __instance, float ___m_spawnTimer)
    {
      if (!Settings.showSpawnAreas)
        return;
      var text = SpawnAreaUtils.GetText(__instance, ___m_spawnTimer);
      Drawer.UpdateTexts(__instance.gameObject, text);
    }
  }
}