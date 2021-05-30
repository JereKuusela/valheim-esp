using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(Fireplace), "GetHoverText")]
  public class Fireplace_GetHoverText
  {
    public static void Postfix(Fireplace __instance, ZNetView ___m_nview, ref string __result)
    {
      if (!Settings.showProgress)
        return;
      var percent = ___m_nview.GetZDO().GetFloat("fuel", 0f);
      var value = percent * __instance.m_secPerFuel;
      var limit = __instance.m_secPerFuel;
      if (limit > 0)
        __result += "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
  }
}