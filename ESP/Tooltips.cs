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
      if (!Settings.showExtraInfo || !Settings.showAllDamageTypes) return;
      if (Player.m_localPlayer == null) return;

      float minFactor;
      float maxFactor;
      Player.m_localPlayer.GetSkills().GetRandomSkillRange(out minFactor, out maxFactor, skillType);
      if (__instance.m_chop != 0f)
        __result += "\n$inventory_chop: " + Patch.DamageTypes_DamageRange(__instance, __instance.m_chop, minFactor, maxFactor);
      if (__instance.m_pickaxe != 0f)
        __result += "\n$inventory_pickaxe: " + Patch.DamageTypes_DamageRange(__instance, __instance.m_pickaxe, minFactor, maxFactor);
    }
  }
  [HarmonyPatch(typeof(HitData.DamageTypes), "GetTooltipString", new Type[] { })]
  public class DamageTypes_GetTooltipString
  {
    public static void Postfix(HitData.DamageTypes __instance, ref string __result)
    {
      if (!Settings.showExtraInfo || !Settings.showAllDamageTypes) return;

      if (__instance.m_chop != 0f)
        __result += "\n$inventory_chop: " + __instance.m_chop.ToString();
      if (__instance.m_pickaxe != 0f)
        __result += "\n$inventory_pickaxe: " + __instance.m_pickaxe.ToString();
    }
  }
  [HarmonyPatch(typeof(ItemDrop.ItemData), "GetTooltip", new Type[] { typeof(ItemDrop.ItemData), typeof(int), typeof(bool) })]
  public class ItemDropItemData_GetTooltip
  {

    public static void Postfix(ItemDrop.ItemData item, ref string __result)
    {
      if (!Settings.showExtraInfo || !Settings.showAllDamageTypes) return;

      float minFactor;
      float maxFactor;
      Player.m_localPlayer.GetSkills().GetRandomSkillRange(out minFactor, out maxFactor, item.m_shared.m_skillType);
      var skillFactor = Player.m_localPlayer.GetSkillFactor(item.m_shared.m_skillType);
      int minKnockback = Mathf.RoundToInt(item.m_shared.m_attackForce * minFactor);
      int maxKnockback = Mathf.RoundToInt(item.m_shared.m_attackForce * maxFactor);
      var knockback = " <color=yellow>(" + minKnockback + "-" + maxKnockback + ")</color>";
      var split = __result.Split('\n').ToList();

      var holdDuration = item.m_shared.m_holdDurationMin * (1f - skillFactor);
      if (item.m_shared.m_attack != null && Texts.GetAttackSpeed(item.m_shared.m_attack) != "")
      {
        var attack = item.m_shared.m_attack;
        if (attack.m_damageMultiplier != 1.0)
          split.Add("Damage: " + Format.Multiplier(attack.m_damageMultiplier, "orange"));
        if (attack.m_staggerMultiplier != 1.0)
          split.Add("Stagger: " + Format.Multiplier(attack.m_staggerMultiplier, "orange"));
        if (attack.m_forceMultiplier != 1.0)
          split.Add("Knockback: " + Format.Multiplier(attack.m_forceMultiplier, "orange"));
        split.Add(Texts.GetStaminaText(attack, item.m_shared.m_skillType));
        if (!attack.m_lowerDamagePerHit)
          split.Add("No multitarget penalty");
        split.Add(Texts.GetAttackSpeed(attack, holdDuration));
        split.Add(Texts.GetProjectileText(attack, holdDuration));
        split.Add(Texts.GetHitboxText(attack));
      }
      if (item.m_shared.m_secondaryAttack != null && Texts.GetAttackSpeed(item.m_shared.m_secondaryAttack) != "")
      {
        var attack = item.m_shared.m_secondaryAttack;
        split.Add("Secondary");
        if (attack.m_damageMultiplier != 1.0)
          split.Add("Damage: " + Format.Multiplier(attack.m_damageMultiplier, "orange"));
        if (attack.m_staggerMultiplier != 1.0)
          split.Add("Stagger: " + Format.Multiplier(attack.m_staggerMultiplier, "orange"));
        if (attack.m_forceMultiplier != 1.0)
          split.Add("Knockback: " + Format.Multiplier(attack.m_forceMultiplier, "orange"));
        split.Add(Texts.GetStaminaText(attack, item.m_shared.m_skillType));
        if (!attack.m_lowerDamagePerHit)
          split.Add("No multitarget penalty");
        split.Add(Texts.GetAttackSpeed(attack, holdDuration));
        split.Add(Texts.GetProjectileText(attack, holdDuration));
        split.Add(Texts.GetHitboxText(attack));
      }
      __result = string.Join("\n", split.Where(line => line != "").Select(line => line.StartsWith("$item_knockback") ? line + knockback : line));
    }
  }
}