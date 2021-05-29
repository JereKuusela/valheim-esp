using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Plant), "GetHoverText")]
  public class Plant_GetHoverText
  {
    public static void Postfix(Plant __instance, ref string __result)
    {
      if (!Settings.showProgress)
        return;
      var value = Patch.Plant_TimeSincePlanted(__instance);
      var limit = Patch.Plant_GetGrowTime(__instance);
      if (limit > 0)
        __result += "\n" + TextUtils.ProgressValue("Progress", value, limit);
    }
  }
}