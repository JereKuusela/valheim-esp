using System;
using HarmonyLib;
using Text;
using UnityEngine;

namespace ESP {
  public partial class Visual {
    private static bool IsDisabled(string name) {
      if (Settings.EffectAreaLineWidth == 0) return true;
      return LocationUtils.IsIn(Settings.ExcludedAreaEffects, name);
    }
    public static void Draw(EffectArea obj) {
      if (!obj) return;
      var text = EffectAreaUtils.GetTypeText(obj.m_type);
      if (IsDisabled(text)) return;
      var color = EffectAreaUtils.GetEffectColor(obj.m_type);
      var radius = obj.GetRadius() * obj.transform.lossyScale.x;
      var line = Visualization.Draw.DrawSphere(Tag.EffectArea, obj, Math.Max(0.5f, radius), color, Settings.EffectAreaLineWidth);
      Visualization.Draw.AddText(line, text, Format.Radius(radius));
    }
    public static void Draw(PrivateArea obj) {
      var text = "Protection";
      if (!obj || IsDisabled(text)) return;
      var line = Visualization.Draw.DrawCylinder(Tag.EffectArea, obj, obj.m_radius, Settings.EffectAreaPrivateAreaColor, Settings.EffectAreaLineWidth);
      Visualization.Draw.AddText(line, text, Format.Radius(obj.m_radius));
    }
    public static void Draw(Container obj) {
      var text = "Container";
      var radius = Settings.CustomContainerEffectAreaRadius;
      if (!obj || IsDisabled(text) || radius == 0f) return;
      var line = Visualization.Draw.DrawSphere(Tag.EffectArea, obj, Math.Max(0.5f, radius), Settings.EffectAreaCustomContainerColor, Settings.EffectAreaLineWidth);
      Visualization.Draw.AddText(line, text, Format.Radius(radius));
    }
    public static void Draw(CraftingStation obj) {
      DrawCover(obj);
      DrawCustomEffectArea(obj);
    }
    public static void DrawCustomEffectArea(CraftingStation obj) {
      var text = "Crafting station";
      var radius = Settings.CustomCraftingEffectAreaRadius;
      if (!obj || IsDisabled(text) || radius == 0f) return;
      var line = Visualization.Draw.DrawSphere(Tag.EffectArea, obj, Math.Max(0.5f, radius), Settings.EffectAreaCustomCraftingColor, Settings.EffectAreaLineWidth);
      Visualization.Draw.AddText(line, text, Format.Radius(radius));
    }
    public static void Draw(Piece obj) {
      if (!obj || obj.m_comfort == 0) return;
      var text = "Comfort";
      if (IsDisabled(text)) return;
      var line = Visualization.Draw.DrawSphere(Tag.EffectArea, obj, 10, Settings.EffectAreaComfortColor, Settings.EffectAreaLineWidth);
      Visualization.Draw.AddText(line, text, Format.Radius(10));
    }
    public static void Draw(Smoke obj) {
      if (!obj || Settings.SmokeLineWidth == 0) return;
      var collider = obj.GetComponent<SphereCollider>();
      if (collider) {
        var line = Visualization.Draw.DrawSphere(Tag.Smoke, obj, collider.radius * obj.transform.lossyScale.x, Settings.SmokeColor, Settings.SmokeLineWidth);
        Text.AddText(line);
      }
    }
  }
  [HarmonyPatch(typeof(EffectArea), "Awake")]
  public class EffectArea_Visual {
    public static void Postfix(EffectArea __instance) => Visual.Draw(__instance);
  }
  [HarmonyPatch(typeof(PrivateArea), "Awake")]
  public class PrivateArea_Visual {
    public static void Postfix(PrivateArea __instance) => Visual.Draw(__instance);
  }
  [HarmonyPatch(typeof(Piece), "Awake")]
  public class Piece_Visual {
    public static void Postfix(Piece __instance) {
      Visual.Draw(__instance);
      Visual.Draw(__instance.GetComponent<CraftingStation>());
      Visual.Draw(__instance.GetComponent<Container>());
      Visual.Draw(__instance.GetComponent<Fermenter>());
      Visual.Draw(__instance.GetComponent<Bed>());
      Visual.Draw(__instance.GetComponent<Fireplace>());
      Visual.Draw(__instance.GetComponent<Beehive>());
      Visual.Draw(__instance.GetComponent<Windmill>());
    }
  }
  [HarmonyPatch(typeof(Smoke), "Awake")]
  public class Smoke_Visual {
    public static void Postfix(Smoke __instance) => Visual.Draw(__instance);
  }
  [HarmonyPatch(typeof(CraftingStation), "Start")]
  public class CraftingStation_Updater {
    // Calls a trivial function to be used as a timer.
    public static void Postfix(CraftingStation __instance) => __instance.InvokeRepeating("GetHoverName", 2f, 2f);
  }
  [HarmonyPatch(typeof(CraftingStation), "GetHoverName")]
  public class CraftingStation_Visual_Update {
    public static void Postfix(CraftingStation __instance) => Visual.Update(__instance);
  }
  [HarmonyPatch(typeof(Fermenter), "SlowUpdate")]
  public class Fermenter_Visual_Update {
    public static void Postfix(Fermenter __instance) => Visual.Update(__instance);
  }
  [HarmonyPatch(typeof(Beehive), "Awake")]
  public class Beehive_Updater {
    // Calls a trivial function to be used as a timer.
    public static void Postfix(Beehive __instance) => __instance.InvokeRepeating("GetHoverName", 2f, 2f);
  }
  [HarmonyPatch(typeof(Beehive), "GetHoverName")]
  public class Beehive_Visual_Update {
    public static void Postfix(Beehive __instance) => Visual.Update(__instance);
  }
  [HarmonyPatch(typeof(Fireplace), "UpdateFireplace")]
  public class Fireplace_Visual_Update {
    public static void Postfix(Fireplace __instance) => Visual.Update(__instance);
  }
  [HarmonyPatch(typeof(Bed), "Awake")]
  public class Bed_Updater {
    // Calls a trivial function to be used as a timer.
    public static void Postfix(Bed __instance) => __instance.InvokeRepeating("GetHoverName", 2f, 2f);
  }
  [HarmonyPatch(typeof(Bed), "GetHoverName")]
  public class Bed_Visual_Update {
    public static void Postfix(Bed __instance) => Visual.Update(__instance);
  }
  [HarmonyPatch(typeof(Windmill), "CheckCover")]
  public class Windmill_Visual_Update {
    public static void Postfix(Windmill __instance) => Visual.Update(__instance);
  }
  [HarmonyPatch(typeof(Player), "Awake")]
  public class Player_Cover {
    public static void Postfix(Player __instance) => Visual.DrawCover(__instance);
  }
  [HarmonyPatch(typeof(Player), "LateUpdate")]
  public class Player_Cover_Update {
    public static void Postfix(Player __instance) => Visual.Update(__instance);
  }
}