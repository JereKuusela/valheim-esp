using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(CraftingStation), "GetHoverText")]
  public class CraftingStation_GetHoverText
  {
    public static void Postfix(CraftingStation __instance, ref string __result)
    {
      var wearNTear = __instance.GetComponent<WearNTear>();
      __result += WearNTearUtils.GetText(wearNTear);
    }
  }
}
