using System;
using HarmonyLib;
using Service;
using UnityEngine;
namespace ESP;

public partial class Visual
{
  public static void Draw(EffectArea obj)
  {
    if (!obj || !obj.m_collider) return;
    var tag = Tag.GetEffectArea(obj.m_type);
    if (Settings.IsDisabled(tag)) return;
    if (Visualization.Draw.HasVisual(obj, tag)) return;
    // Player base uses a very tall capsule collider to ignore terrain elevation.
    // Use ruler to visualize it on the terrain where spawns happen.
    if (obj.m_type == EffectArea.Type.PlayerBase && obj.m_collider is CapsuleCollider)
    {
      if (obj.GetComponent<Visualization.CircleRuler>()) return;
      var ruler = obj.gameObject.AddComponent<Visualization.CircleRuler>();
      ruler.Radius = obj.GetRadius();
      return;
    }
    var text = EffectAreaUtils.GetTypeText(obj.m_type);
    var radius = obj.GetRadius() * obj.transform.lossyScale.x;
    var line = Visualization.Draw.DrawSphere(tag, obj, Math.Max(0.5f, radius));
    Visualization.Draw.AddText(line, text, Text.Radius(radius));
  }
  public static void Draw(PrivateArea obj)
  {
    if (!obj || Settings.IsDisabled(Tag.EffectAreaPrivateArea)) return;
    if (Visualization.Draw.HasVisual(obj, Tag.EffectAreaPrivateArea)) return;
    var line = Visualization.Draw.DrawCylinder(Tag.EffectAreaPrivateArea, obj, obj.m_radius);
    Visualization.Draw.AddText(line, "Protection", Text.Radius(obj.m_radius));
  }
  public static void Draw(Container obj)
  {
    var radius = Settings.CustomContainerEffectAreaRadius;
    if (!obj || Settings.IsDisabled(Tag.EffectAreaCustomContainer) || radius == 0f) return;
    if (Visualization.Draw.HasVisual(obj, Tag.EffectAreaCustomContainer)) return;
    var line = Visualization.Draw.DrawSphere(Tag.EffectAreaCustomContainer, obj, Math.Max(0.5f, radius));
    Visualization.Draw.AddText(line, "Container", Text.Radius(radius));
  }
  public static void Draw(CraftingStation obj)
  {
    DrawCover(obj);
    DrawCustomEffectArea(obj);
  }
  public static void DrawCustomEffectArea(CraftingStation obj)
  {
    var radius = Settings.CustomCraftingEffectAreaRadius;
    if (!obj || Settings.IsDisabled(Tag.EffectAreaCustomCrafting) || radius == 0f) return;
    if (Visualization.Draw.HasVisual(obj, Tag.EffectAreaCustomCrafting)) return;
    var line = Visualization.Draw.DrawSphere(Tag.EffectAreaCustomCrafting, obj, Math.Max(0.5f, radius));
    Visualization.Draw.AddText(line, "Crafting station", Text.Radius(radius));
  }
  public static void Draw(Piece obj)
  {
    if (!obj || obj.m_comfort == 0) return;
    if (Settings.IsDisabled(Tag.EffectAreaComfort)) return;
    if (Visualization.Draw.HasVisual(obj, Tag.EffectAreaComfort)) return;
    var line = Visualization.Draw.DrawSphere(Tag.EffectAreaComfort, obj, Constants.ComfortRadius);
    Visualization.Draw.AddText(line, $"Comfort {obj.m_comfort} {obj.m_comfortGroup}", Text.Radius(10));
  }
  public static void Draw(Smoke obj)
  {
    if (!obj || Settings.IsDisabled(Tag.Smoke)) return;
    if (Visualization.Draw.HasVisual(obj, Tag.Smoke)) return;
    var collider = obj.GetComponent<SphereCollider>();
    if (collider)
    {
      var line = Visualization.Draw.DrawSphere(Tag.Smoke, obj, collider.radius * obj.transform.lossyScale.x);
      Text.AddText(line);
    }
  }
}
[HarmonyPatch(typeof(EffectArea), nameof(EffectArea.Awake)), HarmonyPriority(Priority.Last)]
public class EffectArea_Visual
{
  public static void RebuildLoaded()
  {
    foreach (var obj in SceneObjects.FindLoaded<EffectArea>())
      Postfix(obj);
  }
  static void Postfix(EffectArea __instance) => Visual.Draw(__instance);
}
[HarmonyPatch(typeof(PrivateArea), nameof(PrivateArea.Awake)), HarmonyPriority(Priority.Last)]
public class PrivateArea_Visual
{
  public static void RebuildLoaded()
  {
    foreach (var obj in SceneObjects.FindLoaded<PrivateArea>())
      Postfix(obj);
  }
  static void Postfix(PrivateArea __instance) => Visual.Draw(__instance);
}
[HarmonyPatch(typeof(Piece), nameof(Piece.Awake)), HarmonyPriority(Priority.Last)]
public class Piece_Visual
{
  public static void RebuildLoaded()
  {
    foreach (var obj in SceneObjects.FindLoaded<Piece>())
      Postfix(obj);
  }
  static void Postfix(Piece __instance)
  {
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
[HarmonyPatch(typeof(Smoke), nameof(Smoke.Awake)), HarmonyPriority(Priority.Last)]
public class Smoke_Visual
{
  public static void RebuildLoaded()
  {
    foreach (var obj in SceneObjects.FindLoaded<Smoke>())
      Postfix(obj);
  }
  static void Postfix(Smoke __instance) => Visual.Draw(__instance);
}
[HarmonyPatch(typeof(CraftingStation), nameof(CraftingStation.Start))]
public class CraftingStation_Updater
{
  // Calls a trivial function to be used as a timer.
  static void Postfix(CraftingStation __instance) => __instance.InvokeRepeating("GetHoverName", 2f, 2f);
}
[HarmonyPatch(typeof(CraftingStation), nameof(CraftingStation.GetHoverName))]
public class CraftingStation_Visual_Update
{
  static void Postfix(CraftingStation __instance) => Visual.Update(__instance);
}
[HarmonyPatch(typeof(Fermenter), nameof(Fermenter.SlowUpdate))]
public class Fermenter_Visual_Update
{
  static void Postfix(Fermenter __instance) => Visual.Update(__instance);
}
[HarmonyPatch(typeof(Beehive), nameof(Beehive.Awake)), HarmonyPriority(Priority.Last)]
public class Beehive_Updater
{
  // Calls a trivial function to be used as a timer.
  static void Postfix(Beehive __instance) => __instance.InvokeRepeating("GetHoverName", 2f, 2f);
}
[HarmonyPatch(typeof(Beehive), nameof(Beehive.GetHoverName))]
public class Beehive_Visual_Update
{
  static void Postfix(Beehive __instance) => Visual.Update(__instance);
}
[HarmonyPatch(typeof(Fireplace), nameof(Fireplace.UpdateFireplace))]
public class Fireplace_Visual_Update
{
  static void Postfix(Fireplace __instance) => Visual.Update(__instance);
}
[HarmonyPatch(typeof(Bed), nameof(Bed.Awake)), HarmonyPriority(Priority.Last)]
public class Bed_Updater
{
  // Calls a trivial function to be used as a timer.
  static void Postfix(Bed __instance) => __instance.InvokeRepeating("GetHoverName", 2f, 2f);
}
[HarmonyPatch(typeof(Bed), nameof(Bed.GetHoverName))]
public class Bed_Visual_Update
{
  static void Postfix(Bed __instance) => Visual.Update(__instance);
}
[HarmonyPatch(typeof(Windmill), nameof(Windmill.CheckCover))]
public class Windmill_Visual_Update
{
  static void Postfix(Windmill __instance) => Visual.Update(__instance);
}
[HarmonyPatch(typeof(Player), nameof(Player.Awake)), HarmonyPriority(Priority.Last)]
public class Player_Cover
{
  public static void RebuildLoaded()
  {
    foreach (var obj in SceneObjects.FindLoaded<Player>())
      Postfix(obj);
  }
  static void Postfix(Player __instance) => Visual.DrawCover(__instance);
}
[HarmonyPatch(typeof(Player), nameof(Player.LateUpdate))]
public class Player_Cover_Update
{
  static void Postfix(Player __instance) => Visual.Update(__instance);
}
