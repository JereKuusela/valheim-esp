using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(Fermenter), "GetHoverText")]
  public class Fermenter_GetHoverText
  {
    private static string GetProgressText(Fermenter instance)
    {
      var limit = instance.m_fermentationDuration;
      if (limit == 0) return "";
      var value = Patch.Fermenter_GetFermentationTime(instance);
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static void Postfix(Fermenter __instance, ref string __result)
    {
      if (!Settings.showProgress)
        return;

      __result += GetProgressText(__instance);
    }
  }
}
