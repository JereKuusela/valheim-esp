using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(Fireplace), "GetHoverText")]
  public class Fireplace_GetHoverText
  {
    private static string GetFuelText(Fireplace instance, ZNetView nview)
    {
      var limit = instance.m_secPerFuel;
      if (limit == 0) return "";
      var value = nview.GetZDO().GetFloat("fuel", 0f) * limit;
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static void Postfix(Fireplace __instance, ZNetView ___m_nview, ref string __result)
    {
      if (!Settings.showProgress)
        return;

      __result += GetFuelText(__instance, ___m_nview);
    }
  }
}