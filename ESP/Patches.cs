using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace ESP {
  [HarmonyPatch]
  public class Patch {
    private static T Get<T>(object obj, string field) => Traverse.Create(obj).Field<T>(field).Value;
    private static object Get(object obj, string field) => Traverse.Create(obj).Field<object>(field).Value;
    public static double GetElapsed(MonoBehaviour obj, string key, long defaultValue = 0) {
      var time = ZNet.instance.GetTime();
      var d = GetDateTime(obj, key, defaultValue);
      return (time - d).TotalSeconds;
    }
    public static double GetElapsed(MonoBehaviour obj, int key, long defaultValue = 0) {
      var time = ZNet.instance.GetTime();
      var d = GetDateTime(obj, key, defaultValue);
      return (time - d).TotalSeconds;
    }
    public static DateTime GetDateTime(MonoBehaviour obj, string key, long defaultValue = 0) => new DateTime(GetLong(obj, key, defaultValue));
    public static DateTime GetDateTime(MonoBehaviour obj, int key, long defaultValue = 0) => new DateTime(GetLong(obj, key, defaultValue));
    public static float GetFloat(MonoBehaviour obj, string key, float defaultValue = 0) => Nview(obj).GetZDO().GetFloat(key, defaultValue);
    public static long GetLong(MonoBehaviour obj, string key, long defaultValue = 0) => Nview(obj).GetZDO().GetLong(key, defaultValue);
    public static long GetLong(MonoBehaviour obj, int key, long defaultValue = 0) => Nview(obj).GetZDO().GetLong(key, defaultValue);
    public static int GetInt(MonoBehaviour obj, string key, int defaultValue = 0) => Nview(obj).GetZDO().GetInt(key, defaultValue);
    public static bool GetBool(MonoBehaviour obj, string key, bool defaultValue = false) => Nview(obj).GetZDO().GetBool(key, defaultValue);
    public static string GetString(MonoBehaviour obj, string key, string defaultValue = "") => Nview(obj).GetZDO().GetString(key, defaultValue);
    public static GameObject GetPrefab(MonoBehaviour obj) => ZNetScene.instance.GetPrefab(Nview(obj).GetZDO().GetPrefab());
    public static float Cover(Windmill obj) => Get<float>(obj, "m_cover");
    public static float Health(object obj) => Get<float>(obj, "m_health");
    public static float SpawnTimer(SpawnArea obj) => Get<float>(obj, "m_spawnTimer");
    public static float NoiseRange(Character obj) => Get<float>(obj, "m_noiseRange");
    public static float StaggerDamage(Character obj) => Get<float>(obj, "m_staggerDamage");
    public static float ConsumeSearchTimer(MonsterAI obj) => Get<float>(obj, "m_consumeSearchTimer");
    public static string AiStatus(MonsterAI obj) => Get<string>(obj, "m_aiStatus");
    public static float EventTimer(RandEventSystem obj) => Get<float>(obj, "m_eventTimer");
    public static int SolidRayMask(Fireplace obj) => Get<int>(obj, "m_solidRayMask");
    public static Heightmap Heightmap(SpawnSystem obj) => Get<Heightmap>(obj, "m_heightmap");
    public static float SpawnDistanceMin(SpawnSystem obj) => Get<float>(obj, "m_spawnDistanceMin");
    public static float SpawnDistanceMax(SpawnSystem obj) => Get<float>(obj, "m_spawnDistanceMax");
    public static Rigidbody Body(Character obj) => Get<Rigidbody>(obj, "m_body");
    public static Rigidbody Body(Ship obj) => Get<Rigidbody>(obj, "m_body");
    public static Rigidbody Body(Smoke obj) => Get<Rigidbody>(obj, "m_body");
    public static float Time(Smoke obj) => Get<float>(obj, "m_time");
    public static Vector3 CurrentVel(Player obj) => Get<Vector3>(obj, "m_currentVel");
    public static SEMan Seman(Player obj) => Get<SEMan>(obj, "m_seman");
    public static ZNetView Nview(MonoBehaviour obj) => Get<ZNetView>(obj, "m_nview");
    public static Vector3[] CoverRays(Cover obj) => Get<Vector3[]>(obj, "m_coverRays");
    public static List<Collider> HitAreas(MineRock obj) => Get<List<Collider>>(obj, "m_hitAreas");
    public static IEnumerable<object> HitAreas(MineRock5 obj) => Get<IEnumerable<object>>(obj, "m_hitAreas");
    public static int RayMask(MineRock5 obj) => Get<int>(obj, "m_rayMask");
    public static int CoverRayMask(Cover obj) => Get<int>(obj, "m_coverRayMask");
    public static float UpdateExtensionTimer(CraftingStation obj) => Get<float>(obj, "m_updateExtensionTimer");
    public static List<string> BindList(Terminal obj) => Get<List<string>>(obj, "m_bindList");
    public static List<ItemDrop> Instances(ItemDrop obj) => Get<List<ItemDrop>>(obj, "m_instances");

    [HarmonyReversePatch]
    [HarmonyPatch(typeof(SpawnArea), "GetInstances")]
    public static void SpawnArea_GetInstances(SpawnArea instance, out int near, out int total) {
      throw new NotImplementedException("Dummy");
    }

    [HarmonyReversePatch]
    [HarmonyPatch(typeof(CookingStation), "GetItemConversion")]
    public static CookingStation.ItemConversion CookingStation_GetItemConversion(CookingStation instance, string itemName) {
      throw new NotImplementedException("Dummy");
    }

    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Fermenter), "GetFermentationTime")]
    public static double Fermenter_GetFermentationTime(Fermenter instance) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Plant), "GetGrowTime")]
    public static float Plant_GetGrowTime(Plant instance) {
      throw new System.NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Plant), "TimeSincePlanted")]
    public static double Plant_TimeSincePlanted(Plant instance) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Smelter), "GetBakeTimer")]
    public static float Smelter_GetBakeTimer(Smelter instance) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Smelter), "GetFuel")]
    public static float Smelter_GetFuel(Smelter instance) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Tameable), "GetRemainingTime")]
    public static float Tameable_GetRemainingTime(Tameable instance) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(HitData.DamageTypes), "DamageRange")]
    public static string DamageTypes_DamageRange(HitData.DamageTypes instance, float damage, float minFactor, float maxFactor) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(RandEventSystem), "HaveGlobalKeys")]
    public static bool RandEventSystem_HaveGlobalKeys(RandEventSystem instance, RandomEvent ev) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(RandEventSystem), "CheckBase")]
    public static bool RandEventSystem_CheckBase(RandEventSystem instance, RandomEvent ev, ZDO zdo) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(EnvMan), "GetDayFraction")]
    public static float EnvMan_GetDayFraction(EnvMan instance) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(EnvMan), "GetAvailableEnvironments")]
    public static List<EnvEntry> EnvMan_GetAvailableEnvironments(EnvMan instance, Heightmap.Biome biome) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Player), "TakeInput")]
    public static bool Player_TakeInput(Player instance) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(WearNTear), "GetMaterialProperties")]
    public static void WearNTear_GetMaterialProperties(WearNTear instance, out float maxSupport, out float minSupport, out float horizontalLoss, out float verticalLoss) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(WearNTear), "ResetHighlight")]
    public static void WearNTear_ResetHighlight(WearNTear instance) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(ItemDrop), "GetTimeSinceSpawned")]
    public static double ItemDrop_GetTimeSinceSpawned(ItemDrop instance) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(ItemDrop), "IsInsideBase")]
    public static bool ItemDrop_IsInsideBase(ItemDrop instance) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Character), "GetDamageModifiers")]
    public static HitData.DamageModifiers Character_GetDamageModifiers(Character instance) {
      throw new NotImplementedException("Dummy");
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(EnvMan), "AddWindOctave")]
    public static void EnvMan_AddWindOctave(EnvMan instance, long timeSec, int octave, ref float angle, ref float intensity) {
      throw new NotImplementedException("Dummy");
    }
  }
}