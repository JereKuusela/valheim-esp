﻿using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace ESP
{
  public class Settings
  {
    public static ConfigEntry<bool> configShowCreatureFireLimits;
    public static bool showCreatureFireLimits => configShowCreatureFireLimits.Value;
    public static ConfigEntry<bool> configShowCreatureRays;
    public static bool showCreatureRays => configShowCreatureRays.Value;
    public static ConfigEntry<string> configExcludedCreatures;
    public static string excludedCreatures => configExcludedCreatures.Value;
    public static ConfigEntry<float> configCharacterRayWidth;
    public static float characterRayWidth => configCharacterRayWidth.Value;
    public static ConfigEntry<bool> configShowCreatureStats;
    public static bool showCreatureStats => configShowCreatureStats.Value;
    public static ConfigEntry<bool> configShowDropStats;
    public static bool showDropStats => configShowDropStats.Value;
    public static ConfigEntry<bool> configShowBreedingStats;
    public static bool showBreedingStats => configShowBreedingStats.Value;
    public static ConfigEntry<bool> configShowStatusEffects;
    public static bool showStatusEffects => configShowStatusEffects.Value;
    public static ConfigEntry<bool> configShowBreedingLimits;
    public static bool showBreedingLimits => configShowBreedingLimits.Value;
    public static ConfigEntry<bool> configShowBaseAI;
    public static bool showBaseAI => configShowBaseAI.Value;

    public static ConfigEntry<bool> configShowSpawnSystems;
    public static bool showSpawnSystems => configShowSpawnSystems.Value;
    public static ConfigEntry<bool> configShowRandEventSystem;
    public static bool showRandEventSystem => configShowRandEventSystem.Value;
    public static ConfigEntry<bool> configShowSpawnAreas;
    public static bool showSpawnAreas => configShowSpawnAreas.Value;
    public static ConfigEntry<bool> configShowCreatureSpawners;
    public static bool showCreatureSpawners => configShowCreatureSpawners.Value;
    public static ConfigEntry<string> configExcludedCreatureSpawners;
    public static string excludedCreatureSpawners => configExcludedCreatureSpawners.Value;
    public static ConfigEntry<string> configExcludedSpawnSystems;
    public static string excludedSpawnSystems => configExcludedSpawnSystems.Value;
    public static ConfigEntry<bool> configShowBiomes;
    public static bool showBiomes => configShowBiomes.Value;

    public static ConfigEntry<bool> configShowPickables;
    public static bool showPickables => configShowPickables.Value;
    public static ConfigEntry<string> configExcludedPickables;
    public static string excludedPickables => configExcludedPickables.Value;
    public static ConfigEntry<float> configPickableRayWidth;
    public static float pickableRayWidth => configPickableRayWidth.Value;

    public static ConfigEntry<bool> configShowEffectAreas;
    public static bool showEffectAreas => configShowEffectAreas.Value;

    public static ConfigEntry<bool> configShowChests;
    public static bool showChests => configShowChests.Value;
    public static ConfigEntry<float> configChestRayWidth;
    public static float chestRayWidth => configChestRayWidth.Value;
    public static ConfigEntry<bool> configShowLocations;
    public static bool showLocations => configShowLocations.Value;
    public static ConfigEntry<float> configLocationRayWidth;
    public static float locationRayWidth => configLocationRayWidth.Value;


    public static ConfigEntry<bool> configUseDebugMode;
    public static bool useDegugMode => configUseDebugMode.Value;
    public static ConfigEntry<bool> configUseFreeFly;
    public static bool useFreeFly => configUseFreeFly.Value;
    public static ConfigEntry<bool> configUseGodMode;
    public static bool useGodMode => configUseGodMode.Value;
    public static ConfigEntry<bool> configUseFreeBuild;
    public static bool useFreeBuild => configUseFreeBuild.Value;
    public static ConfigEntry<bool> configShowVisualization;
    public static bool showVisualization => configShowVisualization.Value;
    public static ConfigEntry<bool> configShowDPS;
    public static bool showDPS => configShowDPS.Value;
    public static ConfigEntry<bool> configShowExtraInfo;
    public static bool showExtraInfo => configShowExtraInfo.Value;


    public static ConfigEntry<bool> configShowAllDamageTypes;
    public static bool showAllDamageTypes => configShowAllDamageTypes.Value;
    public static ConfigEntry<string> configSetSkills;
    public static string setSkills => configSetSkills.Value;
    public static ConfigEntry<float> configPlayerDamageRange;
    public static float playerDamageRange => configPlayerDamageRange.Value;
    public static ConfigEntry<float> configCreatureDamageRange;
    public static float creatureDamageRange => configCreatureDamageRange.Value;

    public static ConfigEntry<bool> configShowProgress;
    public static bool showProgress => configShowProgress.Value;
    public static ConfigEntry<bool> configShowStructureHealth;
    public static bool showStructureHealth => configShowStructureHealth.Value;

    public static ConfigEntry<bool> configShowShipStatsOnHud;
    public static bool showShipStatsOnHud => configShowShipStatsOnHud.Value;
    public static ConfigEntry<bool> configShowTimeAndWeather;
    public static bool showTimeAndWeather => configShowTimeAndWeather.Value;


    public static ConfigEntry<bool> configshowNoise;
    public static bool showNoise => configshowNoise.Value;
    public static ConfigEntry<bool> configShowShipStats;
    public static bool showShipStats => configShowShipStats.Value;
    public static ConfigEntry<bool> configFixInvalidLevelData;
    public static bool fixInvalidLevelData => configFixInvalidLevelData.Value;
  }


  [BepInPlugin("valheim.jerekuusela.esp", "ESP", "0.1.0.0")]
  public class ESP : BaseUnityPlugin
  {
    void Awake()
    {
      Settings.configShowCreatureFireLimits = Config.Bind("Creatures", "Show creature fire limits", true, "Vsualize radius of fire fearing");
      Settings.configShowBaseAI = Config.Bind("Creatures", "Show creature senses", true, "Visualize creature sight and hear ranges");
      Settings.configShowCreatureStats = Config.Bind("Creatures", "Show creature stats", true, "Show creature health, stagger, mass and resistances");
      Settings.configShowDropStats = Config.Bind("Creatures", "Show drop stats", true, "Show loot stats");
      Settings.configShowStatusEffects = Config.Bind("Creatures", "Show status effects", true, "Show status effects");
      Settings.configShowBreedingStats = Config.Bind("Creatures", "Show breeding stats", true, "Show taming and breeding related stats");
      Settings.configShowBreedingLimits = Config.Bind("Creatures", "Show breeding limits", true, "Visualize breeding parther check and total limit ranges");
      Settings.configShowCreatureRays = Config.Bind("Creatures", "Show creature rays", true, "Visualize creature locations");
      Settings.configCharacterRayWidth = Config.Bind("Creatures", "Width of the character rays", 0.5f, "");
      Settings.configExcludedCreatures = Config.Bind("Creatures", "Exclude creatures", "", "List of creatures separated by ,");

      Settings.configShowCreatureSpawners = Config.Bind("Spawners", "Show spawn points", true, "Visualize fixed creature spawn points");
      Settings.configExcludedCreatureSpawners = Config.Bind("Spawners", "Excluded spawn points", "", "List of creatures separated by , that are not visualized");
      Settings.configShowSpawnAreas = Config.Bind("Spawners", "Show creature spawners", true, "Visualize physical creature spawners");
      Settings.configExcludedSpawnSystems = Config.Bind("Spawners", "Excluded spawn systems", "Seagal,FireFlies", "List of creatures separated by , that are not visualized");
      Settings.configShowSpawnSystems = Config.Bind("Spawners", "Show spawn zones", true, "Visualize spawn zone system");
      Settings.configShowBiomes = Config.Bind("Spawners", "Show zone corner rays", true, "Visualize zone corners and their biomes");
      Settings.configShowRandEventSystem = Config.Bind("Spawners", "Show random event system", true, "Visualize random event system");

      Settings.configShowPickables = Config.Bind("Pickables", "ShowPickables", true, "Enable for pickables");
      Settings.configExcludedPickables = Config.Bind("Pickables", "ExcludedPickables", "Wood,Stone", "List of items separated by , that are not visualized");
      Settings.configPickableRayWidth = Config.Bind("Pickables", "Width of the pickable rays", 0.5f, "");

      Settings.configShowChests = Config.Bind("Locations", "Show chest rays", true, "Visualize hidden chests");
      Settings.configChestRayWidth = Config.Bind("Locations", "Width of the chest rays", 0.5f, "");
      Settings.configShowLocations = Config.Bind("Locations", "Show location rays", true, "Visualize pre-made structures and other locations");
      Settings.configLocationRayWidth = Config.Bind("Locations", "Width of the location rays", 0.5f, "");

      Settings.configUseDebugMode = Config.Bind("Dev", "Use debugmode", true, "Enable devcommands and debugmode automatically");
      Settings.configUseGodMode = Config.Bind("Dev", "Use god mode", true, "Enable god mode automatically");
      Settings.configUseFreeBuild = Config.Bind("Dev", "Use free build", true, "Enable free build automatically");
      Settings.configUseFreeFly = Config.Bind("Dev", "Use free fly", true, "Enable free fly automatically");
      Settings.configShowVisualization = Config.Bind("Dev", "Show visualization", false, "Show visualization (toggle with O button in the game)");
      Settings.configShowDPS = Config.Bind("Dev", "Show DPS meter", false, "Show DPS meter (toggle with P button in the game)");
      Settings.configShowExtraInfo = Config.Bind("Dev", "Show extra info", false, "Show extra info on tooltips and hover texts (toggle with I button in the game)");

      Settings.configShowAllDamageTypes = Config.Bind("Combat", "Show all damage types", true, "Show all damage types on weapon tooltips");
      Settings.configSetSkills = Config.Bind("Combat", "Set skill levels", "", "Sets all skill levels to a given number");
      Settings.configPlayerDamageRange = Config.Bind("Combat", "Player damage range", 0.15f, "Damage variance for players");
      Settings.configCreatureDamageRange = Config.Bind("Combat", "Creature damage range", 0.25f, "Damage variance for creatures");

      Settings.configShowTimeAndWeather = Config.Bind("HUD", "Show current time and weather", true, "Show current time and weather on the hud");
      Settings.configShowShipStatsOnHud = Config.Bind("HUD", "Show ship stats", true, "Show ship stats on the hud");

      Settings.configShowProgress = Config.Bind("Structures", "Show progress", true, "Show progress for plants and structures");
      Settings.configShowStructureHealth = Config.Bind("Structures", "Show health and resistances", true, "Show health and resistances for structures");

      Settings.configShowEffectAreas = Config.Bind("General", "Show area effects", true, "Visualize structure area effects");
      Settings.configshowNoise = Config.Bind("General", "Show noise", false, "Visualize noise");
      Settings.configShowShipStats = Config.Bind("General", "Show ship stats", false, "Show ship speed and wind direction on the ship");
      Settings.configFixInvalidLevelData = Config.Bind("General", "Fix invalid creature star ranges", true, "Some spawners have higher minimum stars than maximum stars. If true, these are displayed like the code handles them.");
      var harmony = new Harmony("valheim.jerekuusela.esp");
      harmony.PatchAll();
    }
  }
}
