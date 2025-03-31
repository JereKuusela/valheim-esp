using HarmonyLib;
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
  static void Postfix(Character __instance)
  {
    if (Settings.IsDisabled(Tag.CreatureCollider)) return;
    Colliders.DrawColliders(__instance, Tag.CreatureCollider);
  }
}
[HarmonyPatch(typeof(Destructible), nameof(Destructible.Awake)), HarmonyPriority(Priority.Last)]
public class Destructible_Colliders
{
  static void Postfix(Destructible __instance)
  {
    if (Settings.IsDisabled(Tag.DestructibleCollider)) return;
    Colliders.DrawColliders(__instance, Tag.DestructibleCollider);
  }
}
[HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Awake)), HarmonyPriority(Priority.Last)]
public class WearNTear_Colliders
{
  static void Postfix(WearNTear __instance)
  {
    if (Settings.IsDisabled(Tag.StructureCollider)) return;
    Colliders.DrawColliders(__instance, Tag.StructureCollider);
  }
}
