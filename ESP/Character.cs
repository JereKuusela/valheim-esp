using HarmonyLib;
using UnityEngine;
using System;

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
    public static string GetNameText(float range) => "Noise: " + TextUtils.IntValue(range);
    public static string GetNameText(Character character) => TextUtils.StringValue(Localization.instance.Localize(character.m_name));

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

  [HarmonyPatch(typeof(Character), "GetHoverText")]
  public class Character_GetHoverText
  {
    private static string DamageTypeToString(HitData.DamageType damageType)
    {
      if (damageType == HitData.DamageType.Blunt) return "Blunt";
      if (damageType == HitData.DamageType.Chop) return "Chop";
      if (damageType == HitData.DamageType.Elemental) return "Elemental";
      if (damageType == HitData.DamageType.Fire) return "Fire";
      if (damageType == HitData.DamageType.Frost) return "Frost";
      if (damageType == HitData.DamageType.Lightning) return "Lightning";
      if (damageType == HitData.DamageType.Physical) return "Physical";
      if (damageType == HitData.DamageType.Pickaxe) return "Pickaxe";
      if (damageType == HitData.DamageType.Pierce) return "Pierce";
      if (damageType == HitData.DamageType.Poison) return "Poison";
      if (damageType == HitData.DamageType.Slash) return "Slash";
      if (damageType == HitData.DamageType.Spirit) return "Spirit";
      return "";
    }
    private static string ModifierToText(HitData.DamageModifiers modifiers, HitData.DamageType damageType)
    {
      var name = DamageTypeToString(damageType);
      var modifier = modifiers.GetModifier(damageType);
      if (modifier == HitData.DamageModifier.Immune) return name + ": " + TextUtils.StringValue("x0");
      if (modifier == HitData.DamageModifier.Resistant) return name + ": " + TextUtils.StringValue("x0.5");
      if (modifier == HitData.DamageModifier.VeryResistant) return name + ": " + TextUtils.StringValue("x0.25");
      if (modifier == HitData.DamageModifier.Weak) return name + ": " + TextUtils.StringValue("x1.5");
      if (modifier == HitData.DamageModifier.VeryWeak) return name + ": " + TextUtils.StringValue("x2");
      return "";
    }
    private static string GetText(HitData.DamageModifiers modifiers, HitData.DamageType damageType)
    {
      var text = ModifierToText(modifiers, damageType);
      if (text == "") return "";
      return "\n" + text;
    }
    private static string GetStaggerText(float health, float staggerDamageFactor, float staggerDamage)
    {
      var staggerLimit = staggerDamageFactor * health;
      if (staggerLimit > 0)
        return "\n" + "Stagger: " + TextUtils.FloatValue(staggerDamage) + "/" + TextUtils.FloatValue(staggerLimit);
      else
        return "\n" + "Stagger: " + TextUtils.StringValue("Immune");
    }
    private static string GetCreatureStats(Character instance, MonsterAI monsterAI, float staggerDamage, Rigidbody body)
    {
      if (!Settings.showCreatureStats)
        return "";
      var stats = "";
      if (monsterAI && monsterAI.IsAlerted())
        stats += "\n<color=red>Alerted</color>";
      var health = instance.GetMaxHealth();
      stats += "\n" + "Health: " + TextUtils.FloatValue(instance.m_health) + "/" + TextUtils.IntValue(health);
      stats += GetStaggerText(health, instance.m_staggerDamageFactor, staggerDamage);
      stats += "\n" + "Mass: " + TextUtils.IntValue(body.mass) + " (" + TextUtils.PercentValue(1f - 5f / body.mass) + " knockback resistance)";
      var damageModifiers = Patch.Character_GetDamageModifiers(instance);
      stats += GetText(damageModifiers, HitData.DamageType.Blunt);
      stats += GetText(damageModifiers, HitData.DamageType.Chop);
      stats += GetText(damageModifiers, HitData.DamageType.Elemental);
      stats += GetText(damageModifiers, HitData.DamageType.Fire);
      stats += GetText(damageModifiers, HitData.DamageType.Frost);
      stats += GetText(damageModifiers, HitData.DamageType.Lightning);
      stats += GetText(damageModifiers, HitData.DamageType.Physical);
      stats += GetText(damageModifiers, HitData.DamageType.Pickaxe);
      stats += GetText(damageModifiers, HitData.DamageType.Pierce);
      stats += GetText(damageModifiers, HitData.DamageType.Poison);
      stats += GetText(damageModifiers, HitData.DamageType.Slash);
      stats += GetText(damageModifiers, HitData.DamageType.Spirit);
      return stats;
    }

    private static string GetGrowupStats(BaseAI baseAI, Growup growup)
    {
      if (!Settings.showBreedingStats || !baseAI || !growup)
        return "";
      var value = baseAI.GetTimeSinceSpawned().TotalSeconds;
      var limit = growup.m_growTime;
      return "\n" + TextUtils.ProgressValue("Progress", value, limit);
    }
    public static void Postfix(Character __instance, ref string __result, float ___m_staggerDamage, Rigidbody ___m_body)
    {
      var baseAI = __instance.GetComponent<BaseAI>();
      var growup = __instance.GetComponent<Growup>();
      var tameable = __instance.GetComponent<Tameable>();
      var monsterAI = __instance.GetComponent<MonsterAI>();
      __result += GetCreatureStats(__instance, monsterAI, ___m_staggerDamage, ___m_body);
      __result += GetGrowupStats(baseAI, growup);
    }
  }
}