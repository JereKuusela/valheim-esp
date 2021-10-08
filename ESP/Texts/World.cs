using System.Collections.Generic;
using System.Linq;
using Text;
using UnityEngine;

namespace ESP {
  public partial class Texts {
    public static string Get(Location obj) {
      if (!IsValid(obj) || !Settings.Locations) return "";
      var name = Utils.GetPrefabName(obj.gameObject).ToLower();
      var lines = new List<string>();
      var instances = ZoneSystem.instance.m_locationInstances;
      foreach (var l in ZoneSystem.instance.m_locations) {
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
        if (l.m_interiorRadius > 0)
          lines.Add("Interior radius: " + Format.Meters(l.m_interiorRadius));
        var flags = new List<string>();
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
        lines.Add(Format.JoinRow(flags));
      }
      return Format.JoinLines(lines);
    }

    public static string GetVegetation(GameObject obj) {
      if (!obj || !Settings.Vegetation) return "";
      var root = obj.transform.parent == null ? obj : obj.transform.parent.gameObject;
      var name = Utils.GetPrefabName(root).ToLower();
      var lines = new List<string>();
      foreach (var v in ZoneSystem.instance.m_vegetation) {
        if (!v.m_enable) continue;
        if (Utils.GetPrefabName(v.m_prefab).ToLower() != name) continue;
        if (v.m_max < 1)
          lines.Add(Format.Percent(v.m_max) + " chance per zone");
        else
          lines.Add(Format.Range(v.m_min, v.m_max) + " per zone");
        lines[lines.Count() - 1] += " in " + GetBiomes(v.m_biome, v.m_biomeArea, false);
        if (v.m_groupSizeMax > 1)
          lines.Add("Group size: " + Format.Range(v.m_groupSizeMin, v.m_groupSizeMax) + " within " + Format.Meters(v.m_groupRadius));
        var conditions = new List<string>();
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
        var properties = new List<string>();
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

    public static string Get(RandomSpawn obj) {
      if (!IsValid(obj)) return "";
      return "Chance to spawn: " + Format.Percent(obj.m_chanceToSpawn);

    }
  }
}