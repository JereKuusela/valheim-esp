using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch]
  public class Patch
  {
    public static double GetElapsed(MonoBehaviour obj, string key, long defaultValue = 0)
    {
      var time = ZNet.instance.GetTime();
      var d = GetDateTime(obj, "alive_time", defaultValue);
      return (time - d).TotalSeconds;
    }
    public static DateTime GetDateTime(MonoBehaviour obj, string key, long defaultValue = 0) => new DateTime(GetLong(obj, key, defaultValue));
    public static float GetFloat(MonoBehaviour obj, string key, float defaultValue = 0) => m_nview(obj).GetZDO().GetFloat(key, defaultValue);
    public static long GetLong(MonoBehaviour obj, string key, long defaultValue = 0) => m_nview(obj).GetZDO().GetLong(key, defaultValue);
    public static int GetInt(MonoBehaviour obj, string key, int defaultValue = 0) => m_nview(obj).GetZDO().GetInt(key, defaultValue);
    public static bool GetBool(MonoBehaviour obj, string key, bool defaultValue = false) => m_nview(obj).GetZDO().GetBool(key, defaultValue);
    public static string GetString(MonoBehaviour obj, string key, string defaultValue = "") => m_nview(obj).GetZDO().GetString(key, defaultValue);
    public static GameObject GetPrefab(MonoBehaviour obj) => ZNetScene.instance.GetPrefab(m_nview(obj).GetZDO().GetPrefab());
    public static float m_cover(Windmill instance) => Traverse.Create(instance).Field<float>("m_cover").Value;
    public static Vector3 m_currentVel(Player instance) => Traverse.Create(instance).Field<Vector3>("m_currentVel").Value;
    public static ZNetView m_nview(MonoBehaviour obj) => Traverse.Create(obj).Field<ZNetView>("m_nview").Value;
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