using HarmonyLib;

namespace ESP
{

  [HarmonyPatch(typeof(HoverText), "GetHoverText")]
  public class HoverText_GetHoverText
  {
    public static void Postfix(HoverText __instance, ref string __result)
    {
      var obj = __instance;
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<TreeLog>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<TreeBase>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<Destructible>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<Pickable>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<CreatureSpawner>());
    }
  }
}