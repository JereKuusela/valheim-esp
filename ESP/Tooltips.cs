using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch(typeof(HitData.DamageTypes), "GetTooltipString", new[] { typeof(Skills.SkillType) })]
  public class DamageTypes_GetTooltipStringWithSkill
  {
    public static void Postfix(Skills.SkillType skillType, HitData.DamageTypes __instance, ref string __result)
    {
      if (!Settings.extraInfo || !Settings.allDamageTypes) return;
      if (Player.m_localPlayer == null) return;

      float minFactor;
      float maxFactor;
      Player.m_localPlayer.GetSkills().GetRandomSkillRange(out minFactor, out maxFactor, skillType);
      if (__instance.m_chop != 0f)
        __result += "\n$inventory_chop: " + Patch.DamageTypes_DamageRange(__instance, __instance.m_chop, minFactor, maxFactor) + ", " + Format.String("#CHOP_TIER", "orange");
      if (__instance.m_pickaxe != 0f)
        __result += "\n$inventory_pickaxe: " + Patch.DamageTypes_DamageRange(__instance, __instance.m_pickaxe, minFactor, maxFactor) + ", " + Format.String("#PICKAXE_TIER", "orange");
    }
  }
  [HarmonyPatch(typeof(HitData.DamageTypes), "GetTooltipString", new Type[] { })]
  public class DamageTypes_GetTooltipString
  {
    public static void Postfix(HitData.DamageTypes __instance, ref string __result)
    {
      if (!Settings.extraInfo || !Settings.allDamageTypes) return;
      __result = __result.Replace("$inventory_lightning: <color=yellow>0", "$inventory_lightning: <color=yellow>" + __instance.m_lightning.ToString());
      if (__instance.m_chop != 0f)
        __result += "\n$inventory_chop: " + Format.Int(__instance.m_chop) + " " + Format.String("(#CHOP_TIER)");
      if (__instance.m_pickaxe != 0f)
        __result += "\n$inventory_pickaxe: " + Format.Int(__instance.m_pickaxe) + " " + Format.String("(#PICKAXE_TIER)");
    }
  }
  [HarmonyPatch(typeof(ItemDrop.ItemData), "GetTooltip", new Type[] { typeof(ItemDrop.ItemData), typeof(int), typeof(bool) })]
  public class ItemDropItemData_GetTooltip
  {
    public static void Postfix(ItemDrop.ItemData item, ref string __result)
    {
      if (!Settings.extraInfo || !Settings.allDamageTypes) return;
      var data = item.m_shared;
      __result = __result.Replace("#CHOP_TIER", Texts.GetChopTier(data.m_toolTier));
      __result = __result.Replace("#PICKAXE_TIER", Texts.GetPickaxeTier(data.m_toolTier));
      float minFactor;
      float maxFactor;
      Player.m_localPlayer.GetSkills().GetRandomSkillRange(out minFactor, out maxFactor, data.m_skillType);
      var skillFactor = Player.m_localPlayer.GetSkillFactor(data.m_skillType);
      int minKnockback = Mathf.RoundToInt(data.m_attackForce * minFactor);
      int maxKnockback = Mathf.RoundToInt(data.m_attackForce * maxFactor);
      var knockback = " <color=yellow>(" + minKnockback + "-" + maxKnockback + ")</color>";
      var split = __result.Split('\n').ToList();
      var damage = item.GetDamage();

      var holdDuration = data.m_holdDurationMin * (1f - skillFactor);
      if (data.m_attack != null && Texts.GetAttackSpeed(data.m_attack) != "")
      {
        var attack = data.m_attack;
        if (attack.m_damageMultiplier != 1.0)
          split.Add("Damage: " + Format.Multiplier(attack.m_damageMultiplier, "orange"));
        if (attack.m_staggerMultiplier != 1.0)
          split.Add("Stagger: " + Format.Multiplier(attack.m_staggerMultiplier, "orange"));
        if (attack.m_forceMultiplier != 1.0)
          split.Add("Knockback: " + Format.Multiplier(attack.m_forceMultiplier, "orange"));
        split.Add(Texts.GetStaminaText(attack, data.m_skillType));
        if (!attack.m_lowerDamagePerHit)
          split.Add(Format.String("No multitarget penalty", "orange"));
        split.Add(Texts.GetAttackSpeed(attack, holdDuration, "orange"));
        split.Add(Texts.GetAttackType(attack, "orange"));
        split.Add(Texts.GetProjectileText(attack, holdDuration, "orange"));
        split.Add(Texts.GetHitboxText(attack, "orange"));
      }
      if (data.m_secondaryAttack != null && Texts.GetAttackSpeed(data.m_secondaryAttack) != "")
      {
        var attack = data.m_secondaryAttack;
        split.Add("Secondary");
        if (attack.m_damageMultiplier != 1.0)
          split.Add("Damage: " + Format.Multiplier(attack.m_damageMultiplier, "orange"));
        if (attack.m_staggerMultiplier != 1.0)
          split.Add("Stagger: " + Format.Multiplier(attack.m_staggerMultiplier, "orange"));
        if (attack.m_forceMultiplier != 1.0)
          split.Add("Knockback: " + Format.Multiplier(attack.m_forceMultiplier, "orange"));
        split.Add(Texts.GetStaminaText(attack, data.m_skillType));
        if (!attack.m_lowerDamagePerHit)
          split.Add(Format.String("No multitarget penalty", "orange"));
        split.Add(Texts.GetAttackType(attack, "orange"));
        split.Add(Texts.GetAttackSpeed(attack, holdDuration, "orange"));
        split.Add(Texts.GetProjectileText(attack, holdDuration, "orange"));
        split.Add(Texts.GetHitboxText(attack, "orange"));
      }
      __result = string.Join("\n", split.Where(line => line != "").Select(line => line.StartsWith("$item_knockback") ? line + knockback : line));
    }
  }
}