using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Smelter), "UpdateHoverTexts")]
  public partial class HoverTextUtils
  {
    private static string GetProgressText(Smelter instance)
    {
      var limit = instance.m_secPerProduct;
      if (limit == 0) return "";
      var value = Patch.Smelter_GetBakeTimer(instance);
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    private static string GetFuelText(Smelter instance)
    {
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
      if (!windmill) return "";
      var cover = Patch.m_cover(windmill);
      var speed = Utils.LerpStep(windmill.m_minWindSpeed, 1f, EnvMan.instance.GetWindIntensity());
      var powerText = "Power: " + TextUtils.Percent(windmill.GetPowerOutput());
      var speedText = TextUtils.Percent(speed) + " speed";
      var coverText = TextUtils.Percent(cover) + " cover";
      return "\n" + powerText + " from " + speedText + " and " + coverText;
    }
    public static string GetText(Smelter obj)
    {
      if (!obj || !Settings.showProgress) return "";
      return GetProgressText(obj) + GetFuelText(obj) + GetPowerText(obj.m_windmill);
    }
  }

  [HarmonyPatch(typeof(Smelter), "UpdateHoverTexts")]
  public partial class HoverTextUtils
  {
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
      var text = "";
      HoverableUtils.AddTexts(__instance.gameObject, ref text);
      UpdateSwitches(__instance, text);

    }
  }
}