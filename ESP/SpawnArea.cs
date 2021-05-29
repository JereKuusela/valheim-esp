using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public class SpawnAreaUtils
  {
    public static String GetText(SpawnArea instance, float spawnTimer)
    {
      int near, total;
      Patch.SpawnArea_GetInstances(instance, out near, out total);
      var lines = new string[]{
        TextUtils.StringValue(instance.name),
        TextUtils.ProgressValue("Timer", spawnTimer, instance.m_spawnIntervalSec),
        "Area: " + TextUtils.IntValue(instance.m_spawnRadius) + " m",
        "Level up: " + TextUtils.PercentValue(instance.m_levelupChance / 100f),
        "Trigger: " + TextUtils.IntValue(instance.m_triggerDistance) + " m",
        "Near limit: " + TextUtils.StringValue(near + "/" + instance.m_maxNear) + " within " + TextUtils.IntValue(instance.m_nearRadius) + " m",
        "Far limit: " +  TextUtils.StringValue(total + "/" + instance.m_maxTotal) + " within " + TextUtils.IntValue(instance.m_farRadius) + " m"
      };
      if (instance.m_onGroundOnly)
      {
        lines.AddItem("Only on ground");
      }

      lines.AddItem("");
      var totalWeight = 0f;
      foreach (var data in instance.m_prefabs)
      {
        totalWeight += data.m_weight;
      }
      foreach (var data in instance.m_prefabs)
      {
        var level = data.m_maxLevel > data.m_minLevel ? data.m_minLevel + "-" + data.m_maxLevel : data.m_maxLevel.ToString();
        lines.AddItem(TextUtils.StringValue(data.m_prefab.name) + TextUtils.PercentValue(data.m_weight / totalWeight) + " Level: " + TextUtils.StringValue(level));
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
      Drawer.DrawSphere(__instance.gameObject, Vector3.zero, __instance.m_triggerDistance, Color.red, 0.5f, text);
      Drawer.DrawSphere(__instance.gameObject, Vector3.zero, __instance.m_nearRadius, Color.white, 0.5f, text);
      Drawer.DrawSphere(__instance.gameObject, Vector3.zero, __instance.m_spawnRadius, Color.cyan, 0.5f, text);
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