using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Door), "GetHoverText")]
  public class Door_GetHoverText
  {
    public static void Postfix(Door __instance, ref string __result)
    {
      var wearNTear = __instance.GetComponent<WearNTear>();
      __result += WearNTearUtils.GetText(wearNTear);
    }
  }
}
