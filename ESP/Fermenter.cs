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
      if (!Settings.showProgress) return "";
      var limit = instance.m_fermentationDuration;
      if (limit == 0) return "";
      var value = Patch.Fermenter_GetFermentationTime(instance);
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static void Postfix(Fermenter __instance, ref string __result)
    {
      __result += GetProgressText(__instance);

      var wearNTear = __instance.GetComponent<WearNTear>();
      __result += WearNTearUtils.GetText(wearNTear);
    }
  }
}
