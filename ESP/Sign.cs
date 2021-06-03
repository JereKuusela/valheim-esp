using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Sign), "GetHoverText")]
  public class Sign_GetHoverText
  {
    public static void Postfix(Sign __instance, ref string __result)
    {
      var wearNTear = __instance.GetComponent<WearNTear>();
      __result += WearNTearUtils.GetText(wearNTear);
    }
  }
}
