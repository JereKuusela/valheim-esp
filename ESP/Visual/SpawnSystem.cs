using HarmonyLib;
using Service;
using UnityEngine;
using Visualization;

namespace ESP {

  [HarmonyPatch(typeof(SpawnSystem), "Awake")]
  public class SpawnSystem_Awake {
    private static void DrawBiomes(SpawnSystem obj) {
      if (Settings.BiomeCornerRayWidth == 0)
        return;
      var heightmap = obj.m_heightmap;
      var num = ZoneSystem.instance.m_zoneSize * 0.5f;
      var pos1 = new Vector3(num, 0f, num);
      var pos2 = new Vector3(-num, 0f, num);
      var pos3 = new Vector3(num, 0f, -num);
      var pos4 = new Vector3(-num, 0f, -num);
      var biome1 = heightmap.GetBiome(obj.transform.position + pos1);
      var biome2 = heightmap.GetBiome(obj.transform.position + pos2);
      var biome3 = heightmap.GetBiome(obj.transform.position + pos3);
      var biome4 = heightmap.GetBiome(obj.transform.position + pos4);
      DrawMarker(obj, pos1, biome1);
      DrawMarker(obj, pos2, biome2);
      DrawMarker(obj, pos3, biome3);
      DrawMarker(obj, pos4, biome4);
    }
    private static void DrawMarker(MonoBehaviour parent, Vector3 position, Heightmap.Biome biome) {
      var obj = Draw.DrawMarkerLine(Tag.ZoneCorner, parent, Texts.GetColor(biome), Settings.BiomeCornerRayWidth, position);
      var text = obj.AddComponent<BiomeText>();
      text.biome = biome;
    }
    private static int GetTotalAmountOfSpawnSystems(SpawnSystem instance, Heightmap heightmap) {
      var totalAmount = 0;
      foreach (SpawnSystem.SpawnData spawnData in instance.m_spawners) {
        if (!spawnData.m_enabled || !heightmap.HaveBiome(spawnData.m_biome)) continue;
        if (!spawnData.m_spawnAtDay && !spawnData.m_spawnAtNight) continue;
        totalAmount++;
      }
      return totalAmount;
    }
    private static bool IsEnabled(SpawnSystem.SpawnData instance) {
      if (Settings.SpawnSystemRayWidth == 0) return false;
      return !LocationUtils.IsIn(Settings.ExcludedSpawnSystems, Utils.GetPrefabName(instance.m_prefab));
    }
    private static void DrawSpawnSystems(SpawnSystem obj) {
      if (Settings.SpawnSystemRayWidth == 0) return;
      var heightmap = obj.m_heightmap;
      var totalAmount = GetTotalAmountOfSpawnSystems(obj, heightmap);
      var counter = -totalAmount / 2;
      var num = 0;
      var biome = heightmap.GetBiome(obj.transform.position);
      obj.m_spawners.ForEach(spawnData => {
        num++;
        if (!spawnData.m_enabled || !heightmap.HaveBiome(spawnData.m_biome)) return;
        if (!spawnData.m_spawnAtDay && !spawnData.m_spawnAtNight) return;
        if (!IsEnabled(spawnData)) return;
        var stableHashCode = ("b_" + spawnData.m_prefab.name + num.ToString()).GetStableHashCode();
        var position = new Vector3(counter * 2 * Settings.SpawnSystemRayWidth, 0, 0);
        var line = Draw.DrawMarkerLine(Tag.SpawnZone, obj, Texts.GetColor(biome), Settings.SpawnSystemRayWidth, position);
        var text = line.AddComponent<SpawnSystemText>();
        text.spawnSystem = obj;
        text.spawnData = spawnData;
        text.stableHashCode = stableHashCode;
        counter++;
      });
    }
    private static void DrawRandEventSystem(SpawnSystem instance) {
      if (Settings.RandEventSystemRayWidth == 0) return;
      var obj = Draw.DrawMarkerLine(Tag.RandomEventSystem, instance, Settings.RandomEventSystemRayColor, Settings.RandEventSystemRayWidth, new Vector3(0, 0, 5));
      obj.AddComponent<RandEventSystemText>().spawnSystem = instance;
    }
    public static void Postfix(SpawnSystem __instance) {
      DrawBiomes(__instance);
      DrawSpawnSystems(__instance);
      DrawRandEventSystem(__instance);
    }
  }

  public class RandEventSystemText : MonoBehaviour, Hoverable {
    public string GetHoverText() => Texts.GetRandomEvent(spawnSystem);
    public string GetHoverName() => "Random events";
    public SpawnSystem spawnSystem;
  }
  public class SpawnSystemText : MonoBehaviour, Hoverable {
    public string GetHoverText() => Texts.Get(spawnSystem, spawnData, stableHashCode);
    public string GetHoverName() => spawnData.m_name.Length > 0 ? spawnData.m_name : spawnData.m_prefab.name;
    public SpawnSystem spawnSystem;
    public SpawnSystem.SpawnData spawnData;
    public int stableHashCode;
  }

  public class BiomeText : MonoBehaviour, Hoverable {
    public string GetHoverText() => Texts.Get(biome);
    public string GetHoverName() => Translate.Name(biome);
    public Heightmap.Biome biome;
  }
}
