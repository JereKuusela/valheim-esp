using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public partial class Visual
  {
    private static bool IsDisabled(string name)
    {
      if (Settings.effectAreaLineWidth == 0) return true;
      name = name.ToLower();
      var excluded = Settings.excludedAreaEffects.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return true;
      return false;
    }
    public static void Draw(EffectArea obj)
    {
      if (!obj) return;
      var text = EffectAreaUtils.GetTypeText(obj.m_type);
      if (IsDisabled(text)) return;
      var color = EffectAreaUtils.GetEffectColor(obj.m_type);
      var radius = Math.Max(0.5f, obj.GetRadius());

      var line = Drawer.DrawSphere(obj.gameObject, radius, color, Settings.effectAreaLineWidth, Drawer.OTHER);
      Drawer.AddText(line, text, Format.Radius(obj.GetRadius()));
    }
    public static void Draw(PrivateArea obj)
    {
      var text = "Protection";
      if (!obj || IsDisabled(text)) return;
      var line = Drawer.DrawSphere(obj.gameObject, obj.m_radius, Color.gray, Settings.effectAreaLineWidth, Drawer.OTHER);
      Drawer.AddText(line, text, Format.Radius(obj.m_radius));
    }
    public static void Draw(Piece obj)
    {
      if (!obj || obj.m_comfort == 0) return;
      var text = "Comfort";
      if (IsDisabled(text)) return;
      var line = Drawer.DrawSphere(obj.gameObject, 10, Color.cyan, Settings.effectAreaLineWidth, Drawer.OTHER);
      Drawer.AddText(line, text, Format.Radius(10));
    }
  }
  [HarmonyPatch(typeof(EffectArea), "Awake")]
  public class EffectArea_Awake_Visual
  {
    public static void Postfix(EffectArea __instance) => Visual.Draw(__instance);
  }
  [HarmonyPatch(typeof(PrivateArea), "Awake")]
  public class PrivateArea_Awake_Visual
  {
    public static void Postfix(PrivateArea __instance) => Visual.Draw(__instance);
  }
  [HarmonyPatch(typeof(Piece), "Awake")]
  public class Piece_Awake_Visual
  {
    public static void Postfix(Piece __instance) => Visual.Draw(__instance);
  }
}