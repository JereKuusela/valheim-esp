using HarmonyLib;
using Service;
using UnityEngine;
using Visualization;
namespace ESP;

public class Colliders
{


  public static void DrawColliders(MonoBehaviour obj, string tag)
  {
    var colliders = obj.GetComponentsInChildren<Collider>();
    foreach (var collider in colliders)
    {
      if (collider.isTrigger) continue;
      if (collider is BoxCollider box) Draw.DrawBox(tag, obj, box.size);
      if (collider is SphereCollider sphere) Draw.DrawSphere(tag, obj, sphere.radius);
      if (collider is CapsuleCollider capsule) Draw.DrawCapsule(tag, obj, capsule.radius, capsule.height);
    }
  }
}

[HarmonyPatch(typeof(Character), nameof(Character.Awake)), HarmonyPriority(Priority.Last)]
public class Character_Colliders
{
  public static void RebuildLoaded()
  {
    foreach (var obj in SceneObjects.FindLoaded<Character>())
      Postfix(obj);
  }
  static void Postfix(Character __instance)
  {
    if (Settings.IsDisabled(Tag.CreatureCollider)) return;
    if (Draw.HasVisual(__instance, Tag.CreatureCollider)) return;
    Colliders.DrawColliders(__instance, Tag.CreatureCollider);
  }
}
[HarmonyPatch(typeof(Destructible), nameof(Destructible.Awake)), HarmonyPriority(Priority.Last)]
public class Destructible_Colliders
{
  public static void RebuildLoaded()
  {
    foreach (var obj in SceneObjects.FindLoaded<Destructible>())
      Postfix(obj);
  }
  static void Postfix(Destructible __instance)
  {
    if (Settings.IsDisabled(Tag.DestructibleCollider)) return;
    if (Draw.HasVisual(__instance, Tag.DestructibleCollider)) return;
    Colliders.DrawColliders(__instance, Tag.DestructibleCollider);
  }
}
[HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Awake)), HarmonyPriority(Priority.Last)]
public class WearNTear_Colliders
{
  public static void RebuildLoaded()
  {
    foreach (var obj in SceneObjects.FindLoaded<WearNTear>())
      Postfix(obj);
  }
  static void Postfix(WearNTear __instance)
  {
    if (Settings.IsDisabled(Tag.StructureCollider)) return;
    if (Draw.HasVisual(__instance, Tag.StructureCollider)) return;
    Colliders.DrawColliders(__instance, Tag.StructureCollider);
  }
}
