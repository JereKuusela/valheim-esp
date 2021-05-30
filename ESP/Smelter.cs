using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(Smelter), "UpdateHoverTexts")]
  public class Smelter_UpdateHoverTexts
  {
    public static void Postfix(Smelter __instance)
    {
      if (!Settings.showProgress)
        return;
      var hover = __instance.m_addOreSwitch;
      var value = Patch.Smelter_GetBakeTimer(__instance);
      var limit = __instance.m_secPerProduct;
      if (limit > 0)
        hover.m_hoverText += "\n" + TextUtils.ProgressPercent("Progress", value, limit);
      if (__instance.m_maxFuel > 0)
      {
        value = Patch.Smelter_GetFuel(__instance) * __instance.m_secPerProduct / __instance.m_fuelPerProduct;
        limit = __instance.m_maxFuel * __instance.m_secPerProduct / __instance.m_fuelPerProduct;
        if (limit > 0)
          hover.m_hoverText += "\n" + TextUtils.ProgressPercent("Fuel", value, limit);

      }
      if (__instance.m_windmill)
      {
        hover.m_hoverText += "\n" + "Power:" + TextUtils.Percent(__instance.m_windmill.GetPowerOutput());
      }
    }
  }
}