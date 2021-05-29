using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(Smelter), "UpdateHoverTexts")]
  public class Smelter_UpdateHoverTexts
  {
    // Copypaste from decompiled.§
    private static float GetBakeTimer(ZNetView m_nview)
    {
      return m_nview.GetZDO().GetFloat("bakeTimer", 0f);
    }
    // Copypaste from decompiled.
    private static float GetFuel(ZNetView m_nview)
    {
      return m_nview.GetZDO().GetFloat("fuel", 0f);
    }
    public static void Postfix(Smelter __instance, ZNetView ___m_nview)
    {
      if (!Settings.showProgress)
        return;
      var hover = __instance.m_addOreSwitch;
      var value = GetBakeTimer(___m_nview);
      var limit = __instance.m_secPerProduct;
      if (limit > 0)
        hover.m_hoverText += "\n" + TextUtils.ProgressValue("Progress", value, limit);
      if (__instance.m_maxFuel > 0)
      {
        value = GetFuel(___m_nview) * __instance.m_secPerProduct / __instance.m_fuelPerProduct;
        limit = __instance.m_maxFuel * __instance.m_secPerProduct / __instance.m_fuelPerProduct;
        if (limit > 0)
          hover.m_hoverText += "\n" + TextUtils.ProgressValue("Fuel", value, limit);

      }
      if (__instance.m_windmill)
      {
        hover.m_hoverText += "\n" + "Power:" + TextUtils.PercentValue(__instance.m_windmill.GetPowerOutput());
      }
    }
  }
}