using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{

  [HarmonyPatch(typeof(SpawnSystem), "Awake")]
  public class SpawnSystem_Awake
  {
    private static void DrawBiomes(SpawnSystem instance)
    {
      if (Settings.biomeCornerRayWidth == 0)
        return;
      var heightmap = Patch.m_heightmap(instance);
      var num = ZoneSystem.instance.m_zoneSize * 0.5f;
      var pos1 = new Vector3(num, 0f, num);
      var pos2 = new Vector3(-num, 0f, num);
      var pos3 = new Vector3(num, 0f, -num);
      var pos4 = new Vector3(-num, 0f, -num);
      var biome1 = heightmap.GetBiome(instance.transform.position + pos1);
      var biome2 = heightmap.GetBiome(instance.transform.position + pos2);
      var biome3 = heightmap.GetBiome(instance.transform.position + pos3);
      var biome4 = heightmap.GetBiome(instance.transform.position + pos4);
      DrawMarker(instance, pos1, biome1);
      DrawMarker(instance, pos2, biome2);
      DrawMarker(instance, pos3, biome3);
      DrawMarker(instance, pos4, biome4);
    }
    private static void DrawMarker(MonoBehaviour parent, Vector3 position, Heightmap.Biome biome)
    {
      var obj = Drawer.DrawMarkerLine(parent, Texts.GetColor(biome), Settings.biomeCornerRayWidth, Drawer.ZONE, position);
      var text = obj.AddComponent<BiomeText>();
      text.biome = biome;
    }
    private static int GetTotalAmountOfSpawnSystems(SpawnSystem instance, Heightmap heightmap)
    {
      var totalAmount = 0;
      foreach (SpawnSystem.SpawnData spawnData in instance.m_spawners)
      {
        if (!spawnData.m_enabled || !heightmap.HaveBiome(spawnData.m_biome)) continue;
        if (!spawnData.m_spawnAtDay && !spawnData.m_spawnAtNight) continue;
        totalAmount++;
      }
      return totalAmount;
    }
    private static bool IsEnabled(SpawnSystem.SpawnData instance)
    {
      if (Settings.spawnSystemRayWidth == 0) return false;
      var name = Utils.GetPrefabName(instance.m_prefab).ToLower();
      var excluded = Settings.excludedSpawnSystems.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return false;
      return true;
    }
    private static void DrawSpawnSystems(SpawnSystem instance)
    {
      if (Settings.spawnSystemRayWidth == 0) return;
      var heightmap = Patch.m_heightmap(instance);
      var totalAmount = GetTotalAmountOfSpawnSystems(instance, heightmap);
      var counter = -totalAmount / 2;
      var num = 0;
      var biome = heightmap.GetBiome(instance.transform.position);
      instance.m_spawners.ForEach(spawnData =>
      {
        num++;
        if (!spawnData.m_enabled || !heightmap.HaveBiome(spawnData.m_biome)) return;
        if (!spawnData.m_spawnAtDay && !spawnData.m_spawnAtNight) return;
        if (!IsEnabled(spawnData)) return;
        var stableHashCode = ("b_" + spawnData.m_prefab.name + num.ToString()).GetStableHashCode();
        var position = new Vector3(counter * 2 * Settings.spawnSystemRayWidth, 0, 0);
        var obj = Drawer.DrawMarkerLine(instance, Texts.GetColor(biome), Settings.spawnSystemRayWidth, Drawer.ZONE, position);
        var text = obj.AddComponent<SpawnSystemText>();
        text.spawnSystem = instance;
        text.spawnData = spawnData;
        text.stableHashCode = stableHashCode;
        counter++;
      });
    }
    private static void DrawRandEventSystem(SpawnSystem instance)
    {
      if (Settings.randEventSystemRayWidth == 0) return;
      var obj = Drawer.DrawMarkerLine(instance, Color.black, Settings.randEventSystemRayWidth, Drawer.ZONE, new Vector3(0, 0, 5));
      obj.AddComponent<RandEventSystemText>().spawnSystem = instance;
    }
    public static void Postfix(SpawnSystem __instance)
    {
      DrawBiomes(__instance);
      DrawSpawnSystems(__instance);
      DrawRandEventSystem(__instance);
    }
  }

  public class RandEventSystemText : MonoBehaviour, Hoverable
  {
    public string GetHoverText() => Texts.GetRandomEvent(spawnSystem);
    public string GetHoverName() => "Random events";
    public SpawnSystem spawnSystem;
  }
  public class SpawnSystemText : MonoBehaviour, Hoverable
  {
    public string GetHoverText() => Texts.Get(spawnSystem, spawnData, stableHashCode);
    public string GetHoverName() => spawnData.m_name.Length > 0 ? spawnData.m_name : spawnData.m_prefab.name;
    public SpawnSystem spawnSystem;
    public SpawnSystem.SpawnData spawnData;
    public int stableHashCode;
  }

  public class BiomeText : MonoBehaviour, Hoverable
  {
    public string GetHoverText() => Texts.Get(biome);
    public string GetHoverName() => Texts.GetName(biome);
    public Heightmap.Biome biome;
  }
}
