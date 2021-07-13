using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;

namespace ESP
{
  public class Settings
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

    public static ConfigEntry<float> configCreatureFireLineWidth;
    public static float creatureFireLineWidth => configCreatureFireLineWidth.Value;
    public static ConfigEntry<float> configCreatureRayWidth;
    public static float creatureRayWidth => configCreatureRayWidth.Value;
    public static ConfigEntry<float> configBreedingLineWidth;
    public static float breedingLineWidth => configBreedingLineWidth.Value;
    public static ConfigEntry<float> configCoverRayWidth;
    public static float coverRayWidth => configCoverRayWidth.Value;
    public static ConfigEntry<float> configSmokeLineWidth;
    public static float smokeLineWidth => configSmokeLineWidth.Value;
    public static ConfigEntry<float> configSenseLineWidth;
    public static float senseLineWidth => configSenseLineWidth.Value;
    public static ConfigEntry<float> configNoiseLineWidth;
    public static float noiseLineWidth => configNoiseLineWidth.Value;
    public static ConfigEntry<float> configSpawnSystemRayWidth;
    public static float spawnSystemRayWidth => configSpawnSystemRayWidth.Value;
    public static ConfigEntry<float> configRandEventSystemRayWidth;
    public static float randEventSystemRayWidth => configRandEventSystemRayWidth.Value;
    public static ConfigEntry<float> configSpawnAreasLineWidth;
    public static float spawnAreasLineWidth => configSpawnAreasLineWidth.Value;
    public static ConfigEntry<float> configCreatureSpawnersRayWidth;
    public static float creatureSpawnersRayWidth => configCreatureSpawnersRayWidth.Value;
    public static ConfigEntry<float> configBiomeCornerRayWidth;
    public static float biomeCornerRayWidth => configBiomeCornerRayWidth.Value;
    public static ConfigEntry<float> configPickableRayWidth;
    public static float pickableRayWidth => configPickableRayWidth.Value;
    public static ConfigEntry<float> configEffectAreaLineWidth;
    public static float effectAreaLineWidth => configEffectAreaLineWidth.Value;
    public static ConfigEntry<float> configChestRayWidth;
    public static float chestRayWidth => configChestRayWidth.Value;
    public static ConfigEntry<float> configOreRayWidth;
    public static float oreRayWidth => configOreRayWidth.Value;
    public static ConfigEntry<float> configTreeRayWidth;
    public static float treeRayWidth => configTreeRayWidth.Value;
    public static ConfigEntry<float> configDestructibleRayWidth;
    public static float destructibleRayWidth => configDestructibleRayWidth.Value;
    public static ConfigEntry<float> configLocationRayWidth;
    public static float locationRayWidth => configLocationRayWidth.Value;
    public static ConfigEntry<bool> configShowSupport;
    public static bool showSupport => configShowSupport.Value;

    public static ConfigEntry<string> configExcludedAreaEffects;
    public static string excludedAreaEffects => configExcludedAreaEffects.Value;
    public static ConfigEntry<string> configExcludedCreatures;
    public static string excludedCreatures => configExcludedCreatures.Value;
    public static ConfigEntry<string> configTrackedCreatures;
    public static string trackedCreatures => configTrackedCreatures.Value;
    public static ConfigEntry<string> configExcludedCreatureSpawners;
    public static string excludedCreatureSpawners => configExcludedCreatureSpawners.Value;
    public static ConfigEntry<string> configExcludedSpawnSystems;
    public static string excludedSpawnSystems => configExcludedSpawnSystems.Value;
    public static ConfigEntry<string> configExcludedPickables;
    public static string excludedPickables => configExcludedPickables.Value;
    public static ConfigEntry<bool> configShowOthers;
    public static bool showOthers => configShowOthers.Value;
    public static ConfigEntry<bool> configShowZones;
    public static bool showZones => configShowZones.Value;
    public static ConfigEntry<bool> configShowCreatures;
    public static bool showCreatures => configShowCreatures.Value;




    public static ConfigEntry<bool> configUseDebugMode;
    public static bool useDegugMode => configUseDebugMode.Value;
    public static ConfigEntry<bool> configUseFreeFly;
    public static bool useFreeFly => configUseFreeFly.Value;
    public static ConfigEntry<bool> configUseGodMode;
    public static bool useGodMode => configUseGodMode.Value;
    public static ConfigEntry<bool> configUseFreeBuild;
    public static bool useFreeBuild => configUseFreeBuild.Value;
    public static ConfigEntry<bool> configShowDPS;
    public static bool showDPS => configShowDPS.Value;

    public static ConfigEntry<string> configSetSkills;
    public static string setSkills => configSetSkills.Value;
    public static ConfigEntry<float> configPlayerDamageRange;
    public static float playerDamageRange => configPlayerDamageRange.Value;
    public static ConfigEntry<float> configCreatureDamageRange;
    public static float creatureDamageRange => configCreatureDamageRange.Value;

    public static ConfigEntry<bool> configShowShipStatsOnHud;
    public static bool showShipStatsOnHud => configShowShipStatsOnHud.Value;
    public static ConfigEntry<bool> configShowHud;
    public static bool showHud => configShowHud.Value;
    public static ConfigEntry<bool> configShowTimeAndWeather;
    public static bool showTimeAndWeather => configShowTimeAndWeather.Value;


    public static ConfigEntry<bool> configShowShipStats;
    public static bool showShipStats => configShowShipStats.Value;
    public static ConfigEntry<bool> configFixInvalidLevelData;
    public static bool fixInvalidLevelData => configFixInvalidLevelData.Value;

    public static void Init(ConfigFile config)
    {
      Settings.configExtraInfo = config.Bind("Tooltips", "Show extra info on tooltips", false, "Show extra info on tooltips and hover texts (toggle with O button in the game)");
      Settings.configResistances = config.Bind("Tooltips", "Resistances", true, "Show resistances for creatures and structures");
      Settings.configAttacks = config.Bind("Tooltips", "Attacks", true, "Show creature attacks");
      Settings.configStructures = config.Bind("Tooltips", "Structures", true, "Show structure stats (health, resistances, support, etc.)");
      Settings.configCreatures = config.Bind("Tooltips", "Creatures", true, "Show creature stats (health, resistances, drops, attacks, taming, etc.)");
      Settings.configDestructibles = config.Bind("Tooltips", "Destructibles", true, "Show destructible stats (health, resistances)");
      Settings.configPickables = config.Bind("Tooltips", "Pickables", true, "Show pickable stats (respawn, timer)");
      Settings.configItemDrops = config.Bind("Tooltips", "Item drops", true, "Show item drop stats (stack size, despawn timer)");
      Settings.configDrops = config.Bind("Tooltips", "Drops", true, "Show creature drops");
      Settings.configBreeding = config.Bind("Tooltips", "Breeding", true, "Show taming and breeding related information");
      Settings.configStatus = config.Bind("Tooltips", "Status effects", true, "Show creature status effects");
      Settings.configAllDamageTypes = config.Bind("Tooltips", "All damage types", true, "Show all damage types on weapon tooltips");
      Settings.configShowProgress = config.Bind("Visual", "Show progress", true, "Show progress for plants and structures");
      Settings.configSupport = config.Bind("Visual", "Show stats", true, "Show support for structures");
      Settings.configShowShipStats = config.Bind("Visual", "Show ship stats", true, "Show ship speed and wind direction on the ship");

      Settings.configShowZones = config.Bind("Visual", "Show zones", false, "Show visualization for zones (toggle with Y button in the game)");
      Settings.configShowCreatures = config.Bind("Visual", "Show creatures", false, "Show visualization for creatures (toggle with U button in the game)");
      Settings.configShowOthers = config.Bind("Visual", "Show visualization", false, "Show visualization for everything else (toggle with I button in the game)");
      Settings.configCreatureFireLineWidth = config.Bind("Visual", "Creature fire alert range", 0.1f, "Vsualize radius of fire fearing");
      Settings.configSenseLineWidth = config.Bind("Visual", "Creature senses", 0.1f, "Line width of sight and hear ranges (0 to disable)");
      Settings.configBreedingLineWidth = config.Bind("Visual", "Breeding limits", 0.1f, "Visualize breeding parther check and total limit ranges");
      Settings.configCoverRayWidth = config.Bind("Visual", "Cover rays", 0.1f, "Visualize cover check rays");
      Settings.configSmokeLineWidth = config.Bind("Visual", "Smoke", 0.1f, "Visualize smoke particles");
      Settings.configCreatureRayWidth = config.Bind("Visual", "Creature rays", 0.5f, "Line width for tracked creature locations (0 to disable)");
      Settings.configCreatureSpawnersRayWidth = config.Bind("Visual", "Spawn points", 0.1f, "Line width of fixed creature spawn points (0 to disable)");
      Settings.configSpawnAreasLineWidth = config.Bind("Visual", "Creature spawners", 0.1f, "Line width of physical creature spawner ranges (0 to disable)");
      Settings.configSpawnSystemRayWidth = config.Bind("Visual", "Spawn zones", 1f, "Line width of spawn zone system (0 to disable)");
      Settings.configBiomeCornerRayWidth = config.Bind("Visual", "Zone corner rays", 0.25f, "Line width of zone corners (0 to disable)");
      Settings.configRandEventSystemRayWidth = config.Bind("Visual", "Random event system", 1f, "Line width of random event system (0 to disable)");
      Settings.configPickableRayWidth = config.Bind("Visual", "Pickable rays", 0.5f, "Line width of pickable locations (0 to disable)");
      Settings.configChestRayWidth = config.Bind("Visual", "Chest rays", 0.5f, "Line width of hidden chest locations (0 to disable)");
      Settings.configOreRayWidth = config.Bind("Visual", "Ore rays", 0f, "Line width of ore locations (0 to disable)");
      Settings.configTreeRayWidth = config.Bind("Visual", "Tree rays", 0f, "Line width of tree locations (0 to disable)");
      Settings.configDestructibleRayWidth = config.Bind("Visual", "Destructible rays", 0f, "Line width of destructible locations (0 to disable)");
      Settings.configLocationRayWidth = config.Bind("Visual", "Location rays", 0.5f, "Line width of pre-generated structure locations (0 to disable)");
      Settings.configShowSupport = config.Bind("Visual", "Support", true, "Always show support color for structures");
      Settings.configEffectAreaLineWidth = config.Bind("Visual", "Area effects", 0.1f, "Line width of area effect ranges(0 to disable)");
      Settings.configNoiseLineWidth = config.Bind("Visual", "Noise", 0.0f, "Line width of noise range (0 to disable)");

      Settings.configTrackedCreatures = config.Bind("Highlight", "Creatures", "Serpent", "List of creatures to track (separated by ,)");
      Settings.configExcludedAreaEffects = config.Bind("Exclusions", "Area effects", "", "List of area effects separated by ,");
      Settings.configExcludedCreatures = config.Bind("Exclusions", "Creatures", "", "List of creatures separated by ,");
      Settings.configExcludedSpawnSystems = config.Bind("Exclusions", "Spawn systems", "Seagal,FireFlies", "List of creatures separated by , that are not visualized");
      Settings.configExcludedCreatureSpawners = config.Bind("Exclusions", "Spawn points", "", "List of creatures separated by , that are not visualized");
      Settings.configExcludedPickables = config.Bind("Exclusions", "Pickables", "Wood,Stone", "List of items separated by , that are not visualized");

      Settings.configUseDebugMode = config.Bind("Dev", "Use debugmode", true, "Enable devcommands and debugmode automatically (single player)");
      Settings.configUseGodMode = config.Bind("Dev", "Use god mode", true, "Enable god mode automatically (single player)");
      Settings.configUseFreeBuild = config.Bind("Dev", "Use free build", true, "Enable free build automatically (single player)");
      Settings.configUseFreeFly = config.Bind("Dev", "Use free fly", true, "Enable free fly automatically (single player)");
      Settings.configFixInvalidLevelData = config.Bind("Dev", "Fix invalid creature star ranges", true, "Some spawners have higher minimum stars than maximum stars. If true, these are displayed like the code handles them.");

      Settings.configShowDPS = config.Bind("DPS", "Show DPS meter", false, "Show DPS meter (toggle with P button in the game)");
      Settings.configSetSkills = config.Bind("DPS", "Set skill levels", "", "Sets all skill levels to a given number");
      Settings.configPlayerDamageRange = config.Bind("DPS", "Player damage range", 0.15f, "Damage variance for players");
      Settings.configCreatureDamageRange = config.Bind("DPS", "Creature damage range", 0.25f, "Damage variance for creatures");

      Settings.configShowHud = config.Bind("HUD", "Show HUD", true, "Show info and stats on HUD");
      Settings.configShowTimeAndWeather = config.Bind("HUD", "Show current time and weather", true, "Show current time and weather on the hud");
      Settings.configShowShipStatsOnHud = config.Bind("HUD", "Show ship stats", true, "Show ship stats on the hud");


    }
  }

  [HarmonyPatch(typeof(Player), "Update")]
  public class Player_Update
  {
    public static void Postfix(Player __instance)
    {
      if (Patch.Player_TakeInput(__instance))
      {
        if (Input.GetKeyDown(KeyCode.Y))
        {
          Drawer.ToggleZoneVisibility();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
          Drawer.ToggleCreatureVisibility();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
          Drawer.ToggleOtherVisibility();
          SupportUtils.SetVisibility(Drawer.showOthers);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
          Hoverables.extraInfo = !Hoverables.extraInfo;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
          Settings.configShowDPS.Value = !Settings.configShowDPS.Value;
          if (!Settings.showDPS) DPSMeter.Reset();
        }
      }
    }
  }
}
