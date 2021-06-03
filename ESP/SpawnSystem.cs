using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{

  [HarmonyPatch(typeof(SpawnSystem), "Awake")]
  public class SpawnSystem_Awake
  {
    private static void DrawBiomes(SpawnSystem instance, Heightmap heightmap)
    {
      if (!Settings.showBiomes)
        return;
      var num = ZoneSystem.instance.m_zoneSize * 0.5f;
      var pos1 = new Vector3(num, 0f, num);
      var pos2 = new Vector3(-num, 0f, num);
      var pos3 = new Vector3(num, 0f, -num);
      var pos4 = new Vector3(-num, 0f, -num);
      var biome1 = heightmap.GetBiome(instance.transform.position + pos1);
      var biome2 = heightmap.GetBiome(instance.transform.position + pos2);
      var biome3 = heightmap.GetBiome(instance.transform.position + pos3);
      var biome4 = heightmap.GetBiome(instance.transform.position + pos4);
      DrawMarker(instance.gameObject, pos1, biome1);
      DrawMarker(instance.gameObject, pos2, biome2);
      DrawMarker(instance.gameObject, pos3, biome3);
      DrawMarker(instance.gameObject, pos4, biome4);
    }
    private static void DrawMarker(GameObject parent, Vector3 position, Heightmap.Biome biome)
    {
      Drawer.DrawMarkerLine(parent, position, BiomeUtils.GetColor(biome), 0.25f, BiomeUtils.GetName(biome));
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
      if (!Settings.showSpawnSystems) return false;
      var name = Utils.GetPrefabName(instance.m_prefab).ToLower();
      var excluded = Settings.excludedSpawnSystems.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return false;
      return true;
    }
    private static void DrawSpawnSystems(SpawnSystem instance, Heightmap heightmap, ZNetView nview)
    {
      if (!Settings.showSpawnSystems) return;
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
        Action<GameObject> action = (GameObject obj) =>
        {
          var text = obj.AddComponent<SpawnSystemText>();
          text.spawnSystem = instance;
          text.heightmap = heightmap;
          text.nview = nview;
          text.spawnData = spawnData;
          text.stableHashCode = stableHashCode;
        };
        Drawer.DrawMarkerLine(instance.gameObject, new Vector3(counter * 2, 0, 0), BiomeUtils.GetColor(biome), 1f, action);
        counter++;
      });
    }
    private static void DrawRandEventSystem(SpawnSystem instance, Heightmap heightmap, ZNetView nview)
    {
      if (!Settings.showRandEventSystem) return;
      Action<GameObject> action = (GameObject obj) =>
        {
          var text = obj.AddComponent<RandEventSystemText>();
          text.spawnSystem = instance;
          text.heightmap = heightmap;
          text.nview = nview;
        };
      Drawer.DrawMarkerLine(instance.gameObject, new Vector3(0, 0, 5), Color.black, 1f, action);
    }
    public static void Postfix(SpawnSystem __instance, Heightmap ___m_heightmap, ZNetView ___m_nview)
    {
      DrawBiomes(__instance, ___m_heightmap);
      DrawSpawnSystems(__instance, ___m_heightmap, ___m_nview);
      DrawRandEventSystem(__instance, ___m_heightmap, ___m_nview);
    }
  }


  public class SpawnSystemText : MonoBehaviour, Hoverable
  {
    private string GetZoneText()
    {
      var position = spawnSystem.transform.position;
      var zone = ZoneSystem.instance.GetZone(position);
      var text = "Zone: " + TextUtils.String(zone.x + ";" + zone.y);
      var biome = heightmap.GetBiome(spawnSystem.transform.position);
      var biomeArea = heightmap.GetBiomeArea();
      var biomeAreaString = ((biomeArea == Heightmap.BiomeArea.Median) ? ", full" : "");
      var area = heightmap.GetBiomeArea();
      text += " (" + TextUtils.String(BiomeUtils.GetName(biome)) + biomeAreaString + ")";
      text += "\n";
      return text;
    }
    private double GetTimeSinceSpawned()
    {
      var time = ZNet.instance.GetTime();
      DateTime d = new DateTime(nview.GetZDO().GetLong(stableHashCode, 0L));
      return (time - d).TotalSeconds;
    }
    public string GetHoverText()
    {
      var text = GetZoneText();
      var timeSinceSpawned = GetTimeSinceSpawned();
      var time = "";
      if (!spawnData.m_spawnAtDay)
      {
        time = ", only during " + TextUtils.String("night");
      }
      if (!spawnData.m_spawnAtNight)
      {
        time = ", only during " + TextUtils.String("day");
      }
      var forest = "";
      if (!spawnData.m_inForest)
      {
        forest = ", only outside forests";
      }
      if (!spawnData.m_outsideForest)
      {
        forest = ", only inside forests";
      }

      var instances = SpawnSystem.GetNrOfInstances(spawnData.m_prefab, Vector3.zero, 0f, false, false);
      var weather = spawnData.m_requiredEnvironments.Count > 0 ? (", Weather: " + TextUtils.String(spawnData.m_requiredEnvironments.Join(null, ", "))) : "";
      var global = spawnData.m_requiredGlobalKey != "" ? (", Bosses: " + TextUtils.String(spawnData.m_requiredGlobalKey)) : "";
      var spawns = TextUtils.Progress(instances, spawnData.m_maxSpawned);
      var spawnDistance = TextUtils.Int(spawnData.m_spawnDistance) + " meters";
      var level = TextUtils.GetLevel(spawnData.m_minLevel, spawnData.m_maxLevel, spawnSystem.m_levelupChance, spawnData.m_levelUpMinCenterDistance);
      var group = TextUtils.Range(spawnData.m_groupSizeMin, spawnData.m_groupSizeMax);
      var groupRadius = (spawnData.m_groupSizeMax > spawnData.m_groupSizeMin) ? " within " + TextUtils.Int(spawnData.m_groupRadius) + " meters" : "";
      var altitude = TextUtils.Range(spawnData.m_minAltitude, spawnData.m_maxAltitude);
      var offset = (spawnData.m_groundOffset > 0) ? ", " + TextUtils.Int(spawnData.m_groundOffset) + " meters off ground" : "";
      var tilt = TextUtils.Range(spawnData.m_minTilt, spawnData.m_maxTilt);
      var ocean = TextUtils.Range(spawnData.m_minOceanDepth, spawnData.m_maxOceanDepth);
      var hunt = spawnData.m_huntPlayer ? ", forces hunt mode" : "";
      text += "\nCreature: " + TextUtils.String(spawnData.m_prefab.name) + hunt;
      text += "\n" + TextUtils.GetAttempt(timeSinceSpawned, spawnData.m_spawnInterval, spawnData.m_spawnChance);
      var biomeString = TextUtils.GetBiomes(spawnData.m_biome, spawnData.m_biomeArea);
      text += "\n" + biomeString + forest + forest + weather + global + time;
      text += "\nCreature limit: " + spawns + ", Distance limit: " + spawnDistance;
      text += "\n" + level + ", Group size: " + group + groupRadius;
      text += "\nAltitude: " + altitude + offset + ", Tilt: " + tilt + ", Water: " + ocean;
      return text;
    }
    public string GetHoverName() => spawnData.m_name.Length > 0 ? spawnData.m_name : spawnData.m_prefab.name;
    public SpawnSystem spawnSystem;
    public Heightmap heightmap;
    public ZNetView nview;
    public SpawnSystem.SpawnData spawnData;
    public int stableHashCode;
  }
}