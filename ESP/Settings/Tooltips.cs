using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;

namespace ESP
{
  public partial class Settings
  {
    public static ConfigEntry<bool> configDrops;
    public static bool drops => configDrops.Value;
    public static ConfigEntry<bool> configBreeding;
    public static bool breeding => configBreeding.Value;
    public static ConfigEntry<bool> configStatus;
    public static bool status => configStatus.Value;
    public static ConfigEntry<bool> configAttacks;
    public static bool attacks => configAttacks.Value;
    public static ConfigEntry<bool> configResistances;
    public static bool resistances => configResistances.Value;
    public static ConfigEntry<bool> configExtraInfo;
    public static bool extraInfo => configExtraInfo.Value;
    public static ConfigEntry<bool> configAllDamageTypes;
    public static bool allDamageTypes => configAllDamageTypes.Value;
    public static ConfigEntry<bool> configShowProgress;
    public static bool progress => configShowProgress.Value;
    public static ConfigEntry<bool> configSupport;
    public static bool support => configSupport.Value;
    public static ConfigEntry<bool> configStructures;
    public static bool structures => configStructures.Value;
    public static ConfigEntry<bool> configCreatures;
    public static bool creatures => configCreatures.Value;
    public static ConfigEntry<bool> configDestructibles;
    public static bool destructibles => configDestructibles.Value;
    public static ConfigEntry<bool> configPickables;
    public static bool pickables => configPickables.Value;
    public static ConfigEntry<bool> configItemDrops;
    public static bool itemDrops => configItemDrops.Value;
    public static ConfigEntry<bool> configShowShipStats;
    public static bool showShipStats => configShowShipStats.Value;
    public static void InitTooltips(ConfigFile config)
    {
      var section = "3. Tooltips";
      Settings.configExtraInfo = config.Bind(section, "Show extra info on tooltips", false, "Show extra info on tooltips and hover texts (toggle with O button in the game)");
      Settings.configResistances = config.Bind(section, "Resistances", true, "Show resistances for creatures and structures");
      Settings.configAttacks = config.Bind(section, "Attacks", true, "Show creature attacks");
      Settings.configStructures = config.Bind(section, "Structures", true, "Show structure stats (health, resistances, support, etc.)");
      Settings.configCreatures = config.Bind(section, "Creatures", true, "Show creature stats (health, resistances, drops, attacks, taming, etc.)");
      Settings.configDestructibles = config.Bind(section, "Destructibles", true, "Show destructible stats (health, resistances)");
      Settings.configPickables = config.Bind(section, "Pickables", true, "Show pickable stats (respawn, timer)");
      Settings.configItemDrops = config.Bind(section, "Item drops", true, "Show item drop stats (stack size, despawn timer)");
      Settings.configDrops = config.Bind(section, "Drops", true, "Show creature drops");
      Settings.configBreeding = config.Bind(section, "Breeding", true, "Show taming and breeding related information");
      Settings.configStatus = config.Bind(section, "Status effects", true, "Show creature status effects");
      Settings.configAllDamageTypes = config.Bind(section, "All damage types", true, "Show all damage types on weapon tooltips");
      Settings.configShowProgress = config.Bind(section, "Show progress", true, "Show progress for plants and structures");
      Settings.configSupport = config.Bind(section, "Show stats", true, "Show support for structures");
      Settings.configShowShipStats = config.Bind(section, "Show ship stats", true, "Show ship speed and wind direction on the ship");
    }
  }
}
