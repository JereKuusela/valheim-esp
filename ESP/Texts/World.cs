using System;
using System.Collections.Generic;
using System.Linq;
using Service;
using UnityEngine;
namespace ESP;
public partial class Texts
{
  public static string Get(LocationProxy obj)
  {
    if (!Helper.IsValid(obj) || !Settings.Locations) return "";
    var name = Utils.GetPrefabName(obj.m_instance).ToLower();
    List<string> lines = new();
    var instances = ZoneSystem.instance.m_locationInstances;
    foreach (var l in ZoneSystem.instance.m_locations)
    {
      if (!l.m_enable) continue;
      if (l.m_prefabName.ToLower() != name) continue;
      var count = instances.Values.Where(instance => instance.m_location.m_prefabName == l.m_prefabName).Count();
      lines.Add("Placed: " + Format.Progress(count, l.m_quantity));
      if (l.m_group != "")
        lines.Add("Group: " + Format.String(l.m_group));
      lines.Add(GetBiomes(l.m_biome, l.m_biomeArea));
      if (l.m_maxDistance > 0)
        lines.Add("Distance from center: " + Format.Range(l.m_minDistance, l.m_maxDistance));
      if (l.m_minAltitude != l.m_maxAltitude)
        lines.Add("Altitude: " + Format.Range(l.m_minAltitude, l.m_maxAltitude));
      if (l.m_minTerrainDelta != l.m_maxTerrainDelta)
        lines.Add("Terrain angle: " + Format.Range(l.m_minTerrainDelta, l.m_maxTerrainDelta));
      if (l.m_inForest)
        lines.Add("Forest: " + Format.Range(l.m_forestTresholdMin, l.m_forestTresholdMax));
      if (l.m_minDistanceFromSimilar > 0)
        lines.Add("Distance from similar: " + Format.Meters(l.m_minDistanceFromSimilar));
      if (l.m_exteriorRadius > 0)
        lines.Add("Exterior radius: " + Format.Meters(l.m_location.m_exteriorRadius));
      if (l.m_location.m_hasInterior)
        lines.Add("Interior radius: " + Format.Meters(l.m_location.m_interiorRadius));
      List<string> flags = new();
      if (l.m_prioritized)
        flags.Add("Prioritized");
      if (l.m_centerFirst)
        flags.Add("Center first");
      if (l.m_randomRotation)
        flags.Add("Random rotation");
      if (l.m_unique)
        flags.Add("Unique");
      if (l.m_iconAlways)
        flags.Add("Always show icon");
      if (l.m_location.m_applyRandomDamage)
        flags.Add("Randomly damaged");
      if (l.m_location.m_clearArea)
        flags.Add("Clear area");
      if (l.m_location.m_noBuild)
        flags.Add("No build area");
      lines.Add(Format.JoinRow(flags));
    }
    return Format.JoinLines(lines);
  }

  public static string GetVegetation(GameObject obj)
  {
    if (!obj || !Settings.Vegetation) return "";
    var root = obj.transform.parent == null ? obj : obj.transform.parent.gameObject;
    var name = Utils.GetPrefabName(root).ToLower();
    List<string> lines = new();
    foreach (var v in ZoneSystem.instance.m_vegetation)
    {
      if (!v.m_enable) continue;
      if (Utils.GetPrefabName(v.m_prefab).ToLower() != name) continue;
      if (v.m_max < 1)
        lines.Add(Format.Percent(v.m_max) + " chance per zone");
      else
        lines.Add(Format.Range(v.m_min, v.m_max) + " per zone");
      lines[lines.Count() - 1] += " in " + GetBiomes(v.m_biome, v.m_biomeArea, false);
      if (v.m_groupSizeMax > 1)
        lines.Add("Group size: " + Format.Range(v.m_groupSizeMin, v.m_groupSizeMax) + " within " + Format.Meters(v.m_groupRadius));
      List<string> conditions = new();
      if (v.m_minAltitude != v.m_maxAltitude)
        conditions.Add("Altitude: " + Format.Range(v.m_minAltitude, v.m_maxAltitude));
      if (v.m_minOceanDepth != v.m_maxOceanDepth)
        conditions.Add("Ocean depth: " + Format.Range(v.m_minOceanDepth, v.m_maxOceanDepth));
      if (v.m_terrainDeltaRadius > 0)
        conditions.Add("Terrain delta: " + Format.Range(v.m_minTerrainDelta, v.m_maxTerrainDelta) + " within " + Format.Meters(v.m_terrainDeltaRadius));
      if (v.m_minTilt != v.m_maxTilt)
        conditions.Add("Terrain angle: " + Format.Range(v.m_minTilt, v.m_maxTilt));
      if (v.m_inForest)
        conditions.Add("Forest: " + Format.Range(v.m_forestTresholdMin, v.m_forestTresholdMax));
      lines.Add(Format.JoinRow(conditions));
      List<string> properties = new();
      if (v.m_snapToWater)
        properties.Add("Snaps to water");
      if (v.m_forcePlacement)
        properties.Add("Prioritized");
      if (v.m_blockCheck)
        properties.Add("Needs clear ground");
      if (v.m_groundOffset > 0)
        properties.Add(Format.Meters(v.m_groundOffset) + " from ground");
      if (v.m_chanceToUseGroundTilt >= 1)
        properties.Add("Ground tilt");
      else if (v.m_chanceToUseGroundTilt > 0)
        properties.Add(Format.Percent(v.m_chanceToUseGroundTilt) + " for ground tilt");
      if (v.m_randTilt > 0)
        properties.Add(Format.Float(v.m_randTilt) + " degrees random tilt");
      if (v.m_scaleMin != v.m_scaleMax)
        properties.Add("Random scale " + Format.Range(v.m_scaleMin, v.m_scaleMax));
      lines.Add(Format.JoinRow(properties));
    }
    if (lines.Count > 0) lines.Insert(0, " ");
    return Format.JoinLines(lines);
  }

  public static string Get(RandomSpawn obj)
  {
    if (!Helper.IsValid(obj)) return "";
    return "Chance to spawn: " + Format.Percent(obj.m_chanceToSpawn);

  }

  static int Respawn = "override_respawn".GetStableHashCode();
  static int SpawnTime = "spawn_time".GetStableHashCode();
  private static string GetItem(OfferingBowl obj)
  {
    if (obj.m_useItemStands) return "Item stands: " + obj.m_itemstandMaxRange + " m";
    return "Item: " + obj.m_bossItems + " of " + obj.m_bossItem.GetHoverName();
  }
  private static string GetRespawnTime(OfferingBowl obj)
  {
    var view = obj.GetComponentInParent<ZNetView>();
    var ret = "";
    DataUtils.Float(view, Respawn, respawn =>
    {
      DataUtils.Long(view, SpawnTime, spawnTime =>
      {
        var now = ZNet.instance.GetTime();
        var date = new DateTime(spawnTime);
        var elapsed = (now - date).TotalMinutes;
        ret = "Cooldown: " + Format.Int(elapsed) + " / " + Format.Int(respawn) + " minutes";
      });
    });
    return ret;
  }
  public static string Get(OfferingBowl obj)
  {
    if (!obj) return "";
    List<string> lines = new(){
        "Spawn: " + (obj.m_bossPrefab ? Utils.GetPrefabName(obj.m_bossPrefab) : ""),
        GetItem(obj),
        GetRespawnTime(obj),
        "Area: " + Format.Int(obj.m_spawnBossMaxDistance) + " m"};
    return Format.JoinLines(lines);
  }

  public static string Get(SpawnArea obj)
  {
    if (!obj) return "";
    int near, total;
    obj.GetInstances(out near, out total);
    List<string> lines = new(){
        Format.ProgressPercent("Timer", obj.m_spawnTimer, obj.m_spawnIntervalSec),
        "Area: " + Format.Int(obj.m_spawnRadius) + " m",
        "Level up: " + Format.Percent(obj.m_levelupChance / 100f),
        "Trigger: " + Format.Int(obj.m_triggerDistance) + " m",
        "Near limit: " + Format.Progress(near, obj.m_maxNear) + " within " + Format.Int(obj.m_nearRadius) + " m",
        "Far limit: " +  Format.Progress(total, obj.m_maxTotal) + " within " + Format.Int(obj.m_farRadius) + " m"
      };
    if (obj.m_onGroundOnly)
    {
      lines.Add("Only on ground");
    }

    lines.Add(" ");
    var totalWeight = 0f;
    foreach (var data in obj.m_prefabs)
      totalWeight += data.m_weight;
    foreach (var data in obj.m_prefabs)
    {
      var level = Format.Range(data.m_minLevel, data.m_maxLevel);
      lines.Add(data.m_prefab.name + ": " + Format.Percent(data.m_weight / totalWeight) + " with level " + level);
    }
    return Format.JoinLines(lines);
  }
}
