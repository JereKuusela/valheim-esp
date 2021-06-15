using BepInEx.Configuration;
using UnityEngine;
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
    public static ConfigEntry<bool> configShowStructureStats;
    public static bool showStructureStats => configShowStructureStats.Value;
    public static ConfigEntry<bool> configShowSupport;
    public static bool showSupport => configShowSupport.Value;


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

    public static void Init(ConfigFile config)
    {
      Settings.configShowCreatureFireLimits = config.Bind("Creatures", "Show creature fire limits", true, "Vsualize radius of fire fearing");
      Settings.configShowBaseAI = config.Bind("Creatures", "Show creature senses", true, "Visualize creature sight and hear ranges");
      Settings.configShowCreatureStats = config.Bind("Creatures", "Show creature stats", true, "Show creature health, stagger, mass and resistances");
      Settings.configShowDropStats = config.Bind("Creatures", "Show drop stats", true, "Show loot stats");
      Settings.configShowStatusEffects = config.Bind("Creatures", "Show status effects", true, "Show status effects");
      Settings.configShowBreedingStats = config.Bind("Creatures", "Show breeding stats", true, "Show taming and breeding related stats");
      Settings.configShowBreedingLimits = config.Bind("Creatures", "Show breeding limits", true, "Visualize breeding parther check and total limit ranges");
      Settings.configShowCreatureRays = config.Bind("Creatures", "Show creature rays", true, "Visualize creature locations");
      Settings.configCharacterRayWidth = config.Bind("Creatures", "Width of the character rays", 0.5f, "");
      Settings.configExcludedCreatures = config.Bind("Creatures", "Exclude creatures", "", "List of creatures separated by ,");

      Settings.configShowCreatureSpawners = config.Bind("Spawners", "Show spawn points", true, "Visualize fixed creature spawn points");
      Settings.configExcludedCreatureSpawners = config.Bind("Spawners", "Excluded spawn points", "", "List of creatures separated by , that are not visualized");
      Settings.configShowSpawnAreas = config.Bind("Spawners", "Show creature spawners", true, "Visualize physical creature spawners");
      Settings.configExcludedSpawnSystems = config.Bind("Spawners", "Excluded spawn systems", "Seagal,FireFlies", "List of creatures separated by , that are not visualized");
      Settings.configShowSpawnSystems = config.Bind("Spawners", "Show spawn zones", true, "Visualize spawn zone system");
      Settings.configShowBiomes = config.Bind("Spawners", "Show zone corner rays", true, "Visualize zone corners and their biomes");
      Settings.configShowRandEventSystem = config.Bind("Spawners", "Show random event system", true, "Visualize random event system");

      Settings.configShowPickables = config.Bind("Pickables", "ShowPickables", true, "Enable for pickables");
      Settings.configExcludedPickables = config.Bind("Pickables", "ExcludedPickables", "Wood,Stone", "List of items separated by , that are not visualized");
      Settings.configPickableRayWidth = config.Bind("Pickables", "Width of the pickable rays", 0.5f, "");

      Settings.configShowChests = config.Bind("Locations", "Show chest rays", true, "Visualize hidden chests");
      Settings.configChestRayWidth = config.Bind("Locations", "Width of the chest rays", 0.5f, "");
      Settings.configShowLocations = config.Bind("Locations", "Show location rays", true, "Visualize pre-made structures and other locations");
      Settings.configLocationRayWidth = config.Bind("Locations", "Width of the location rays", 0.5f, "");

      Settings.configUseDebugMode = config.Bind("Dev", "Use debugmode", true, "Enable devcommands and debugmode automatically");
      Settings.configUseGodMode = config.Bind("Dev", "Use god mode", true, "Enable god mode automatically");
      Settings.configUseFreeBuild = config.Bind("Dev", "Use free build", true, "Enable free build automatically");
      Settings.configUseFreeFly = config.Bind("Dev", "Use free fly", true, "Enable free fly automatically");
      Settings.configShowVisualization = config.Bind("Dev", "Show visualization", false, "Show visualization (toggle with O button in the game)");
      Settings.configShowDPS = config.Bind("Dev", "Show DPS meter", false, "Show DPS meter (toggle with P button in the game)");
      Settings.configShowExtraInfo = config.Bind("Dev", "Show extra info", false, "Show extra info on tooltips and hover texts (toggle with I button in the game)");

      Settings.configShowAllDamageTypes = config.Bind("Combat", "Show all damage types", true, "Show all damage types on weapon tooltips");
      Settings.configSetSkills = config.Bind("Combat", "Set skill levels", "", "Sets all skill levels to a given number");
      Settings.configPlayerDamageRange = config.Bind("Combat", "Player damage range", 0.15f, "Damage variance for players");
      Settings.configCreatureDamageRange = config.Bind("Combat", "Creature damage range", 0.25f, "Damage variance for creatures");

      Settings.configShowTimeAndWeather = config.Bind("HUD", "Show current time and weather", true, "Show current time and weather on the hud");
      Settings.configShowShipStatsOnHud = config.Bind("HUD", "Show ship stats", true, "Show ship stats on the hud");

      Settings.configShowProgress = config.Bind("Structures", "Show progress", true, "Show progress for plants and structures");
      Settings.configShowStructureStats = config.Bind("Structures", "Show stats", true, "Show health, resistances and support for structures");
      Settings.configShowSupport = config.Bind("Structures", "Show support", true, "Always show support color for structures");

      Settings.configShowEffectAreas = config.Bind("General", "Show area effects", true, "Visualize structure area effects");
      Settings.configshowNoise = config.Bind("General", "Show noise", false, "Visualize noise");
      Settings.configShowShipStats = config.Bind("General", "Show ship stats", false, "Show ship speed and wind direction on the ship");
      Settings.configFixInvalidLevelData = config.Bind("General", "Fix invalid creature star ranges", true, "Some spawners have higher minimum stars than maximum stars. If true, these are displayed like the code handles them.");

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
          Drawer.ToggleVisibility();
          SupportUtils.ToggleVisibility();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
          Settings.configShowExtraInfo.Value = !Settings.configShowExtraInfo.Value;
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
