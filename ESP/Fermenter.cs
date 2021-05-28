using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(Fermenter), "GetHoverText")]
  public class Fermenter_GetHoverText
  {
    // Copypaste from decompiled code.
    private static float GetFermentationTime(ZNetView m_nview)
    {
      DateTime d = new DateTime(m_nview.GetZDO().GetLong("StartTime", 0L));
      if (d.Ticks == 0L)
      {
        return -1.0f;
      }
      return (float)(ZNet.instance.GetTime() - d).TotalSeconds;
    }
    public static void Postfix(Fermenter __instance, ZNetView ___m_nview, ref string __result)
    {
      if (!Settings.showProgress)
        return;
      var value = GetFermentationTime(___m_nview);
      var limit = __instance.m_fermentationDuration;
      if (limit > 0)
        __result += "\n" + TextUtils.ProgressValue(value, limit);
    }
  }
}
