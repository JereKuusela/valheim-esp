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
    public static void InitHUD(ConfigFile config) {
      var section = "2. HUD";
      configShowHud = config.Bind(section, "Show HUD", true, "Show info and stats on HUD");
      configShowTimeAndWeather = config.Bind(section, "Show current time and weather", true, "Show current time and weather on the hud");
      configShowPosition = config.Bind(section, "Show current position", true, "Show current position on the hud");
      configShowShipStatsOnHud = config.Bind(section, "Show ship stats", true, "Show ship stats on the hud");
      configTrackedObjects = config.Bind(section, "Tracked objects", "Serpent", "List of creature and item drop ids to track (separated by ,)");
      configTrackRadius = config.Bind(section, "Track radius", 500f, "Radius to find objects.");
    }
  }
}
