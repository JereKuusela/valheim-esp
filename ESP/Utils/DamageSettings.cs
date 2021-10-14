using Authorization;
using HarmonyLib;

namespace ESP {
  [HarmonyPatch(typeof(Player), "RPC_UseStamina")]
  public class Player_RPC_UseStamina {
    public static void Prefix(ref float v) {
      if (!Admin.Enabled) return;
      v *= Settings.PlayerStaminaUsage;
    }
  }
  [HarmonyPatch(typeof(Player), "IsDodgeInvincible")]
  public class Player_IsDodgeInvincible {
    public static void Postfix(ref bool __result) {
      if (!Admin.Enabled) return;
      if (Settings.PlayerForceDodging)
        __result = true;
    }
  }
  [HarmonyPatch(typeof(Skills), "GetRandomSkillFactor")]
  public class Skills_GetRandomSkillFactor {
    public static void Postfix(Skills __instance, ref float __result, Skills.SkillType skillType) {
      if (!Admin.Enabled) return;
      __result *= (1f + Settings.PlayerDamageBoost);
    }
  }
}