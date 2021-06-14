using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ESP
{
  public class SpawnAreaUtils
  {
    public static String GetText(SpawnArea obj)
    {
      var spawnTimer = Patch.m_spawnTimer(obj);
      int near, total;
      Patch.SpawnArea_GetInstances(obj, out near, out total);
      var lines = new List<string>(){
        TextUtils.ProgressPercent("Timer", spawnTimer, obj.m_spawnIntervalSec),
        "Area: " + TextUtils.Int(obj.m_spawnRadius) + " m",
        "Level up: " + TextUtils.Percent(obj.m_levelupChance / 100f),
        "Trigger: " + TextUtils.Int(obj.m_triggerDistance) + " m",
        "Near limit: " + TextUtils.Progress(near, obj.m_maxNear) + " within " + TextUtils.Int(obj.m_nearRadius) + " m",
        "Far limit: " +  TextUtils.Progress(total, obj.m_maxTotal) + " within " + TextUtils.Int(obj.m_farRadius) + " m"
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
    public string GetHoverText() => GetHoverName() + "\n" + SpawnAreaUtils.GetText(spawnArea);
    public string GetHoverName() => TextUtils.String(spawnArea.name);
    public SpawnArea spawnArea;
  }
}