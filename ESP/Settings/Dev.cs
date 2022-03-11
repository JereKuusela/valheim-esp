using BepInEx.Configuration;

namespace ESP {
  public partial class Settings {
    public static ConfigEntry<bool> configIgnoreForsakedPowerCooldown;
    public static bool IgnoreForsakedPowerCooldown => configIgnoreForsakedPowerCooldown.Value;
    public static ConfigEntry<float> configPlayerDamageBoost;
    public static float PlayerDamageBoost => configPlayerDamageBoost.Value;
    public static ConfigEntry<float> configPlayerStaminaUsage;
    public static float PlayerStaminaUsage => configPlayerStaminaUsage.Value;
    public static ConfigEntry<bool> configPlayerForceDodging;
    public static bool PlayerForceDodging => configPlayerForceDodging.Value;
    public static ConfigEntry<float> configTerrainEditMultiplier;
    public static float TerrainEditMultiplier => configTerrainEditMultiplier.Value;

    public static void InitDev(ConfigFile config) {
      var section = "1. Dev";
      configPlayerDamageBoost = config.Bind(section, "Player damage boost", 0f, "Percentage increase for damage dealt");
      configPlayerStaminaUsage = config.Bind(section, "Player stamina usage", 1f, "Multiplier to stamina usage");
      configPlayerForceDodging = config.Bind(section, "Player force dodging", false, "If true, player always dodges");
      configTerrainEditMultiplier = config.Bind(section, "Terrain changes", 1f, "Multiplier to terrain changes");
      configIgnoreForsakedPowerCooldown = config.Bind(section, "Unlimited forsaken powers", false, "Enable usage of forsaken powers even when on cooldown");
    }
  }
}
