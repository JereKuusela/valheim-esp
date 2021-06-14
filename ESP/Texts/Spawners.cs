using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ESP
{
  public partial class Texts
  {
    private static string GetTime(CreatureSpawner obj)
    {
      if (!obj.m_spawnAtDay)
        return "Only during " + Format.String("night");
      if (!obj.m_spawnAtNight)
        return "Only during " + Format.String("day");
      return "";
    }
    private static String GetRespawnTime(CreatureSpawner obj)
    {
      if (obj.m_respawnTimeMinuts == 0) return "Never";
      var elapsed = Patch.GetElapsed(obj, "alive_time");
      var elapsedString = elapsed == 0 ? "Alive" : elapsed.ToString("N0");
      return elapsedString + " / " + (60 * obj.m_respawnTimeMinuts).ToString("N0") + " seconds";
    }
    private static string GetRespawnTime(Pickable obj)
    {
      if (!obj.m_hideWhenPicked || obj.m_respawnTimeMinutes == 0) return "Never";
      var nView = Patch.m_nview(obj);
      var elapsed = Patch.GetElapsed(obj, "picked_time");
      var picked = Patch.GetBool(obj, "picked");
      var elapsedText = picked ? elapsed.ToString("N0") : "Not picked";
      return elapsedText + " / " + obj.m_respawnTimeMinutes.ToString("N0") + " minutes";
    }
    public static string Get(Pickable obj)
    {
      if (!obj || !Settings.showStructureStats) return "";
      var respawn = GetRespawnTime(obj);
      var text = "\nRespawn: " + Format.String(respawn);
      if (obj.m_amount > 0)
        text += "\nAmount: " + Format.String(obj.m_amount.ToString());
      return text;
    }

    public static string Get(CreatureSpawner obj)
    {
      if (!obj) return "";
      var respawn = GetRespawnTime(obj);
      var noise = obj.m_triggerNoise > 0 ? " with noise of " + Format.Int(obj.m_triggerNoise) : "";
      var lines = new List<string>();
      lines.Add(Format.Name(obj.m_creaturePrefab));
      lines.Add("Respawn: " + Format.String(respawn));
      lines.Add(Format.GetLevel(obj.m_minLevel, obj.m_maxLevel, obj.m_levelupChance));
      var timeText = GetTime(obj);
      if (timeText.Length > 0) lines.Add(timeText);
      if (obj.m_setPatrolSpawnPoint) lines.Add("Patrol point");
      lines.Add("Activates within " + Format.Int(obj.m_triggerDistance) + " meters" + noise);
      return string.Join("\n", lines);
    }

    private static string GetEventText()
    {
      var instance = RandEventSystem.instance;
      var timer = Patch.m_eventTime(instance);
      return Format.GetAttempt(timer, instance.m_eventIntervalMin * 60, instance.m_eventChance);
    }
    private static string GetEventsText()
    {
      var instance = RandEventSystem.instance;
      var zdo = ZDOMan.instance.GetZDO(Player.m_localPlayer.GetZDOID());
      var currentBiome = WorldGenerator.instance.GetBiome(Player.m_localPlayer.transform.position);
      var texts = instance.m_events.Where(randomEvent => randomEvent.m_enabled && randomEvent.m_random).Select(randomEvent =>
      {
        var validBiome = (currentBiome & randomEvent.m_biome) > 0;
        var validBase = Patch.RandEventSystem_CheckBase(instance, randomEvent, zdo);
        var validKeys = Patch.RandEventSystem_HaveGlobalKeys(instance, randomEvent);
        var valid = validBiome && validBase && validKeys;
        var parts = new List<string>(){
          Texts.GetNames(randomEvent.m_biome, currentBiome),
          Format.GetGlobalKeys(randomEvent.m_requiredGlobalKeys, randomEvent.m_notRequiredGlobalKeys),
          randomEvent.m_nearBaseOnly ? Format.String("player base", validBase) : ""
        };
        return Format.String(randomEvent.m_name, valid) + ": " + String.Join(", ", parts.Where(part => part.Length > 0));
      });
      return String.Join("\n", texts);
    }
    private static string GetSpawnerText(SpawnSystem obj, SpawnSystem.SpawnData spawnData, int stableHashCode)
    {
      var timeSinceSpawned = Patch.GetElapsed(obj, stableHashCode, 0);
      var text = "";
      var time = "";
      if (!spawnData.m_spawnAtDay)
      {
        time = ", only during " + Format.String("night");
      }
      if (!spawnData.m_spawnAtNight)
      {
        time = ", only during " + Format.String("day");
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

      var instances = SpawnSystem.GetNrOfInstances(spawnData.m_prefab, Vector3.zero, 0f, true, false);
      var progress = Format.ProgressPercent("Attempt", timeSinceSpawned, spawnData.m_spawnInterval);
      var chance = Format.Percent(spawnData.m_spawnChance / 100.0) + " chance";
      var weather = spawnData.m_requiredEnvironments.Count > 0 ? (", Weather: " + Format.String(string.Join(", ", spawnData.m_requiredEnvironments))) : "";
      var global = spawnData.m_requiredGlobalKey != "" ? (", Bosses: " + Format.String(spawnData.m_requiredGlobalKey)) : "";
      var spawns = Format.Progress(instances, spawnData.m_maxSpawned);
      var spawnDistance = Format.Int(spawnData.m_spawnDistance) + " meters";
      var level = Format.GetLevel(spawnData.m_minLevel, spawnData.m_maxLevel, obj.m_levelupChance, spawnData.m_levelUpMinCenterDistance);
      var group = Format.Range(spawnData.m_groupSizeMin, spawnData.m_groupSizeMax);
      var groupRadius = (spawnData.m_groupSizeMax > spawnData.m_groupSizeMin) ? " within " + Format.Int(spawnData.m_groupRadius) + " meters" : "";
      var altitude = Format.Range(spawnData.m_minAltitude, spawnData.m_maxAltitude);
      var offset = (spawnData.m_groundOffset > 0) ? ", " + Format.Int(spawnData.m_groundOffset) + " meters off ground" : "";
      var tilt = Format.Range(spawnData.m_minTilt, spawnData.m_maxTilt);
      var ocean = Format.Range(spawnData.m_minOceanDepth, spawnData.m_maxOceanDepth);
      var hunt = spawnData.m_huntPlayer ? ", forces hunt mode" : "";
      text += "\nCreature: " + Format.String(spawnData.m_prefab.name) + hunt;
      text += "\n" + progress + ", " + chance;
      var biomeString = Texts.GetBiomes(spawnData.m_biome, spawnData.m_biomeArea);
      if (biomeString.Length > 0)
        text += "\n" + biomeString + forest;
      text += "\nCreature limit: " + spawns + ", Distance limit: " + spawnDistance;
      text += "\n" + level + ", Group size: " + group + groupRadius;
      //text += "\nAltitude: " + altitude + offset + ", Tilt: " + tilt + ", Water: " + ocean;
      return text;
    }
    public static string GetRandomEvent(SpawnSystem obj)
    {
      var randEvent = RandEventSystem.instance;
      var text = GetEventText() + "\n\n";
      var num = 0;
      var currentEvent = randEvent.GetCurrentRandomEvent();
      if (currentEvent == null || randEvent.GetCurrentSpawners() == null)
        text += GetEventsText();
      else
      {
        text += Format.ProgressPercent(currentEvent.m_name, currentEvent.m_time, currentEvent.m_duration) + "\n";
        var spawners = randEvent.GetCurrentSpawners();
        var texts = spawners.Select(spawnData =>
        {
          num++;
          var stableHashCode = ("e_" + spawnData.m_prefab.name + num.ToString()).GetStableHashCode();
          return GetSpawnerText(obj, spawnData, stableHashCode);
        });
        text += String.Join("\n", texts);
      }
      return text;
    }

    private static string GetZoneText(SpawnSystem obj)
    {
      var position = obj.transform.position;
      var heightmap = Patch.m_heightmap(obj);
      var zone = ZoneSystem.instance.GetZone(position);
      var text = "Zone: " + Format.String(zone.x + ";" + zone.y);
      var biome = heightmap.GetBiome(obj.transform.position);
      var biomeArea = heightmap.GetBiomeArea();
      var biomeAreaString = ((biomeArea == Heightmap.BiomeArea.Median) ? ", full" : "");
      var area = heightmap.GetBiomeArea();
      text += " (" + Format.Name(biome) + biomeAreaString + ")";
      text += "\n";
      return text;
    }
    public static string Get(SpawnSystem obj, SpawnSystem.SpawnData spawnData, int stableHashCode)
    {
      var text = GetZoneText(obj);
      var timeSinceSpawned = Patch.GetElapsed(obj, stableHashCode, 0);
      var time = "";
      if (!spawnData.m_spawnAtDay)
      {
        time = ", only during " + Format.String("night");
      }
      if (!spawnData.m_spawnAtNight)
      {
        time = ", only during " + Format.String("day");
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
      var weather = spawnData.m_requiredEnvironments.Count > 0 ? (", Weather: " + Format.String(string.Join(", ", spawnData.m_requiredEnvironments))) : "";
      var global = spawnData.m_requiredGlobalKey != "" ? (", Bosses: " + Format.String(spawnData.m_requiredGlobalKey)) : "";
      var spawns = Format.Progress(instances, spawnData.m_maxSpawned);
      var spawnDistance = Format.Int(spawnData.m_spawnDistance) + " meters";
      var level = Format.GetLevel(spawnData.m_minLevel, spawnData.m_maxLevel, obj.m_levelupChance, spawnData.m_levelUpMinCenterDistance);
      var group = Format.Range(spawnData.m_groupSizeMin, spawnData.m_groupSizeMax);
      var groupRadius = (spawnData.m_groupSizeMax > spawnData.m_groupSizeMin) ? " within " + Format.Int(spawnData.m_groupRadius) + " meters" : "";
      var altitude = Format.Range(spawnData.m_minAltitude, spawnData.m_maxAltitude);
      var offset = (spawnData.m_groundOffset > 0) ? ", " + Format.Int(spawnData.m_groundOffset) + " meters off ground" : "";
      var tilt = Format.Range(spawnData.m_minTilt, spawnData.m_maxTilt);
      var ocean = Format.Range(spawnData.m_minOceanDepth, spawnData.m_maxOceanDepth);
      var hunt = spawnData.m_huntPlayer ? ", forces hunt mode" : "";
      text += "\nCreature: " + Format.String(spawnData.m_prefab.name) + hunt;
      text += "\n" + Format.GetAttempt(timeSinceSpawned, spawnData.m_spawnInterval, spawnData.m_spawnChance);
      var biomeString = Texts.GetBiomes(spawnData.m_biome, spawnData.m_biomeArea);
      text += "\n" + biomeString + forest + forest + weather + global + time;
      text += "\nCreature limit: " + spawns + ", Distance limit: " + spawnDistance;
      text += "\n" + level + ", Group size: " + group + groupRadius;
      text += "\nAltitude: " + altitude + offset + ", Tilt: " + tilt + ", Water: " + ocean;
      return text;
    }
  }
}

