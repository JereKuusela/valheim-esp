using BepInEx.Configuration;
namespace ESP;
public partial class Settings {
  public static ConfigEntry<string> configExcludedCreatures;
  public static string ExcludedCreatures => configExcludedCreatures.Value;
  public static ConfigEntry<string> configExcludedCreatureSpawners;
  public static string ExcludedCreatureSpawners => configExcludedCreatureSpawners.Value;
  public static ConfigEntry<string> configExcludedSpawnSystems;
  public static string ExcludedSpawnZones => configExcludedSpawnSystems.Value;
  public static ConfigEntry<string> configExcludedResources;
  public static string ExcludedResources => configExcludedResources.Value;

  public static void InitExcluded(ConfigFile config) {
    var section = "4. Excluded";
    configExcludedCreatures = config.Bind(section, "Creatures", "", "List of creatures separated by ,");
    configExcludedSpawnSystems = config.Bind(section, "Spawn systems", "Seagal,FireFlies", "List of creatures separated by , that are not visualized");
    configExcludedCreatureSpawners = config.Bind(section, "Spawn points", "", "List of creatures separated by , that are not visualized");
    configExcludedResources = config.Bind(section, "Resources", "Wood,Stone,Rock*,*RockPillar", "List of resources separated by , that are not visualized");
  }
}
