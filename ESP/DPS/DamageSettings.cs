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
      if (Settings.setSkills.Length == 0) return;
      int amount;
      if (!Int32.TryParse(Settings.setSkills, out amount)) return;

      __result = (float)amount / 100f;
    }
  }

  [HarmonyPatch(typeof(Skills), "GetRandomSkillRange")]
  public class Skills_GetRandomSkillRange
  {
    public static void Postfix(Skills __instance, out float min, out float max, Skills.SkillType skillType)
    {
      // Copy paste from decompiled.
      var skillFactor = __instance.GetSkillFactor(skillType);
      var num = Mathf.Lerp(0.4f, 1f, skillFactor);
      min = Mathf.Clamp01(num - Settings.playerDamageRange);
      max = Mathf.Clamp01(num + Settings.playerDamageRange);
    }
  }
  [HarmonyPatch(typeof(Skills), "GetRandomSkillFactor")]
  public class Skills_GetRandomSkillFactor
  {
    public static void Postfix(Skills __instance, ref float __result, Skills.SkillType skillType)
    {
      // Copy paste from decompiled.
      float skillFactor = __instance.GetSkillFactor(skillType);
      float num = Mathf.Lerp(0.4f, 1f, skillFactor);
      float a = Mathf.Clamp01(num - Settings.playerDamageRange);
      float b = Mathf.Clamp01(num + Settings.playerDamageRange);
      __result = Mathf.Lerp(a, b, UnityEngine.Random.value);
    }
  }
  [HarmonyPatch(typeof(Character), "GetRandomSkillFactor")]
  public class Character_GetRandomSkillFactor
  {
    public static void Postfix(ref float __result)
    {
      __result = UnityEngine.Random.Range(1f - Settings.creatureDamageRange, 1f);
    }
  }
  #endregion
}