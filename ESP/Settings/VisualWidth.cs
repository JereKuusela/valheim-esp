using BepInEx.Configuration;

namespace ESP {
  public partial class Settings {
    public static ConfigEntry<float> configCreatureFireLineWidth;
    public static float CreatureFireLineWidth => configCreatureFireLineWidth.Value;
    public static ConfigEntry<float> configCreatureRayWidth;
    public static float CreatureRayWidth => configCreatureRayWidth.Value;
    public static ConfigEntry<float> configBreedingLineWidth;
    public static float BreedingLineWidth => configBreedingLineWidth.Value;
    public static ConfigEntry<float> configCoverRayWidth;
    public static float CoverRayWidth => configCoverRayWidth.Value;
    public static ConfigEntry<float> configPlayerCoverRayWidth;
    public static float PlayerCoverRayWidth => configPlayerCoverRayWidth.Value;
    public static ConfigEntry<float> configSmokeLineWidth;
    public static float SmokeLineWidth => configSmokeLineWidth.Value;
    public static ConfigEntry<float> configSenseLineWidth;
    public static float SenseLineWidth => configSenseLineWidth.Value;
    public static ConfigEntry<float> configNoiseLineWidth;
    public static float NoiseLineWidth => configNoiseLineWidth.Value;
    public static ConfigEntry<float> configSpawnSystemRayWidth;
    public static float SpawnSystemRayWidth => configSpawnSystemRayWidth.Value;
    public static ConfigEntry<float> configRandEventSystemRayWidth;
    public static float RandEventSystemRayWidth => configRandEventSystemRayWidth.Value;
    public static ConfigEntry<float> configSpawnAreasLineWidth;
    public static float SpawnAreasLineWidth => configSpawnAreasLineWidth.Value;
    public static ConfigEntry<float> configCreatureSpawnerRayWidth;
    public static float CreatureSpawnersRayWidth => configCreatureSpawnerRayWidth.Value;
    public static ConfigEntry<float> configBiomeCornerRayWidth;
    public static float BiomeCornerRayWidth => configBiomeCornerRayWidth.Value;
    public static ConfigEntry<float> configPickableRayWidth;
    public static float PickableRayWidth => configPickableRayWidth.Value;
    public static ConfigEntry<float> configEffectAreaLineWidth;
    public static float EffectAreaLineWidth => configEffectAreaLineWidth.Value;
    public static ConfigEntry<float> configCustomContainerEffectAreaRadius;
    public static float CustomContainerEffectAreaRadius => configCustomContainerEffectAreaRadius.Value;
    public static ConfigEntry<float> configCustomCraftingEffectAreaRadius;
    public static float CustomCraftingEffectAreaRadius => configCustomCraftingEffectAreaRadius.Value;
    public static ConfigEntry<float> configRulerRadius;
    public static float RulerRadius => configRulerRadius.Value;
    public static ConfigEntry<float> configChestRayWidth;
    public static float ChestRayWidth => configChestRayWidth.Value;
    public static ConfigEntry<float> configOreRayWidth;
    public static float OreRayWidth => configOreRayWidth.Value;
    public static ConfigEntry<float> configTreeRayWidth;
    public static float TreeRayWidth => configTreeRayWidth.Value;
    public static ConfigEntry<float> configDestructibleRayWidth;
    public static float DestructibleRayWidth => configDestructibleRayWidth.Value;
    public static ConfigEntry<float> configLocationRayWidth;
    public static float LocationRayWidth => configLocationRayWidth.Value;

    public static void InitVisualWidth(ConfigFile config) {
      var section = "9. Width of visuals (set to 0 for hard disabling features).";
      configCreatureFireLineWidth = config.Bind(section, "Creature fire alert range", 0.02f, "");
      configSenseLineWidth = config.Bind(section, "Creature senses", 0.02f, "");
      configBreedingLineWidth = config.Bind(section, "Breeding limits", 0.02f, "");
      configCoverRayWidth = config.Bind(section, "Cover rays", 0.02f, "");
      configPlayerCoverRayWidth = config.Bind(section, "Player cover rays", 0.02f, "");
      configSmokeLineWidth = config.Bind(section, "Smoke", 0.02f, "");
      configCreatureRayWidth = config.Bind(section, "Creature rays", 0.1f, "");
      configCreatureSpawnerRayWidth = config.Bind(section, "Spawn points", 0.02f, "");
      configSpawnAreasLineWidth = config.Bind(section, "Creature spawners", 0.02f, "");
      configSpawnSystemRayWidth = config.Bind(section, "Spawn zones", 0.1f, "");
      configBiomeCornerRayWidth = config.Bind(section, "Zone corner rays", 0.1f, "");
      configRandEventSystemRayWidth = config.Bind(section, "Random event system", 0.1f, "");
      configPickableRayWidth = config.Bind(section, "Pickable rays", 0.02f, "");
      configRulerRadius = config.Bind(section, "Ruler point radius", 0.1f, "");
      configCustomContainerEffectAreaRadius = config.Bind(section, "Custom radius for containers", 0.0f, "Custom effect area sphere for containers (0 to disable)");
      configCustomCraftingEffectAreaRadius = config.Bind(section, "Custom radius for crafting stations", 0.0f, "Custom effect area sphere for crafting stations (0 to disable)");
      configChestRayWidth = config.Bind(section, "Chest rays", 0.02f, "");
      configOreRayWidth = config.Bind(section, "Ore rays", 0.02f, "");
      configTreeRayWidth = config.Bind(section, "Tree rays", 0.02f, "");
      configDestructibleRayWidth = config.Bind(section, "Destructible rays", 0.02f, "");
      configLocationRayWidth = config.Bind(section, "Location rays", 0.1f, "");
      configEffectAreaLineWidth = config.Bind(section, "Area effects", 0.02f, "");
      configNoiseLineWidth = config.Bind(section, "Noise", 0.02f, "");
    }
  }
}
