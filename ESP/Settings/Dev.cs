using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;

namespace ESP
{
  public partial class Settings
  {
    public static ConfigEntry<bool> configUseDebugMode;
    public static bool useDegugMode => configUseDebugMode.Value;
    public static ConfigEntry<bool> configUseFreeFly;
    public static bool useFreeFly => configUseFreeFly.Value;
    public static ConfigEntry<bool> configUseGodMode;
    public static bool useGodMode => configUseGodMode.Value;
    public static ConfigEntry<bool> configUseFreeBuild;
    public static bool useFreeBuild => configUseFreeBuild.Value;
    public static ConfigEntry<string> configSetSkills;
    public static string setSkills => configSetSkills.Value;
    public static ConfigEntry<float> configPlayerDamageRange;
    public static float playerDamageRange => configPlayerDamageRange.Value;
    public static ConfigEntry<float> configPlayerDamageBoost;
    public static float playerDamageBoost => configPlayerDamageBoost.Value;
    public static ConfigEntry<float> configPlayerStaminaUsage;
    public static float playerStaminaUsage => configPlayerStaminaUsage.Value;
    public static ConfigEntry<bool> configPlayerForceDodging;
    public static bool playerForceDodging => configPlayerForceDodging.Value;
    public static ConfigEntry<float> configTerrainEditMultiplier;
    public static float terrainEditMultiplier => configTerrainEditMultiplier.Value;
    public static ConfigEntry<float> configCreatureDamageRange;
    public static float creatureDamageRange => configCreatureDamageRange.Value;
    public static ConfigEntry<bool> configFixInvalidLevelData;
    public static bool fixInvalidLevelData => configFixInvalidLevelData.Value;

    public static void InitDev(ConfigFile config)
    {
      var section = "1. Dev";
      Settings.configUseDebugMode = config.Bind(section, "Use debugmode", true, "Enable devcommands and debugmode automatically (single player)");
      Settings.configUseGodMode = config.Bind(section, "Use god mode", true, "Enable god mode automatically (single player)");
      Settings.configUseFreeBuild = config.Bind(section, "Use free build", true, "Enable free build automatically (single player)");
      Settings.configUseFreeFly = config.Bind(section, "Use free fly", true, "Enable free fly automatically (single player)");
      Settings.configFixInvalidLevelData = config.Bind(section, "Fix invalid creature star ranges", true, "Some spawners have higher minimum stars than maximum stars. If true, these are displayed like the code handles them.");
      Settings.configSetSkills = config.Bind(section, "Set skill levels", "", "Sets all skill levels to a given number");
      Settings.configPlayerDamageRange = config.Bind(section, "Player damage range", 0.15f, "Damage variance for players");
      Settings.configCreatureDamageRange = config.Bind(section, "Creature damage range", 0.25f, "Damage variance for creatures");
      Settings.configPlayerDamageBoost = config.Bind(section, "Player damage boost", 0f, "Percentage increase for damage dealt");
      Settings.configPlayerStaminaUsage = config.Bind(section, "Player stamina usage", 1f, "Multiplier to stamina usage");
      Settings.configPlayerForceDodging = config.Bind(section, "Player force dodging", false, "If true, player always dodges");
      Settings.configTerrainEditMultiplier = config.Bind(section, "Terrain changes", 1f, "Multiplier to terrain changes");
    }
  }
}
