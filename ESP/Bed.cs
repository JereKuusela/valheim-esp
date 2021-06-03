using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Bed), "GetHoverText")]
  public class Bed_GetHoverText
  {
    public static void Postfix(Bed __instance, ref string __result)
    {
      var wearNTear = __instance.GetComponent<WearNTear>();
      __result += WearNTearUtils.GetText(wearNTear);
    }
  }
}
