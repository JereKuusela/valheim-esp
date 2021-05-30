using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ESP
{
  [HarmonyPatch(typeof(EffectArea), "Awake")]
  public class EffectArea_Awake
  {
    private static Color GetEffectColor(EffectArea.Type type)
    {
      if ((type & EffectArea.Type.Burning) != 0) return Color.yellow;
      if ((type & EffectArea.Type.Heat) != 0) return Color.magenta;
      if ((type & EffectArea.Type.Fire) != 0) return Color.red;
      if ((type & EffectArea.Type.NoMonsters) != 0) return Color.green;
      if ((type & EffectArea.Type.Teleport) != 0) return Color.blue;
      if ((type & EffectArea.Type.PlayerBase) != 0) return Color.white;
      return Color.black;
    }
    private static String GetTypeText(EffectArea.Type type)
    {
      var types = new List<string>();
      if ((type & EffectArea.Type.Burning) != 0) types.Add("Burning");
      if ((type & EffectArea.Type.Heat) != 0) types.Add("Heat");
      if ((type & EffectArea.Type.Fire) != 0) types.Add("Fire");
      if ((type & EffectArea.Type.NoMonsters) != 0) types.Add("No monsters");
      if ((type & EffectArea.Type.Teleport) != 0) types.Add("Teleport");
      if ((type & EffectArea.Type.PlayerBase) != 0) types.Add("Base");
      return types.Join(null, ", ");
    }
    private static String GetRadiusText(float radius)
    {
      return "Radius: " + TextUtils.Float(radius);
    }
    public static void Postfix(EffectArea __instance)
    {
      if (!Settings.showEffectAreas)
        return;
      var color = GetEffectColor(__instance.m_type);
      var radius = Math.Max(0.5f, __instance.GetRadius());
      var text = GetTypeText(__instance.m_type) + "\n" + GetRadiusText(__instance.GetRadius());
      Drawer.DrawSphere(__instance.gameObject, Vector3.zero, radius, color, 0.1f, text);
    }
  }
}