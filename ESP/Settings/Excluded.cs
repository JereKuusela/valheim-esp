using BepInEx.Configuration;

namespace ESP
{
  public partial class Settings
  {
    public static ConfigEntry<string> configExcludedAreaEffects;
    public static string excludedAreaEffects => configExcludedAreaEffects.Value;
    public static ConfigEntry<string> configExcludedCreatures;
    public static string excludedCreatures => configExcludedCreatures.Value;
    public static ConfigEntry<string> configExcludedCreatureSpawners;
    public static string excludedCreatureSpawners => configExcludedCreatureSpawners.Value;
    public static ConfigEntry<string> configExcludedSpawnSystems;
    public static string excludedSpawnSystems => configExcludedSpawnSystems.Value;
    public static ConfigEntry<string> configExcludedResources;
    public static string excludedResources => configExcludedResources.Value;
    public static ConfigEntry<bool> configShowOthers;
    public static bool showOthers => configShowOthers.Value;
    public static ConfigEntry<bool> configShowZones;
    public static bool showZones => configShowZones.Value;
    public static ConfigEntry<bool> configShowCreatures;
    public static bool showCreatures => configShowCreatures.Value;

    public static void InitExcluded(ConfigFile config)
    {
      var section = "5. Excluded";
      Settings.configExcludedAreaEffects = config.Bind(section, "Area effects", "", "List of area effects separated by ,");
      Settings.configExcludedCreatures = config.Bind(section, "Creatures", "", "List of creatures separated by ,");
      Settings.configExcludedSpawnSystems = config.Bind(section, "Spawn systems", "Seagal,FireFlies", "List of creatures separated by , that are not visualized");
      Settings.configExcludedCreatureSpawners = config.Bind(section, "Spawn points", "", "List of creatures separated by , that are not visualized");
      Settings.configExcludedResources = config.Bind(section, "Resources", "Wood,Stone,Rock*,*RockPillar", "List of resources separated by , that are not visualized");
    }
  }
}
