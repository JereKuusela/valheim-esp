using BepInEx.Configuration;
namespace ESP;
public partial class Settings
{
#nullable disable
  public static ConfigEntry<bool> configShowShipStatsOnHud;
  public static bool ShowShipStatsOnHud => configShowShipStatsOnHud.Value;
  public static ConfigEntry<bool> configShowHud;
  public static bool ShowHud => configShowHud.Value;
  public static ConfigEntry<bool> configShowTime;
  public static bool ShowTime => configShowTime.Value;
  public static ConfigEntry<bool> configShowPosition;
  public static bool ShowPosition => configShowPosition.Value;
  public static ConfigEntry<bool> configShowAltitude;
  public static bool ShowAltitude => configShowAltitude.Value;
  public static ConfigEntry<bool> configShowForest;
  public static bool ShowForest => configShowForest.Value;
  public static ConfigEntry<bool> configShowBlocked;
  public static bool ShowBlocked => configShowBlocked.Value;

  public static ConfigEntry<string> configTrackedObjects;
  public static string TrackedObjects => configTrackedObjects.Value;
  public static ConfigEntry<float> configTrackRadius;
  public static float TrackRadius => configTrackRadius.Value;
  public static ConfigEntry<bool> configShowStagger;
  public static bool ShowStagger => configShowStagger.Value;
  public static ConfigEntry<bool> configShowHeat;
  public static bool ShowHeat => configShowHeat.Value;
  public static ConfigEntry<bool> configShowSpeed;
  public static bool ShowSpeed => configShowSpeed.Value;
  public static ConfigEntry<bool> configShowStealth;
  public static bool ShowStealth => configShowStealth.Value;
  public static ConfigEntry<bool> configShowWeather;
  public static bool ShowWeather => configShowWeather.Value;
  public static ConfigEntry<bool> configShowWind;
  public static bool ShowWind => configShowWind.Value;
  public static void InitHUD(ConfigFile config)
  {
    var section = "1. HUD";
    configShowHud = config.Bind(section, "Show HUD", false, "Show info and stats on HUD");
    configShowTime = config.Bind(section, "Show time", true, "Show time");
    configShowWeather = config.Bind(section, "Show weather", true, "Show weather");
    configShowWind = config.Bind(section, "Show wind", false, "Show wind");
    configShowPosition = config.Bind(section, "Show position", true, "Show position");
    configShowAltitude = config.Bind(section, "Show altitude", false, "Show altitude");
    configShowForest = config.Bind(section, "Show forest", false, "Show forest");
    configShowBlocked = config.Bind(section, "Show blocked", false, "Show whether the sky is blocked");
    configShowStagger = config.Bind(section, "Show stagger timer", false, "Shows stagger timer");
    configShowHeat = config.Bind(section, "Show heat", false, "Shows heat");
    configShowSpeed = config.Bind(section, "Show speed", false, "Shows speed");
    configShowStealth = config.Bind(section, "Show stealth", false, "Shows stealth");
    configShowShipStatsOnHud = config.Bind(section, "Show ship stats", true, "Show ship stats on the hud");
    configTrackedObjects = config.Bind(section, "Tracked objects", "Serpent", "List of objects to track (separated by ,)");
    configTrackRadius = config.Bind(section, "Track radius", 500f, "Radius to find objects.");
  }
}
