using HarmonyLib;
namespace ESP;
[HarmonyPatch(typeof(HoverText), nameof(HoverText.GetHoverText))]
public class HoverText_GetHoverText
{
  static void Postfix(HoverText __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(SpawnArea), nameof(SpawnArea.Awake)), HarmonyPriority(Priority.Last)]
public class SpawnArea_Awake_AddHover
{
  static void Postfix(SpawnArea __instance) => Text.AddHoverText(__instance);
}
[HarmonyPatch(typeof(Destructible), nameof(Destructible.Awake)), HarmonyPriority(Priority.Last)]
public class Destructible_Awake_AddHover
{
  static void Postfix(Destructible __instance) => Text.AddHoverText(__instance);
}
[HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Awake)), HarmonyPriority(Priority.Last)]
public class WearNTear_Awake_AddHover
{
  static void Postfix(WearNTear __instance) => Text.AddHoverText(__instance);
}
[HarmonyPatch(typeof(Beehive), nameof(Beehive.GetHoverText))]
public class Beehive_GetHoverText
{
  static void Postfix(Beehive __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.GetHoverText))]
public class ItemDrop_GetHoverText
{
  static void Postfix(Beehive __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(OfferingBowl), nameof(OfferingBowl.GetHoverText))]
public class OfferingBowl_GetHoverText
{
  static void Postfix(OfferingBowl __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(Chair), nameof(Chair.GetHoverText))]
public class Chair_GetHoverText
{
  static void Postfix(Chair __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(Container), nameof(Container.GetHoverText))]
public class Container_GetHoverText
{
  static void Postfix(Container __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(CookingStation), nameof(CookingStation.GetHoverText))]
public class CookingStation_GetHoverText
{
  static void Postfix(CookingStation __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(CraftingStation), nameof(CraftingStation.GetHoverText))]
public class CraftingStation_GetHoverText
{
  static void Postfix(CraftingStation __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(Bed), nameof(Bed.GetHoverText))]
public class Bed_GetHoverText
{
  static void Postfix(Bed __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(Door), nameof(Door.GetHoverText))]
public class Door_GetHoverText
{
  static void Postfix(Door __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(Fermenter), nameof(Fermenter.GetHoverText))]
public class Fermenter_GetHoverText
{
  static void Postfix(Fermenter __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(Fireplace), nameof(Fireplace.GetHoverText))]
public class Fireplace_GetHoverText
{
  static void Postfix(Fireplace __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(MineRock), nameof(MineRock.GetHoverText))]
public class MineRock_GetHoverText
{
  static void Postfix(MineRock __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(MineRock5), nameof(MineRock5.GetHoverText))]
public class MineRock5_GetHoverText
{
  static void Postfix(MineRock5 __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(Pickable), nameof(Pickable.GetHoverText))]
public class Pickable_GetHoverText
{
  static void Postfix(Pickable __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(Plant), nameof(Plant.GetHoverText))]
public class Plant_GetHoverText
{
  static void Postfix(Plant __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(PrivateArea), nameof(PrivateArea.GetHoverText))]
public class PrivateArea_GetHoverText
{
  static void Postfix(PrivateArea __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(Sign), nameof(Sign.GetHoverText))]
public class Sign_GetHoverText
{
  static void Postfix(Sign __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(ItemStand), nameof(ItemStand.GetHoverText))]
public class ItemStand_GetHoverText
{
  static void Postfix(ItemStand __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(StationExtension), nameof(StationExtension.GetHoverText))]
public class StationExtension_GetHoverText
{
  static void Postfix(StationExtension __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(Vagon), nameof(Vagon.GetHoverText))]
public class Vagon_GetHoverText
{
  static void Postfix(Vagon __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(TeleportWorld), nameof(TeleportWorld.GetHoverText))]
public class TeleportWorld_GetHoverText
{
  static void Postfix(TeleportWorld __instance, ref string __result) => Text.AddTexts(__instance.gameObject, ref __result);
}
[HarmonyPatch(typeof(Smelter), nameof(Smelter.OnHoverAddOre))]
public class OnHoverAddOre
{
  static string Postfix(string result, Smelter __instance)
  {
    Text.AddTexts(__instance.gameObject, ref result);
    return result;
  }
}
[HarmonyPatch(typeof(Smelter), nameof(Smelter.OnHoverAddFuel))]
public class OnHoverAddFuel
{
  static string Postfix(string result, Smelter __instance)
  {
    Text.AddTexts(__instance.gameObject, ref result);
    return result;
  }
}

[HarmonyPatch(typeof(Smelter), nameof(Smelter.OnHoverEmptyOre))]
public class OnHoverEmptyOre
{
  static string Postfix(string result, Smelter __instance)
  {
    Text.AddTexts(__instance.gameObject, ref result);
    return result;
  }
}

