using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(Fermenter), "GetHoverText")]
  public class Fermenter_GetHoverText
  {
    public static void Postfix(Fermenter __instance, ref string __result)
    {
      if (!Settings.showProgress)
        return;
      var value = Patch.Fermenter_GetFermentationTime(__instance);
      var limit = __instance.m_fermentationDuration;
      if (limit > 0)
        __result += "\n" + TextUtils.ProgressValue("Progress", value, limit);
    }
  }
}
