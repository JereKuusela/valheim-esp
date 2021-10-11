using BepInEx.Configuration;

namespace ESP {
  public partial class Settings {
    public static ConfigEntry<bool> configShowShipStatsOnHud;
    public static bool ShowShipStatsOnHud => configShowShipStatsOnHud.Value;
    public static ConfigEntry<bool> configShowHud;
    public static bool ShowHud => configShowHud.Value;
    public static ConfigEntry<bool> configShowTimeAndWeather;
    public static bool ShowTimeAndWeather => configShowTimeAndWeather.Value;
    public static ConfigEntry<bool> configShowPosition;
    public static bool ShowPosition => configShowPosition.Value;
    public static ConfigEntry<string> configTrackedObjects;
    public static string TrackedObjects => configTrackedObjects.Value;
    public static ConfigEntry<float> configTrackRadius;
    public static float TrackRadius => configTrackRadius.Value;
    public static ConfigEntry<bool> configShowDPS;
    public static bool ShowDPS => configShowDPS.Value;
    public static ConfigEntry<bool> configShowExperienceMeter;
    public static bool ShowExperienceMeter => configShowExperienceMeter.Value;
    public static ConfigEntry<bool> configShowRuler;
    public static void InitHUD(ConfigFile config) {
      var section = "2. HUD";
      configShowHud = config.Bind(section, "Show HUD", true, "Show info and stats on HUD");
      configShowTimeAndWeather = config.Bind(section, "Show current time and weather", true, "Show current time and weather on the hud");
      configShowPosition = config.Bind(section, "Show current position", true, "Show current position on the hud");
      configShowShipStatsOnHud = config.Bind(section, "Show ship stats", true, "Show ship stats on the hud");
      configTrackedObjects = config.Bind(section, "Tracked objects", "Serpent", "List of creature and item drop ids to track (separated by ,)");
      configTrackRadius = config.Bind(section, "Track radius", 500f, "Radius to find objects.");
      configShowDPS = config.Bind(section, "Show DPS meter", false, "Show DPS meter (toggle with P button in the game)");
      configShowDPS.SettingChanged += (s, e) => {
        if (!ShowDPS) DPSMeter.Reset();
      };
      configShowExperienceMeter = config.Bind(section, "Show experience meter", false, "Show experience meter (toggle with L button in the game)");
      configShowExperienceMeter.SettingChanged += (s, e) => {
        if (!ShowExperienceMeter) ExperienceMeter.Reset();
      };
      configShowRuler = config.Bind(section, "Ruler enables", false, "Setting true enables rules at current location.");
      configShowRuler.SettingChanged += (s, e) => {
        if (configShowRuler.Value)
          Ruler.Set(Player.m_localPlayer.transform.position);
        else
          Ruler.Reset();
      };
    }
  }
}
