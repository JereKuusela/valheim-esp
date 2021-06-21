using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public class AreaEffectUtils
  {
    private static bool IsEnabled(EffectArea obj)
    {
      if (Settings.creatureSpawnersRayWidth == 0) return false;
      var name = obj.name.ToLower();
      var excluded = Settings.excludedCreatureSpawners.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return false;
      return true;
    }
  }
  [HarmonyPatch(typeof(EffectArea), "Awake")]
  public class EffectArea_Awake
  {
    public static void Postfix(EffectArea __instance)
    {
      if (Settings.effectAreaLineWidth == 0) return;
      var color = EffectAreaUtils.GetEffectColor(__instance.m_type);
      var radius = Math.Max(0.5f, __instance.GetRadius());

      var obj = Drawer.DrawSphere(__instance.gameObject, radius, color, Settings.effectAreaLineWidth, Drawer.OTHER);
      Drawer.AddText(obj, EffectAreaUtils.GetTypeText(__instance.m_type), Format.Radius(__instance.GetRadius()));
    }
  }
  [HarmonyPatch(typeof(PrivateArea), "Awake")]
  public class PrivateArea_Awake
  {
    public static void Postfix(PrivateArea __instance)
    {
      if (Settings.effectAreaLineWidth == 0) return;
      var obj = Drawer.DrawSphere(__instance.gameObject, __instance.m_radius, Color.gray, Settings.effectAreaLineWidth, Drawer.OTHER);
      Drawer.AddText(obj, "Protection", Format.Radius(__instance.m_radius));
    }
  }
}