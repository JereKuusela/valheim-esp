using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;

namespace ESP
{
  public partial class Settings
  {
    public static ConfigEntry<bool> configShowShipStatsOnHud;
    public static bool showShipStatsOnHud => configShowShipStatsOnHud.Value;
    public static ConfigEntry<bool> configShowHud;
    public static bool showHud => configShowHud.Value;
    public static ConfigEntry<bool> configShowTimeAndWeather;
    public static bool showTimeAndWeather => configShowTimeAndWeather.Value;
    public static ConfigEntry<string> configTrackedCreatures;
    public static string trackedCreatures => configTrackedCreatures.Value;
    public static ConfigEntry<bool> configShowDPS;
    public static bool showDPS => configShowDPS.Value;
    public static ConfigEntry<bool> configShowExperienceMeter;
    public static bool showExperienceMeter => configShowExperienceMeter.Value;
    public static void InitHUD(ConfigFile config)
    {
      var section = "2. HUD";
      Settings.configShowHud = config.Bind(section, "Show HUD", true, "Show info and stats on HUD");
      Settings.configShowTimeAndWeather = config.Bind(section, "Show current time and weather", true, "Show current time and weather on the hud");
      Settings.configShowShipStatsOnHud = config.Bind(section, "Show ship stats", true, "Show ship stats on the hud");
      Settings.configTrackedCreatures = config.Bind(section, "Creatures", "Serpent", "List of creatures to track (separated by ,)");
      Settings.configShowDPS = config.Bind(section, "Show DPS meter", false, "Show DPS meter (toggle with P button in the game)");
      Settings.configShowExperienceMeter = config.Bind(section, "Show experience meter", false, "Show experience meter (toggle with L button in the game)");
    }
  }
}
