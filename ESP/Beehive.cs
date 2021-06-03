using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Beehive), "GetHoverText")]
  public class Beehive_GetHoverText
  {
    private static string GetProgressText(Beehive instance, ZNetView nview)
    {
      if (!Settings.showProgress) return "";
      var limit = instance.m_secPerUnit;
      if (limit == 0) return "";
      var value = nview.GetZDO().GetFloat("product", 0f);
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static void Postfix(Beehive __instance, ZNetView ___m_nview, ref string __result)
    {
      var wearNTear = __instance.GetComponent<WearNTear>();
      __result += GetProgressText(__instance, ___m_nview) + WearNTearUtils.GetText(wearNTear);
    }
  }
}