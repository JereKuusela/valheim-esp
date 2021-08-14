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
      return LocationUtils.IsIn(Settings.excludedAreaEffects, name);
    }
    public static void Draw(EffectArea obj)
    {
      if (!obj) return;
      var text = EffectAreaUtils.GetTypeText(obj.m_type);
      if (IsDisabled(text)) return;
      var color = EffectAreaUtils.GetEffectColor(obj.m_type);
      var radius = obj.GetRadius() * obj.transform.lossyScale.x;

      var line = Drawer.DrawSphere(obj, Math.Max(0.5f, radius), color, Settings.effectAreaLineWidth, Drawer.OTHER);
      Drawer.AddText(line, text, Format.Radius(radius));
    }
    public static void Draw(PrivateArea obj)
    {
      var text = "Protection";
      if (!obj || IsDisabled(text)) return;
      var line = Drawer.DrawCylinder(obj, obj.m_radius, Color.gray, Settings.effectAreaLineWidth, Drawer.OTHER);
      Drawer.AddText(line, text, Format.Radius(obj.m_radius));
    }
    public static void Draw(Piece obj)
    {
      if (!obj || obj.m_comfort == 0) return;
      var text = "Comfort";
      if (IsDisabled(text)) return;
      var line = Drawer.DrawSphere(obj, 10, Color.cyan, Settings.effectAreaLineWidth, Drawer.OTHER);
      Drawer.AddText(line, text, Format.Radius(10));
    }
    public static void Draw(Smoke obj)
    {
      if (!obj || Settings.smokeLineWidth == 0) return;
      var collider = obj.GetComponent<SphereCollider>();
      if (collider)
      {
        var line = Drawer.DrawSphere(obj, collider.radius * obj.transform.lossyScale.x, Color.black, Settings.smokeLineWidth, Drawer.OTHER);
        Drawer.AddText(line);
      }
    }
  }
  [HarmonyPatch(typeof(EffectArea), "Awake")]
  public class EffectArea_Visual
  {
    public static void Postfix(EffectArea __instance) => Visual.Draw(__instance);
  }
  [HarmonyPatch(typeof(PrivateArea), "Awake")]
  public class PrivateArea_Visual
  {
    public static void Postfix(PrivateArea __instance) => Visual.Draw(__instance);
  }
  [HarmonyPatch(typeof(Piece), "Awake")]
  public class Piece_Visual
  {
    public static void Postfix(Piece __instance)
    {
      Visual.Draw(__instance);
      Visual.Draw(__instance.GetComponent<CraftingStation>());
      Visual.Draw(__instance.GetComponent<Fermenter>());
      Visual.Draw(__instance.GetComponent<Bed>());
      Visual.Draw(__instance.GetComponent<Fireplace>());
      Visual.Draw(__instance.GetComponent<Beehive>());
      Visual.Draw(__instance.GetComponent<Windmill>());
    }
  }
  [HarmonyPatch(typeof(Smoke), "Awake")]
  public class Smoke_Visual
  {
    public static void Postfix(Smoke __instance) => Visual.Draw(__instance);
  }
  [HarmonyPatch(typeof(CraftingStation), "Start")]
  public class CraftingStation_Updater
  {
    // Calls a trivial function to be used as a timer.
    public static void Postfix(CraftingStation __instance) => __instance.InvokeRepeating("GetHoverName", 2f, 2f);
  }
  [HarmonyPatch(typeof(CraftingStation), "GetHoverName")]
  public class CraftingStation_Visual_Update
  {
    public static void Postfix(CraftingStation __instance) => Visual.Update(__instance);
  }
  [HarmonyPatch(typeof(Fermenter), "SlowUpdate")]
  public class Fermenter_Visual_Update
  {
    public static void Postfix(Fermenter __instance) => Visual.Update(__instance);
  }
  [HarmonyPatch(typeof(Beehive), "Awake")]
  public class Beehive_Updater
  {
    // Calls a trivial function to be used as a timer.
    public static void Postfix(Beehive __instance) => __instance.InvokeRepeating("GetHoverName", 2f, 2f);
  }
  [HarmonyPatch(typeof(Beehive), "GetHoverName")]
  public class Beehive_Visual_Update
  {
    public static void Postfix(Beehive __instance) => Visual.Update(__instance);
  }
  [HarmonyPatch(typeof(Fireplace), "UpdateFireplace")]
  public class Fireplace_Visual_Update
  {
    public static void Postfix(Fireplace __instance) => Visual.Update(__instance);
  }
  [HarmonyPatch(typeof(Bed), "Awake")]
  public class Bed_Updater
  {
    // Calls a trivial function to be used as a timer.
    public static void Postfix(Bed __instance) => __instance.InvokeRepeating("GetHoverName", 2f, 2f);
  }
  [HarmonyPatch(typeof(Bed), "GetHoverName")]
  public class Bed_Visual_Update
  {
    public static void Postfix(Bed __instance) => Visual.Update(__instance);
  }
  [HarmonyPatch(typeof(Windmill), "CheckCover")]
  public class Windmill_Visual_Update
  {
    public static void Postfix(Windmill __instance) => Visual.Update(__instance);
  }
}