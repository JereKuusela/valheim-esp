using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
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