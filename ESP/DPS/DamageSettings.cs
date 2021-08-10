using HarmonyLib;
using System;
using UnityEngine;

namespace ESP
{
  #region Damage related settings
  [HarmonyPatch(typeof(Skills), "GetSkillFactor")]
  public class Skills_GetSkillFactor
  {
    public static void Postfix(ref float __result)
    {
      if (!Cheats.IsAdmin) return;
      if (Settings.setSkills.Length == 0) return;
      int amount;
      if (!Int32.TryParse(Settings.setSkills, out amount)) return;

      __result = (float)amount / 100f;
    }
  }

  [HarmonyPatch(typeof(Skills), "GetRandomSkillRange")]
  public class Skills_GetRandomSkillRange
  {
    public static bool Prefix(Skills __instance, out float min, out float max, Skills.SkillType skillType)
    {
      // Copy paste from decompiled.
      var skillFactor = __instance.GetSkillFactor(skillType);
      var num = Mathf.Lerp(0.4f, 1f, skillFactor);
      min = Mathf.Clamp01(num - Settings.playerDamageRange);
      max = Mathf.Clamp01(num + Settings.playerDamageRange);
      // Out parameters don't allow returning early so overwrite with default implementation when not admin.
      return !Cheats.IsAdmin;
    }
  }
  [HarmonyPatch(typeof(Skills), "GetRandomSkillFactor")]
  public class Skills_GetRandomSkillFactor
  {
    public static void Postfix(Skills __instance, ref float __result, Skills.SkillType skillType)
    {
      if (!Cheats.IsAdmin) return;
      // Copy paste from decompiled.
      float skillFactor = __instance.GetSkillFactor(skillType);
      float num = Mathf.Lerp(0.4f, 1f, skillFactor);
      float a = Mathf.Clamp01(num - Settings.playerDamageRange);
      float b = Mathf.Clamp01(num + Settings.playerDamageRange);
      __result = Mathf.Lerp(a, b, UnityEngine.Random.value) * (1f + Settings.playerDamageBoost);
    }
  }
  [HarmonyPatch(typeof(Character), "GetRandomSkillFactor")]
  public class Character_GetRandomSkillFactor
  {
    public static void Postfix(ref float __result)
    {
      if (!Cheats.IsAdmin) return;
      __result = UnityEngine.Random.Range(1f - Settings.creatureDamageRange, 1f);
    }
  }
  [HarmonyPatch(typeof(Player), "RPC_UseStamina")]
  public class Player_RPC_UseStamina
  {
    public static void Prefix(ref float v)
    {
      if (!Cheats.IsAdmin) return;
      v *= Settings.playerStaminaUsage;
    }
  }
  [HarmonyPatch(typeof(Player), "IsDodgeInvincible")]
  public class Player_IsDodgeInvincible
  {
    public static void Postfix(ref bool __result)
    {
      if (!Cheats.IsAdmin) return;
      if (Settings.playerForceDodging)
        __result = true;
    }
  }
  [HarmonyPatch(typeof(TerrainComp), "RaiseTerrain")]
  public class TerrainComp_RaiseTerrain
  {
    public static void Prefix(ref float radius)
    {
      if (!Cheats.IsAdmin) return;
      radius *= Settings.terrainEditMultiplier;
    }
  }
  #endregion
}