using System;
using HarmonyLib;
using UnityEngine;

namespace ESP {
  #region Damage related settings
  [HarmonyPatch(typeof(Skills), "GetSkillFactor")]
  public class Skills_GetSkillFactor {
    public static void Postfix(ref float __result) {
      if (!Admin.Enabled) return;
      if (Settings.SetSkills.Length == 0) return;
      int amount;
      if (!Int32.TryParse(Settings.SetSkills, out amount)) return;

      __result = (float)amount / 100f;
    }
  }

  [HarmonyPatch(typeof(Skills), "GetRandomSkillRange")]
  public class Skills_GetRandomSkillRange {
    public static bool Prefix(Skills __instance, out float min, out float max, Skills.SkillType skillType) {
      // Copy paste from decompiled.
      var skillFactor = __instance.GetSkillFactor(skillType);
      var num = Mathf.Lerp(0.4f, 1f, skillFactor);
      min = Mathf.Clamp01(num - Settings.PlayerDamageRange);
      max = Mathf.Clamp01(num + Settings.PlayerDamageRange);
      // Out parameters don't allow returning early so overwrite with default implementation when not admin.
      return !Admin.Enabled;
    }
  }
  [HarmonyPatch(typeof(Skills), "GetRandomSkillFactor")]
  public class Skills_GetRandomSkillFactor {
    public static void Postfix(Skills __instance, ref float __result, Skills.SkillType skillType) {
      if (!Admin.Enabled) return;
      // Copy paste from decompiled.
      float skillFactor = __instance.GetSkillFactor(skillType);
      float num = Mathf.Lerp(0.4f, 1f, skillFactor);
      float a = Mathf.Clamp01(num - Settings.PlayerDamageRange);
      float b = Mathf.Clamp01(num + Settings.PlayerDamageRange);
      __result = Mathf.Lerp(a, b, UnityEngine.Random.value) * (1f + Settings.PlayerDamageBoost);
    }
  }
  [HarmonyPatch(typeof(Character), "GetRandomSkillFactor")]
  public class Character_GetRandomSkillFactor {
    public static void Postfix(ref float __result) {
      if (!Admin.Enabled) return;
      __result = UnityEngine.Random.Range(1f - Settings.CreatureDamageRange, 1f);
    }
  }
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
  [HarmonyPatch(typeof(Player), "StartGuardianPower")]
  public class Player_StartGuardianPower {
    public static void Prefix(ref float ___m_guardianPowerCooldown) {
      if (!Admin.Enabled) return;
      if (Settings.IgnoreForsakedPowerCooldown)
        ___m_guardianPowerCooldown = 0;
    }
  }
  [HarmonyPatch(typeof(TerrainComp), "RaiseTerrain")]
  public class TerrainComp_RaiseTerrain {
    public static void Prefix(ref float radius) {
      if (!Admin.Enabled) return;
      radius *= Settings.TerrainEditMultiplier;
    }
  }
  [HarmonyPatch(typeof(Attack), "Start")]
  public class Attack_Start_CapChain {
    public static void Prefix(ref Attack previousAttack, ref int ___m_currentAttackCainLevel) {
      if (!Admin.Enabled || Settings.MaxAttackChainLevels == 0) return;
      if (previousAttack == null) return;
      var nextLevel = Patch.Get<int>(previousAttack, "m_nextAttackChainLevel");
      if (nextLevel >= Settings.MaxAttackChainLevels) {
        previousAttack = null;
        ___m_currentAttackCainLevel = 0;
      }
    }
  }
  #endregion
}