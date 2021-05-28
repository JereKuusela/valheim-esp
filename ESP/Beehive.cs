using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Beehive), "GetHoverText")]
  public class Beehive_GetHoverText
  {
    public static void Postfix(Beehive __instance, ZNetView ___m_nview, ref string __result)
    {
      if (!Settings.showProgress)
        return;
      var value = ___m_nview.GetZDO().GetFloat("product", 0f);
      var limit = __instance.m_secPerUnit;
      if (limit > 0)
        __result += "\n" + TextUtils.ProgressValue(value, limit);
    }
  }
}