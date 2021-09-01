using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;

namespace ESP
{
  public partial class Settings
  {
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
    public static ConfigEntry<float> configCreatureSpawnerRayWidth;
    public static float creatureSpawnersRayWidth => configCreatureSpawnerRayWidth.Value;
    public static ConfigEntry<float> configBiomeCornerRayWidth;
    public static float biomeCornerRayWidth => configBiomeCornerRayWidth.Value;
    public static ConfigEntry<float> configPickableRayWidth;
    public static float pickableRayWidth => configPickableRayWidth.Value;
    public static ConfigEntry<float> configEffectAreaLineWidth;
    public static float effectAreaLineWidth => configEffectAreaLineWidth.Value;
    public static ConfigEntry<float> configCustomContainerEffectAreaRadius;
    public static float customContainerEffectAreaRadius => configCustomContainerEffectAreaRadius.Value;
    public static ConfigEntry<float> configCustomCraftingEffectAreaRadius;
    public static float customCraftingEffectAreaRadius => configCustomCraftingEffectAreaRadius.Value;
    public static ConfigEntry<float> configMineRockSupportLineWidth;
    public static float mineRockSupportLineWidth => configMineRockSupportLineWidth.Value;
    public static ConfigEntry<float> configRulerRadius;
    public static float rulerRadius => configRulerRadius.Value;
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
    public static void InitVisuals(ConfigFile config)
    {
      var section = "4. Visuals";
      Settings.configShowZones = config.Bind(section, "Show zones", false, "Show visualization for zones (toggle with Y button in the game)");
      Settings.configShowCreatures = config.Bind(section, "Show creatures", false, "Show visualization for creatures (toggle with U button in the game)");
      Settings.configShowOthers = config.Bind(section, "Show visualization", false, "Show visualization for everything else (toggle with I button in the game)");
      Settings.configCreatureFireLineWidth = config.Bind(section, "Creature fire alert range", 0.1f, "Vsualize radius of fire fearing");
      Settings.configSenseLineWidth = config.Bind(section, "Creature senses", 0.1f, "Line width of sight and hear ranges (0 to disable)");
      Settings.configBreedingLineWidth = config.Bind(section, "Breeding limits", 0.1f, "Visualize breeding parther check and total limit ranges");
      Settings.configCoverRayWidth = config.Bind(section, "Cover rays", 0.1f, "Visualize cover check rays");
      Settings.configSmokeLineWidth = config.Bind(section, "Smoke", 0.1f, "Visualize smoke particles");
      Settings.configCreatureRayWidth = config.Bind(section, "Creature rays", 0.25f, "Line width for tracked creature locations (0 to disable)");
      Settings.configCreatureSpawnerRayWidth = config.Bind(section, "Spawn points", 0.1f, "Line width of fixed creature spawn points (0 to disable)");
      Settings.configSpawnAreasLineWidth = config.Bind(section, "Creature spawners", 0.1f, "Line width of physical creature spawner ranges (0 to disable)");
      Settings.configSpawnSystemRayWidth = config.Bind(section, "Spawn zones", 0.5f, "Line width of spawn zone system (0 to disable)");
      Settings.configBiomeCornerRayWidth = config.Bind(section, "Zone corner rays", 0.25f, "Line width of zone corners (0 to disable)");
      Settings.configRandEventSystemRayWidth = config.Bind(section, "Random event system", 0.5f, "Line width of random event system (0 to disable)");
      Settings.configPickableRayWidth = config.Bind(section, "Pickable rays", 0.1f, "Line width of pickable locations (0 to disable)");
      Settings.configRulerRadius = config.Bind(section, "Ruler point radius", 0.5f, "Ruler point radius (0 to disable)");
      Settings.configChestRayWidth = config.Bind(section, "Chest rays", 0.1f, "Line width of hidden chest locations (0 to disable)");
      Settings.configOreRayWidth = config.Bind(section, "Ore rays", 0.1f, "Line width of ore locations (0 to disable)");
      Settings.configTreeRayWidth = config.Bind(section, "Tree rays", 0f, "Line width of tree locations (0 to disable)");
      Settings.configDestructibleRayWidth = config.Bind(section, "Destructible rays", 0f, "Line width of destructible locations (0 to disable)");
      Settings.configLocationRayWidth = config.Bind(section, "Location rays", 0.5f, "Line width of pre-generated structure locations (0 to disable)");
      Settings.configShowSupport = config.Bind(section, "Support", true, "Always show support color for structures");
      Settings.configEffectAreaLineWidth = config.Bind(section, "Area effects", 0.1f, "Line width of area effect ranges (0 to disable)");
      Settings.configCustomContainerEffectAreaRadius = config.Bind(section, "Custom radius for containers", 0.0f, "Custom effect area sphere for containers (0 to disable)");
      Settings.configCustomCraftingEffectAreaRadius = config.Bind(section, "Custom radius for crafting stations", 0.0f, "Custom effect area sphere for crafting stations (0 to disable)");
      Settings.configNoiseLineWidth = config.Bind(section, "Noise", 0.0f, "Line width of noise range (0 to disable)");
      Settings.configMineRockSupportLineWidth = config.Bind(section, "Mine rock support", 0.0f, "Line width of mine rock support bounding boxes (0 to disable)");
    }
  }
}
