using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch]
  public class Patch
  {
    public static float m_cover(Windmill instance) => Traverse.Create(instance).Field<float>("m_cover").Value;
    public static Vector3 m_currentVel(Player instance) => Traverse.Create(instance).Field<Vector3>("m_currentVel").Value;
    public static ZNetView m_nview(TreeLog instance) => Traverse.Create(instance).Field<ZNetView>("m_nview").Value;
    public static ZNetView m_nview(TreeBase instance) => Traverse.Create(instance).Field<ZNetView>("m_nview").Value;
    public static ZNetView m_nview(Destructible instance) => Traverse.Create(instance).Field<ZNetView>("m_nview").Value;
    public static ZNetView m_nview(Pickable instance) => Traverse.Create(instance).Field<ZNetView>("m_nview").Value;
    public static ZNetView m_nview(CreatureSpawner instance) => Traverse.Create(instance).Field<ZNetView>("m_nview").Value;
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(SpawnArea), "GetInstances")]
    public static void SpawnArea_GetInstances(SpawnArea instance, out int near, out int total)
    {
      throw new NotImplementedException("Dummy");
    }

    [HarmonyReversePatch]
    [HarmonyPatch(typeof(CookingStation), "GetItemConversion")]
    public static CookingStation.ItemConversion CookingStation_GetItemConversion(CookingStation instance, string itemName)
    {
      throw new NotImplementedException("Dummy");
    }

    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Fermenter), "GetFermentationTime")]
    public static double Fermenter_GetFermentationTime(Fermenter instance)
    {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Plant), "GetGrowTime")]
    public static float Plant_GetGrowTime(Plant instance)
    {
      throw new System.NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Plant), "TimeSincePlanted")]
    public static double Plant_TimeSincePlanted(Plant instance)
    {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Smelter), "GetBakeTimer")]
    public static float Smelter_GetBakeTimer(Smelter instance)
    {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Smelter), "GetFuel")]
    public static float Smelter_GetFuel(Smelter instance)
    {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Character), "GetDamageModifiers")]
    public static HitData.DamageModifiers Character_GetDamageModifiers(Character instance)
    {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Tameable), "GetRemainingTime")]
    public static float Tameable_GetRemainingTime(Tameable instance)
    {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(HitData.DamageTypes), "DamageRange")]
    public static string DamageTypes_DamageRange(HitData.DamageTypes instance, float damage, float minFactor, float maxFactor)
    {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(RandEventSystem), "HaveGlobalKeys")]
    public static bool RandEventSystem_HaveGlobalKeys(RandEventSystem instance, RandomEvent ev)
    {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(RandEventSystem), "CheckBase")]
    public static bool RandEventSystem_CheckBase(RandEventSystem instance, RandomEvent ev, ZDO zdo)
    {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(EnvMan), "GetDayFraction")]
    public static float EnvMan_GetDayFraction(EnvMan instance)
    {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(EnvMan), "GetAvailableEnvironments")]
    public static List<EnvEntry> EnvMan_GetAvailableEnvironments(EnvMan instance, Heightmap.Biome biome)
    {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Player), "TakeInput")]
    public static bool Player_TakeInput(Player instance)
    {
      throw new NotImplementedException("Dummy");
    }
  }
}