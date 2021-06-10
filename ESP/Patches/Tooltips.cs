using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace ESP
{
  [HarmonyPatch(typeof(HitData.DamageTypes), "GetTooltipString", new[] { typeof(Skills.SkillType) })]
  public class DamageTypes_GetTooltipStringWithSkill
  {
    public static void Postfix(Skills.SkillType skillType, HitData.DamageTypes __instance, ref string __result)
    {
      if (!Settings.showAllDamageTypes)
        return;
      if (Player.m_localPlayer == null)
        return;

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
      if (!Settings.showAllDamageTypes)
        return;

      if (__instance.m_chop != 0f)
        __result += "\n$inventory_chop: " + __instance.m_chop.ToString();
      if (__instance.m_pickaxe != 0f)
        __result += "\n$inventory_pickaxe: " + __instance.m_pickaxe.ToString();
    }
  }
  [HarmonyPatch(typeof(ItemDrop.ItemData), "GetTooltip", new Type[] { typeof(ItemDrop.ItemData), typeof(int), typeof(bool) })]
  public class ItemDropItemData_GetTooltip
  {
    private static string GetStaminaText(Attack attack, Skills.SkillType skillType)
    {
      if (attack == null) return "";
      var maxStamina = attack.m_attackStamina;
      var skillFactor = Player.m_localPlayer.GetSkillFactor(skillType);
      var stamina = maxStamina * (1 - 0.33f * skillFactor);
      return "Stamina: " + TextUtils.Int(maxStamina, "orange") + " " + TextUtils.String("(" + TextUtils.Float(stamina) + ")");
    }
    private static string GetHitboxText(Attack attack)
    {
      if (attack == null) return "";
      var texts = new List<string>();
      if (attack.m_attackRange > 0)
        texts.Add(TextUtils.Float(attack.m_attackRange, TextUtils.FORMAT, "orange") + " m");
      if (attack.m_attackAngle > 0)
        texts.Add(TextUtils.Float(attack.m_attackAngle, TextUtils.FORMAT, "orange") + " deg");
      if (attack.m_attackHeight > 0)
        texts.Add(TextUtils.Float(attack.m_attackHeight, TextUtils.FORMAT, "orange") + " h");
      if (attack.m_attackRayWidth > 0)
        texts.Add(TextUtils.Float(attack.m_attackRayWidth, TextUtils.FORMAT, "orange") + " ray");
      if (attack.m_attackOffset > 0)
        texts.Add(TextUtils.Float(attack.m_attackOffset, TextUtils.FORMAT, "orange") + " offset");
      return "Hit: " + string.Join(", ", texts);
    }
    private static string GetAttackSpeed(string animation)
    {
      if (animation == "atgeir_attack") return "2.98 s (0.84 + 0.86 + 1.28)";
      if (animation == "atgeir_secondary") return "1.74 s";
      if (animation == "battleaxe_attack") return "3.44 s (1.82 + 0.92 + 0.7)";
      if (animation == "battleaxe_secondary") return "";
      if (animation == "bow_fire") return "0.94 s";
      if (animation == "emote_drink") return "";
      if (animation == "knife_stab") return "2.04 s (0.88 + 0.54 + 0.62)";
      if (animation == "knife_secondary") return "1.72 s";
      if (animation == "mace_secondary") return "1.72 s";
      if (animation == "spear_poke") return "0.68 s";
      if (animation == "spear_throw") return "1.57 s (includes picking up)";
      if (animation == "swing_axe") return "2.74 s (0.93 + 0.69 + 1.12)";
      if (animation == "swing_hammer") return "";
      if (animation == "swing_hoe") return "";
      // Also used for the mace.
      if (animation == "swing_longsword") return "2.46 s (1.02 + 0.68 + 0.76)";
      if (animation == "swing_pickaxe") return "1.4 s";
      if (animation == "swing_sledge") return "2.23 s";
      if (animation == "sword_secondary") return "1.84 s";
      return "";
    }
    private static string GetAttackSpeed(Attack attack)
    {
      if (attack == null) return "";
      var animation = attack.m_attackAnimation;
      var text = GetAttackSpeed(animation);
      if (text == "") return "";
      text = text.Replace("(", "<color=yellow>(");
      text = text.Replace(")", ")</color>");
      return "Speed: " + TextUtils.String(text, "orange");
    }
    public static void Postfix(ItemDrop.ItemData item, ref string __result)
    {
      if (!Settings.showAllDamageTypes)
        return;

      float minFactor;
      float maxFactor;
      Player.m_localPlayer.GetSkills().GetRandomSkillRange(out minFactor, out maxFactor, item.m_shared.m_skillType);
      int minKnockback = Mathf.RoundToInt(item.m_shared.m_attackForce * minFactor);
      int maxKnockback = Mathf.RoundToInt(item.m_shared.m_attackForce * maxFactor);
      var knockback = " <color=yellow>(" + minKnockback + "-" + maxKnockback + ")</color>";
      var split = __result.Split('\n').ToList();
      if (item.m_shared.m_attack != null && GetAttackSpeed(item.m_shared.m_attack) != "")
      {
        var attack = item.m_shared.m_attack;
        if (attack.m_damageMultiplier != 1.0)
          split.Add("Damage: " + TextUtils.Multiplier(attack.m_damageMultiplier, "orange"));
        if (attack.m_staggerMultiplier != 1.0)
          split.Add("Stagger: " + TextUtils.Multiplier(attack.m_staggerMultiplier, "orange"));
        if (attack.m_forceMultiplier != 1.0)
          split.Add("Knockback: " + TextUtils.Multiplier(attack.m_forceMultiplier, "orange"));
        split.Add(GetStaminaText(attack, item.m_shared.m_skillType));
        if (!attack.m_lowerDamagePerHit)
          split.Add("No multitarget penalty");
        split.Add(GetAttackSpeed(attack));
        split.Add(GetHitboxText(attack));
      }
      if (item.m_shared.m_secondaryAttack != null && GetAttackSpeed(item.m_shared.m_secondaryAttack) != "")
      {
        var attack = item.m_shared.m_secondaryAttack;
        split.Add("Secondary");
        if (attack.m_damageMultiplier != 1.0)
          split.Add("Damage: " + TextUtils.Multiplier(attack.m_damageMultiplier, "orange"));
        if (attack.m_staggerMultiplier != 1.0)
          split.Add("Stagger: " + TextUtils.Multiplier(attack.m_staggerMultiplier, "orange"));
        if (attack.m_forceMultiplier != 1.0)
          split.Add("Knockback: " + TextUtils.Multiplier(attack.m_forceMultiplier, "orange"));
        split.Add(GetStaminaText(attack, item.m_shared.m_skillType));
        if (!attack.m_lowerDamagePerHit)
          split.Add("No multitarget penalty");
        split.Add(GetAttackSpeed(attack));
        split.Add(GetHitboxText(attack));
      }
      __result = string.Join("\n", split.Select(line => line.StartsWith("$item_knockback") ? line + knockback : line));
    }
  }
}