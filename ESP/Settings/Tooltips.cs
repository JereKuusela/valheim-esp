using BepInEx.Configuration;
namespace ESP;
public partial class Settings
{
#nullable disable
  public static ConfigEntry<string> configCustom;
  public static string Custom => configCustom.Value;
  public static ConfigEntry<bool> configDrops;
  public static bool Drops => configDrops.Value;
  public static ConfigEntry<bool> configBreeding;
  public static bool Breeding => configBreeding.Value;
  public static ConfigEntry<bool> configStatus;
  public static bool Status => configStatus.Value;
  public static ConfigEntry<bool> configAttacks;
  public static bool Attacks => configAttacks.Value;
  public static ConfigEntry<bool> configResistances;
  public static bool Resistances => configResistances.Value;
  public static ConfigEntry<bool> configExtraInfo;
  public static bool ExtraInfo => configExtraInfo.Value;
  public static ConfigEntry<bool> configCustomOnly;
  public static bool CustomOnly => configCustomOnly.Value;
  public static ConfigEntry<bool> configWeaponInfo;
  public static bool WeaponInfo => configWeaponInfo.Value;
  public static ConfigEntry<bool> configShowProgress;
  public static bool Progress => configShowProgress.Value;
  public static ConfigEntry<bool> configSupport;
  public static bool Support => configSupport.Value;
  public static ConfigEntry<bool> configStructures;
  public static bool Structures => configStructures.Value;
  public static ConfigEntry<bool> configCreatures;
  public static bool Creatures => configCreatures.Value;
  public static ConfigEntry<bool> configDestructibles;
  public static bool Destructibles => configDestructibles.Value;
  public static ConfigEntry<bool> configPickables;
  public static bool Pickables => configPickables.Value;
  public static ConfigEntry<bool> configItemDrops;
  public static bool ItemDrops => configItemDrops.Value;
  public static ConfigEntry<bool> configShowShipStats;
  public static bool ShowShipStats => configShowShipStats.Value;
  public static ConfigEntry<bool> configLocations;
  public static bool Locations => configLocations.Value;
  public static ConfigEntry<bool> configVegetation;
  public static bool Vegetation => configVegetation.Value;
  public static void InitTooltips(ConfigFile config)
  {
    var section = "2. Tooltips";
    configExtraInfo = config.Bind(section, "Show extra info on tooltips", false, "Show extra info on tooltips and hover texts (toggle with O button in the game)");
    configResistances = config.Bind(section, "Resistances", true, "Show resistances for creatures and structures");
    configAttacks = config.Bind(section, "Attacks", false, "Show creature attacks");
    configStructures = config.Bind(section, "Structures", true, "Show structure stats (health, resistances, support, etc.)");
    configCreatures = config.Bind(section, "Creatures", true, "Show creature stats (health, resistances, drops, attacks, taming, etc.)");
    configDestructibles = config.Bind(section, "Destructibles", true, "Show destructible stats (health, resistances)");
    configPickables = config.Bind(section, "Pickables", true, "Show pickable stats (respawn, timer)");
    configItemDrops = config.Bind(section, "Item drops", true, "Show item drop stats (stack size, despawn timer)");
    configDrops = config.Bind(section, "Drops", true, "Show creature drops");
    configBreeding = config.Bind(section, "Breeding", true, "Show taming and breeding related information");
    configStatus = config.Bind(section, "Status effects", true, "Show creature status effects");
    configWeaponInfo = config.Bind(section, "Weapon info", true, "Show extra info on weapon tooltips");
    configShowProgress = config.Bind(section, "Show progress", true, "Show progress for plants and structures");
    configSupport = config.Bind(section, "Show stats", true, "Show support for structures");
    configLocations = config.Bind(section, "Locations", true, "Show generator stats for locations");
    configVegetation = config.Bind(section, "Vegetation", false, "Show generator stats for vegetation");
    configShowShipStats = config.Bind(section, "Show ship stats", true, "Show ship speed and wind direction on the ship");
    configCustomOnly = config.Bind(section, "Custom only", false, "Show only custom info when available");
    configCustom = config.Bind(section, "Custom", "", "Custom text format");
  }
}

