using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Smelter), "UpdateHoverTexts")]
  public class Smelter_UpdateHoverTexts
  {
    private static string GetProgressText(Smelter instance)
    {
      if (!Settings.showProgress) return "";

      var limit = instance.m_secPerProduct;
      if (limit == 0) return "";
      var value = Patch.Smelter_GetBakeTimer(instance);
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    private static string GetFuelText(Smelter instance)
    {
      if (!Settings.showProgress) return "";

      var maxFuel = instance.m_maxFuel;
      var secPerFuel = instance.m_secPerProduct / instance.m_fuelPerProduct;
      if (maxFuel == 0) return "";
      var limit = maxFuel * secPerFuel;
      if (limit == 0) return "";
      var value = Patch.Smelter_GetFuel(instance) * secPerFuel;
      return "\n" + TextUtils.ProgressPercent("Fuel", value, limit);
    }
    private static string GetPowerText(Windmill windmill)
    {
      if (!Settings.showProgress) return "";
      if (!windmill) return "";
      var cover = Patch.Windmill_m_cover(windmill);
      var speed = Utils.LerpStep(windmill.m_minWindSpeed, 1f, EnvMan.instance.GetWindIntensity());
      var powerText = "Power: " + TextUtils.Percent(windmill.GetPowerOutput());
      var speedText = TextUtils.Percent(speed) + " speed";
      var coverText = TextUtils.Percent(cover) + " cover";
      return "\n" + powerText + " from " + speedText + " and " + coverText;
    }
    private static void UpdateSwitches(Smelter instance, string text)
    {
      var oreSwitch = instance.m_addOreSwitch;
      var woodSwitch = instance.m_addWoodSwitch;
      var emptySwitch = instance.m_emptyOreSwitch;
      if (oreSwitch) oreSwitch.m_hoverText += text;
      if (woodSwitch) woodSwitch.m_hoverText += text;
      if (emptySwitch) emptySwitch.m_hoverText += text;
    }
    public static void Postfix(Smelter __instance)
    {
      var text = GetProgressText(__instance) + GetFuelText(__instance) + GetPowerText(__instance.m_windmill);
      var wearNTear = __instance.GetComponent<WearNTear>();
      text += WearNTearUtils.GetText(wearNTear);
      UpdateSwitches(__instance, text);

    }
  }
}