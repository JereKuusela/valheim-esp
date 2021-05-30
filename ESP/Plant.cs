using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Plant), "GetHoverText")]
  public class Plant_GetHoverText
  {
    private static string GetProgressText(Plant instance)
    {
      var limit = Patch.Plant_GetGrowTime(instance);
      if (limit == 0) return "";
      var value = Patch.Plant_TimeSincePlanted(instance);
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static void Postfix(Plant __instance, ref string __result)
    {
      if (!Settings.showProgress)
        return;

      __result += GetProgressText(__instance);
    }
  }
}