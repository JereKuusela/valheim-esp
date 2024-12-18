using System;
using System.Linq;
using HarmonyLib;
using Service;
using UnityEngine;
namespace ESP;
[HarmonyPatch(typeof(HitData.DamageTypes), nameof(HitData.DamageTypes.GetTooltipString), [typeof(Skills.SkillType)])]
public class DamageTypes_GetTooltipStringWithSkill
{
  static void Postfix(Skills.SkillType skillType, HitData.DamageTypes __instance, ref string __result)
  {
    if (!Settings.ExtraInfo || !Settings.WeaponInfo) return;
    if (Player.m_localPlayer == null) return;
    var obj = __instance;

    Player.m_localPlayer.GetSkills().GetRandomSkillRange(out float min, out float max, skillType);
    if (obj.m_chop != 0f)
      __result += $"\n$inventory_chop: <color=orange>{Mathf.RoundToInt(obj.m_chop)}</color> <color=#FFFF00>({Mathf.RoundToInt(obj.m_chop * min)}-{Mathf.RoundToInt(obj.m_chop * max)}) </color>, {Format.String("<CHOP_TIER>", "orange")}";
    if (obj.m_pickaxe != 0f)
      __result += $"\n$inventory_pickaxe: <color=orange>{Mathf.RoundToInt(obj.m_pickaxe)}</color> <color=#FFFF00>({Mathf.RoundToInt(obj.m_pickaxe * min)}-{Mathf.RoundToInt(obj.m_pickaxe * max)}) </color>, {Format.String("¤PICKAXE_TIER", "orange")}";
  }
}
[HarmonyPatch(typeof(HitData.DamageTypes), nameof(HitData.DamageTypes.GetTooltipString), [])]
public class DamageTypes_GetTooltipString
{
  static void Postfix(HitData.DamageTypes __instance, ref string __result)
  {
    if (!Settings.ExtraInfo || !Settings.WeaponInfo) return;
    if (__instance.m_chop != 0f)
      __result += "\n$inventory_chop: " + Format.Int(__instance.m_chop) + " " + Format.String("(<CHOP_TIER>)");
    if (__instance.m_pickaxe != 0f)
      __result += "\n$inventory_pickaxe: " + Format.Int(__instance.m_pickaxe) + " " + Format.String("(<PICKAXE_TIER>)");
  }
}
[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetTooltip), typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float), typeof(int))]
public class ItemDropItemData_GetTooltip
{
  static void Postfix(ItemDrop.ItemData item, ref string __result)
  {
    if (!Settings.ExtraInfo || !Settings.WeaponInfo) return;
    var data = item.m_shared;
    __result = __result.Replace("<CHOP_TIER>", Texts.GetChopTier(data.m_toolTier));
    __result = __result.Replace("<PICKAXE_TIER>", Texts.GetPickaxeTier(data.m_toolTier));
    Player.m_localPlayer.GetSkills().GetRandomSkillRange(out float minFactor, out float maxFactor, data.m_skillType);
    var skillFactor = Player.m_localPlayer.GetSkillFactor(data.m_skillType);
    int minKnockback = Mathf.RoundToInt(data.m_attackForce * minFactor);
    int maxKnockback = Mathf.RoundToInt(data.m_attackForce * maxFactor);
    var knockback = " <color=#FFFF00>(" + minKnockback + "-" + maxKnockback + ")</color>";
    var split = __result.Split('\n').ToList();
    var damage = item.GetDamage();

    var drawDuration = data.m_attack.m_drawDurationMin;
    drawDuration = Mathf.Lerp(drawDuration, drawDuration * 0.2f, skillFactor);
    if (data.m_attack != null && data.m_attack.m_attackAnimation != "")
    {
      var attack = data.m_attack;
      if (attack.m_damageMultiplier != 1.0)
        split.Add("Damage: " + Format.Multiplier(attack.m_damageMultiplier, "orange"));
      if (attack.m_staggerMultiplier != 1.0)
        split.Add("Stagger: " + Format.Multiplier(attack.m_staggerMultiplier, "orange"));
      if (attack.m_forceMultiplier != 1.0)
        split.Add("Knockback: " + Format.Multiplier(attack.m_forceMultiplier, "orange"));
      split.Add(Texts.GetStaminaText(attack, data.m_skillType));
      split.Add(Texts.GetEitrText(attack, data.m_skillType));
      split.Add(Texts.GetHealthText(attack, data.m_skillType));
      if (!attack.m_lowerDamagePerHit)
        split.Add(Format.String("No multitarget penalty", "orange"));
      split.Add(Texts.GetAttackSpeed(attack, drawDuration, "orange"));
      split.Add(Texts.GetAttackType(attack, "orange"));
      split.Add(Texts.GetProjectileText(attack, drawDuration, "orange"));
      split.Add(Texts.GetHitboxText(attack, "orange"));
      if (attack.m_raiseSkillAmount != 1.0)
        split.Add("Experience gain: " + Format.Multiplier(attack.m_raiseSkillAmount, "orange"));
      if (attack.m_requiresReload && attack.m_reloadTime != 1.0)
        split.Add("Reload: " + Format.Float(Mathf.Lerp(attack.m_reloadTime, attack.m_reloadTime * 0.5f, skillFactor), "orange"));
    }
    if (data.m_secondaryAttack != null && data.m_secondaryAttack.m_attackAnimation != "")
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
      split.Add(Texts.GetEitrText(attack, data.m_skillType));
      split.Add(Texts.GetHealthText(attack, data.m_skillType));
      if (!attack.m_lowerDamagePerHit)
        split.Add(Format.String("No multitarget penalty", "orange"));
      split.Add(Texts.GetAttackType(attack, "orange"));
      split.Add(Texts.GetAttackSpeed(attack, drawDuration, "orange"));
      split.Add(Texts.GetProjectileText(attack, drawDuration, "orange"));
      split.Add(Texts.GetHitboxText(attack, "orange"));
      if (attack.m_raiseSkillAmount != 1.0)
        split.Add("Experience gain: " + Format.Multiplier(attack.m_raiseSkillAmount, "orange"));
    }
    __result = Format.JoinLines(split.Where(line => line != "").Select(line => line.StartsWith("$item_knockback") ? line + knockback : line));
  }
}
