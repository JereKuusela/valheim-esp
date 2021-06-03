using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESP
{
  public class CharacterUtils
  {
    public static bool IsExcluded(Character instance)
    {
      var name = instance.name.ToLower();
      var m_name = instance.m_name.ToLower();
      var localized = Localization.instance.Localize(instance.m_name).ToLower();
      var excluded = Settings.excludedCreatures.ToLower().Split(',');
      return Array.Exists(excluded, item => item == name || item == m_name || item == localized);
    }
    public static string GetNameText(float range) => "Noise: " + TextUtils.Int(range);
    public static string GetNameText(Character character) => TextUtils.String(Localization.instance.Localize(character.m_name));

  }
  [HarmonyPatch(typeof(Character), "Awake")]
  public class Character_Awake
  {
    private static void DrawNoise(Character instance, float noiseRange)
    {
      if (!Settings.showNoise || CharacterUtils.IsExcluded(instance))
        return;
      var text = CharacterUtils.GetNameText(instance) + "\n" + CharacterUtils.GetNameText(noiseRange);
      Drawer.DrawSphere(instance.gameObject, instance.transform.position, noiseRange, Color.cyan, 0.1f, text);

    }
    public static void Postfix(Character __instance, float ___m_noiseRange)
    {
      DrawNoise(__instance, ___m_noiseRange);
    }
  }

  [HarmonyPatch(typeof(Character), "UpdateNoise")]
  public class Character_UpdateNoise : Component
  {
    public static void Postfix(Character __instance, float ___m_noiseRange)
    {
      if (!Settings.showNoise || CharacterUtils.IsExcluded(__instance))
        return;
      var text = CharacterUtils.GetNameText(__instance) + "\n" + CharacterUtils.GetNameText(___m_noiseRange);
      Drawer.UpdateSphere(__instance.gameObject, Vector3.zero, ___m_noiseRange, text);
    }
  }

  struct DropChance
  {
    public float chance;
    public int min;
    public int max;
    public bool perPlayer;
  }

  [HarmonyPatch(typeof(Character), "GetHoverText")]
  public class Character_GetHoverText
  {
    private static string GetStaggerText(float health, float staggerDamageFactor, float staggerDamage)
    {
      var staggerLimit = staggerDamageFactor * health;
      if (staggerLimit > 0)
        return "\n" + "Stagger: " + TextUtils.Progress(staggerDamage, staggerLimit);
      else
        return "\n" + "Stagger: " + TextUtils.String("Immune");
    }
    private static string GetCreatureStats(Character instance, BaseAI baseAI, MonsterAI monsterAI, float staggerDamage, Rigidbody body)
    {
      if (!Settings.showCreatureStats)
        return "";
      var stats = "";
      if (monsterAI && (monsterAI.IsAlerted() || monsterAI.HuntPlayer()))
      {
        var mode = "";
        if (monsterAI.IsAlerted())
          mode += "Alerted";
        if (monsterAI.IsAlerted() && monsterAI.HuntPlayer())
          mode += ", ";
        if (monsterAI.HuntPlayer())
          mode += "Hunt mode";
        stats += "\n<color=red>" + mode + "</color>";
      }
      var health = instance.GetMaxHealth();
      stats += "\n" + TextUtils.GetHealth(instance.GetHealth(), health);
      stats += GetStaggerText(health, instance.m_staggerDamageFactor, staggerDamage);
      stats += "\n" + "Mass: " + TextUtils.Int(body.mass) + " (" + TextUtils.Percent(1f - 5f / body.mass) + " knockback resistance)";
      var damageModifiers = Patch.Character_GetDamageModifiers(instance);
      stats += DamageModifierUtils.GetText(damageModifiers);
      if (baseAI)
      {
        Vector3 patrolPoint;
        var patrol = baseAI.GetPatrolPoint(out patrolPoint);
        var patrolText = patrol ? patrolPoint.ToString("F0") : "No patrol";
        stats += "\nPatrol: " + TextUtils.String(patrolText);
      }
      if (monsterAI.m_consumeItems.Count > 0)
      {
        var heal = " (" + TextUtils.Int(monsterAI.m_consumeHeal) + " health)";
        var items = monsterAI.m_consumeItems.Select(item => TextUtils.String(Utils.GetPrefabName(item.gameObject)));
        stats += "\n" + string.Join(", ", items) + heal;
      }
      return stats;
    }

    private static string GetStatusStats(Character character)
    {
      if (!Settings.showStatusEffects || !character)
        return "";
      var statusEffects = character.GetSEMan().GetStatusEffects();
      var text = "";
      foreach (var status in statusEffects)
      {
        text += "\n" + Localization.instance.Localize(status.m_name) + ": " + TextUtils.Progress(status.GetRemaningTime(), status.m_ttl) + " seconds";
      }
      return text;
    }
    private static string GetGrowupStats(BaseAI baseAI, Growup growup)
    {
      if (!Settings.showBreedingStats || !baseAI || !growup)
        return "";
      var value = baseAI.GetTimeSinceSpawned().TotalSeconds;
      var limit = growup.m_growTime;
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }

    private static List<string> GetDropTexts(CharacterDrop characterDrop, Character character)
    {
      var list = new List<string>();
      int num = character ? Mathf.Max(1, (int)Mathf.Pow(2f, (float)(character.GetLevel() - 1))) : 1;
      foreach (CharacterDrop.Drop drop in characterDrop.m_drops)
      {
        if (!(drop.m_prefab == null))
        {
          float chance = drop.m_chance;
          if (drop.m_levelMultiplier)
          {
            chance *= (float)num;
          }
          int min = drop.m_amountMin;
          int max = drop.m_amountMax;
          if (drop.m_levelMultiplier)
          {
            min *= num;
            max *= num;
          }
          var dropChance = new DropChance()
          {
            chance = chance,
            max = max,
            min = min,
            perPlayer = drop.m_onePerPlayer
          };
          var text = "";
          if (max > 1 || (max == 1 && chance >= 1.0)) text += TextUtils.Range(min, max) + " ";
          text += drop.m_prefab.name;
          if (drop.m_onePerPlayer) text += " (per player)";
          if (chance < 1.0) text += " (" + TextUtils.Percent(chance) + ")";
          list.Add(text);
        }
      }
      return list;
    }
    private static string GetDropStats(CharacterDrop characterDrop, Character character)
    {
      if (!Settings.showDropStats || !characterDrop)
        return "";
      var dropTexts = GetDropTexts(characterDrop, character).Join(null, ", ");
      return "\nDrops: " + dropTexts;
    }
    public static void Postfix(Character __instance, ref string __result, float ___m_staggerDamage, Rigidbody ___m_body)
    {
      var baseAI = __instance.GetComponent<BaseAI>();
      var growup = __instance.GetComponent<Growup>();
      var tameable = __instance.GetComponent<Tameable>();
      var monsterAI = __instance.GetComponent<MonsterAI>();
      var characterDrop = __instance.GetComponent<CharacterDrop>();
      __result += GetCreatureStats(__instance, baseAI, monsterAI, ___m_staggerDamage, ___m_body);
      __result += GetStatusStats(__instance);
      __result += GetGrowupStats(baseAI, growup);
      __result += GetDropStats(characterDrop, __instance);
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
}