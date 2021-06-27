using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(HoverText), "GetHoverText")]
  public class HoverText_GetHoverText
  {
    public static void Postfix(HoverText __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Destructible), "Awake")]
  public class Destructible_Awake_AddHover
  {
    public static void Postfix(Destructible __instance) => Hoverables.AddHoverText(__instance);
  }
  [HarmonyPatch(typeof(WearNTear), "Awake")]
  public class WearNTear_Awake_AddHover
  {
    public static void Postfix(WearNTear __instance) => Hoverables.AddHoverText(__instance);
  }
  [HarmonyPatch(typeof(Beehive), "GetHoverText")]
  public class Beehive_GetHoverText
  {
    public static void Postfix(Beehive __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Chair), "GetHoverText")]
  public class Chair_GetHoverText
  {
    public static void Postfix(Chair __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Container), "GetHoverText")]
  public class Container_GetHoverText
  {
    public static void Postfix(Container __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(CookingStation), "GetHoverText")]
  public class CookingStation_GetHoverText
  {
    public static void Postfix(CookingStation __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(CraftingStation), "GetHoverText")]
  public class CraftingStation_GetHoverText
  {
    public static void Postfix(CraftingStation __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Bed), "GetHoverText")]
  public class Bed_GetHoverText
  {
    public static void Postfix(Bed __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Door), "GetHoverText")]
  public class Door_GetHoverText
  {
    public static void Postfix(Door __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Fermenter), "GetHoverText")]
  public class Fermenter_GetHoverText
  {
    public static void Postfix(Fermenter __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Fireplace), "GetHoverText")]
  public class Fireplace_GetHoverText
  {
    public static void Postfix(Fireplace __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(MineRock), "GetHoverText")]
  public class MineRock_GetHoverText
  {
    public static void Postfix(MineRock __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(MineRock5), "GetHoverText")]
  public class MineRock5_GetHoverText
  {
    public static void Postfix(MineRock5 __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Pickable), "GetHoverText")]
  public class Pickable_GetHoverText
  {
    public static void Postfix(Pickable __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Plant), "GetHoverText")]
  public class Plant_GetHoverText
  {
    public static void Postfix(Plant __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(PrivateArea), "GetHoverText")]
  public class PrivateArea_GetHoverText
  {
    public static void Postfix(PrivateArea __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Sign), "GetHoverText")]
  public class Sign_GetHoverText
  {
    public static void Postfix(Sign __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(ItemStand), "GetHoverText")]
  public class ItemStand_GetHoverText
  {
    public static void Postfix(ItemStand __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(StationExtension), "GetHoverText")]
  public class StationExtension_GetHoverText
  {
    public static void Postfix(StationExtension __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Vagon), "GetHoverText")]
  public class Vagon_GetHoverText
  {
    public static void Postfix(Vagon __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(TeleportWorld), "GetHoverText")]
  public class TeleportWorld_GetHoverText
  {
    public static void Postfix(TeleportWorld __instance, ref string __result) => Hoverables.AddTexts(__instance.gameObject, ref __result);
  }
  [HarmonyPatch(typeof(Smelter), "UpdateHoverTexts")]
  public partial class HoverTextUtils
  {
    private static void UpdateSwitches(Smelter instance, string text)
    {
      var oreSwitch = instance.m_addOreSwitch;
      var woodSwitch = instance.m_addWoodSwitch;
      var emptySwitch = instance.m_emptyOreSwitch;
      if (oreSwitch) oreSwitch.m_hoverText += text;
      if (woodSwitch) woodSwitch.m_hoverText += text;
      if (emptySwitch) emptySwitch.m_hoverText += text;
    }
    public static void Postfix(Smelter __instance)
    {
      var text = "";
      Hoverables.AddTexts(__instance.gameObject, ref text);
      UpdateSwitches(__instance, text);
    }
  }
}