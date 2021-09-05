using BepInEx.Configuration;

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
      configShowHud = config.Bind(section, "Show HUD", true, "Show info and stats on HUD");
      configShowTimeAndWeather = config.Bind(section, "Show current time and weather", true, "Show current time and weather on the hud");
      configShowShipStatsOnHud = config.Bind(section, "Show ship stats", true, "Show ship stats on the hud");
      configTrackedCreatures = config.Bind(section, "Creatures", "Serpent", "List of creatures to track (separated by ,)");
      configShowDPS = config.Bind(section, "Show DPS meter", false, "Show DPS meter (toggle with P button in the game)");
      configShowExperienceMeter = config.Bind(section, "Show experience meter", false, "Show experience meter (toggle with L button in the game)");
    }
  }
}
