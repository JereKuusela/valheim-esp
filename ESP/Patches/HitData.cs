using HarmonyLib;
using System;

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
}