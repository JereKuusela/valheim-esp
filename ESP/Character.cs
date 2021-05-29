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
    public static string GetNameText(Character character) => TextUtils.StringValue(character.m_name);

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
    public static void Postfix(Character __instance, ref string __result, float ___m_staggerDamage, Rigidbody ___m_body)
    {
      if (!Settings.showCreatureStats)
        return;
      var health = __instance.GetMaxHealth();
      __result += "\n" + "Health: " + TextUtils.FloatValue(__instance.m_health) + "/" + TextUtils.IntValue(health);
      __result += GetStaggerText(health, __instance.m_staggerDamageFactor, ___m_staggerDamage);
      __result += "\n" + "Mass: " + TextUtils.IntValue(___m_body.mass) + " (" + TextUtils.PercentValue(1f - 5f / ___m_body.mass) + " knockback resistance)";
      var damageModifiers = Patch.Character_GetDamageModifiers(__instance);
      __result += GetText(damageModifiers, HitData.DamageType.Blunt);
      __result += GetText(damageModifiers, HitData.DamageType.Chop);
      __result += GetText(damageModifiers, HitData.DamageType.Elemental);
      __result += GetText(damageModifiers, HitData.DamageType.Fire);
      __result += GetText(damageModifiers, HitData.DamageType.Frost);
      __result += GetText(damageModifiers, HitData.DamageType.Lightning);
      __result += GetText(damageModifiers, HitData.DamageType.Physical);
      __result += GetText(damageModifiers, HitData.DamageType.Pickaxe);
      __result += GetText(damageModifiers, HitData.DamageType.Pierce);
      __result += GetText(damageModifiers, HitData.DamageType.Poison);
      __result += GetText(damageModifiers, HitData.DamageType.Slash);
      __result += GetText(damageModifiers, HitData.DamageType.Spirit);
    }
  }
}