using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Beehive), "GetHoverText")]
  public class Beehive_GetHoverText
  {
    private static string GetProgressText(Beehive instance, ZNetView nview)
    {
      var limit = instance.m_secPerUnit;
      if (limit == 0) return "";
      var value = nview.GetZDO().GetFloat("product", 0f);
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static void Postfix(Beehive __instance, ZNetView ___m_nview, ref string __result)
    {
      if (!Settings.showProgress)
        return;

      __result += GetProgressText(__instance, ___m_nview);
    }
  }
}