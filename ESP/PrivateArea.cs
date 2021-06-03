using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(PrivateArea), "GetHoverText")]
  public class PrivateArea_GetHoverText
  {
    public static void Postfix(PrivateArea __instance, ref string __result)
    {
      var wearNTear = __instance.GetComponent<WearNTear>();
      __result += WearNTearUtils.GetText(wearNTear);
    }
  }
}
