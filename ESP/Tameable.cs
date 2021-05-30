using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(Tameable), "GetHoverText")]
  public class Tameable_GetHoverText
  {
    private static string GetFedText(Tameable instance, ZNetView nview)
    {
      if (!Settings.showBreedingStats) return "";
      var feedingTime = nview.GetZDO().GetLong("TameLastFeeding", 0L);
      DateTime d = new DateTime(nview.GetZDO().GetLong("TameLastFeeding", 0L));
      var value = feedingTime == 0 ? 0 : instance.m_fedDuration - (ZNet.instance.GetTime() - d).TotalSeconds;
      var limit = instance.m_fedDuration;
      return "\n" + TextUtils.ProgressValue("Food", value, limit);
    }
    private static string GetTamingText(Tameable instance)
    {
      if (!Settings.showBreedingStats) return "";
      var value = instance.m_tamingTime - Patch.Tameable_GetRemainingTime(instance);
      var limit = instance.m_tamingTime;
      return "\n" + TextUtils.ProgressValue("Taming", value, limit);
    }
    private static string GetPregnancyText(Procreation instance, ZNetView nview)
    {
      if (!Settings.showBreedingStats || !instance || !instance.IsPregnant()) return "";
      DateTime d = new DateTime(nview.GetZDO().GetLong("pregnant", 0L));
      var value = (ZNet.instance.GetTime() - d).TotalSeconds;
      var limit = instance.m_pregnancyDuration;
      return "\n" + TextUtils.ProgressValue("Pregnancy", value, limit);
    }
    private static string GetLoveText(Procreation instance, ZNetView nview)
    {
      if (!Settings.showBreedingStats || !instance || instance.IsPregnant()) return "";
      var value = nview.GetZDO().GetInt("lovePoints", 0);
      var limit = instance.m_requiredLovePoints;
      return "\nBreeding: " + TextUtils.StringValue(value + "/" + limit) + ", " + TextUtils.PercentValue(instance.m_pregnancyChance) + " chance every " + TextUtils.IntValue(instance.m_updateInterval) + " seconds";
    }
    private static string GetPartnersText(Procreation instance, ZNetView nview)
    {
      if (!Settings.showBreedingStats || !instance || instance.IsPregnant()) return "";
      var prefab = ZNetScene.instance.GetPrefab(nview.GetZDO().GetPrefab());
      var partners = SpawnSystem.GetNrOfInstances(prefab, instance.transform.position, instance.m_partnerCheckRange, false, true) - 1;
      return "\nPartners: " + TextUtils.IntValue(partners) + " within " + instance.m_partnerCheckRange + " meters";
    }
    private static string GetLimitText(Procreation instance, ZNetView nview)
    {
      if (!Settings.showBreedingStats || !instance) return "";
      var offspring = ZNetScene.instance.GetPrefab(Utils.GetPrefabName(instance.m_offspring));
      var prefab = ZNetScene.instance.GetPrefab(nview.GetZDO().GetPrefab());
      var ownAmount = SpawnSystem.GetNrOfInstances(prefab, instance.transform.position, instance.m_totalCheckRange, false, false);
      var offspringAmount = SpawnSystem.GetNrOfInstances(offspring, instance.transform.position, instance.m_totalCheckRange, false, false);
      var value = ownAmount + offspringAmount;
      var limit = instance.m_maxCreatures;
      return "\nLimit: " + TextUtils.StringValue(value + "/" + limit) + " within " + instance.m_totalCheckRange + " meters";
    }
    public static void Postfix(Tameable __instance, Character ___m_character, ZNetView ___m_nview, ref string __result)
    {
      var procreation = __instance.GetComponent<Procreation>();
      __result += GetFedText(__instance, ___m_nview);
      if (___m_character.IsTamed())
      {
        __result += GetLoveText(procreation, ___m_nview);
        __result += GetPregnancyText(procreation, ___m_nview);
        __result += GetPartnersText(procreation, ___m_nview);
        __result += GetLimitText(procreation, ___m_nview);
      }
      else
      {
        __result += GetTamingText(__instance);
      }
    }
  }
}