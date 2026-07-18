using HarmonyLib;
using Service;
using UnityEngine;
using Visualization;
namespace ESP;

[HarmonyPatch(typeof(SpawnSystem), nameof(SpawnSystem.Awake)), HarmonyPriority(Priority.Last)]
public class SpawnSystem_Awake
{
  public static void RebuildLoaded()
  {
    foreach (var obj in SceneObjects.FindLoaded<SpawnSystem>())
    {
      if (!obj.m_heightmap) continue;
      ClearSpawnSystemVisuals(obj);
      Postfix(obj);
    }
  }
  private static bool IsSpawnSystemTag(string tag)
  {
    return tag == Tag.RandomEventSystem || tag == Tag.SpawnZone;
  }
  private static void ClearSpawnSystemVisuals(SpawnSystem obj)
  {
    var visuals = obj.GetComponentsInChildren<Visualization.Visualization>(true);
    foreach (var visual in visuals)
    {
      if (!visual || !IsSpawnSystemTag(visual.Tag)) continue;
      Object.Destroy(visual.gameObject);
    }
  }
  private static int GetTotalAmountOfSpawnSystems(SpawnSystem instance, Heightmap heightmap)
  {
    var totalAmount = 0;
    foreach (var list in instance.m_spawnLists)
    {
      foreach (var spawnData in list.m_spawners)
      {
        if (!spawnData.m_enabled || !heightmap.HaveBiome(spawnData.m_biome)) continue;
        if (!spawnData.m_spawnAtDay && !spawnData.m_spawnAtNight) continue;
        totalAmount++;
      }
    }

    return totalAmount;
  }
  private static bool IsEnabled(SpawnSystem.SpawnData instance)
  {
    return !LocationUtils.IsIn(Settings.ExcludedSpawnZones, Utils.GetPrefabName(instance.m_prefab));
  }
  private static void DrawSpawnSystems(SpawnSystem obj)
  {
    if (Settings.IsDisabled(Tag.SpawnZone)) return;
    var heightmap = obj.m_heightmap;
    var totalAmount = GetTotalAmountOfSpawnSystems(obj, heightmap);
    var counter = -totalAmount / 2;
    var biome = heightmap.GetBiome(obj.transform.position);
    var subTag = Tag.GetSpawnZone(biome);
    obj.m_spawnLists.ForEach(list =>
    {
      var num = 0;
      list.m_spawners.ForEach(spawnData =>
      {
        num++;
        if (!spawnData.m_enabled || !heightmap.HaveBiome(spawnData.m_biome)) return;
        if (!spawnData.m_spawnAtDay && !spawnData.m_spawnAtNight) return;
        if (!IsEnabled(spawnData)) return;
        var stableHashCode = ("b_" + spawnData.m_prefab.name + num.ToString()).GetStableHashCode();
        Vector3 position = new(counter * 3f * Settings.configSpawnZoneRayWidth.Value / 100f, 0, 0);
        var line = Draw.DrawMarkerLine(Tag.SpawnZone, subTag, obj, position);
        var text = line.AddComponent<SpawnSystemText>();
        text.spawnSystem = obj;
        text.spawnData = spawnData;
        text.stableHashCode = stableHashCode;
        counter++;
      });
    });
  }
  private static void DrawRandEventSystem(SpawnSystem instance)
  {
    if (Settings.IsDisabled(Tag.RandomEventSystem)) return;
    if (Draw.HasVisual(instance, Tag.RandomEventSystem)) return;
    var obj = Draw.DrawMarkerLine(Tag.RandomEventSystem, instance, new(0, 0, 5));
    obj.AddComponent<RandEventSystemText>().spawnSystem = instance;
  }
  static void Postfix(SpawnSystem __instance)
  {
    // Jewelcrafting spawns this object without the terrain loaded.
    if (!__instance.m_heightmap) return;
    DrawSpawnSystems(__instance);
    DrawRandEventSystem(__instance);
  }
}

public class RandEventSystemText : MonoBehaviour, Hoverable
{
  public string GetHoverText() => spawnSystem != null ? Texts.GetRandomEvent(spawnSystem) : "";
  public string GetHoverName() => "Random events";
  public SpawnSystem? spawnSystem;
}
public class SpawnSystemText : MonoBehaviour, Hoverable
{
  public string GetHoverText() => spawnSystem == null || spawnData == null ? "" : Texts.Get(spawnSystem, spawnData, stableHashCode);
  public string GetHoverName() => spawnData == null ? "" : spawnData.m_name.Length > 0 ? spawnData.m_name : spawnData.m_prefab.name;
  public SpawnSystem? spawnSystem;
  public SpawnSystem.SpawnData? spawnData;
  public int stableHashCode;
}

public class BiomeText : MonoBehaviour, Hoverable
{
  public string GetHoverText() => Texts.Get(biome);
  public string GetHoverName() => Translate.Name(biome);
  public Heightmap.Biome biome;
}
