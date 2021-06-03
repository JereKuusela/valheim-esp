using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Vagon), "GetHoverText")]
  public class Vagon_GetHoverText
  {
    public static void Postfix(Vagon __instance, ref string __result)
    {
      var wearNTear = __instance.GetComponent<WearNTear>();
      __result += WearNTearUtils.GetText(wearNTear);
    }
  }
}
