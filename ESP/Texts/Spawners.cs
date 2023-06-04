using System;
using System.Collections.Generic;
using System.Linq;
using Service;
namespace ESP;
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
    var elapsed = Helper.GetElapsed(obj, "alive_time");
    var elapsedString = elapsed == 0 ? "Alive" : elapsed.ToString("N0");
    return elapsedString + " / " + (60 * obj.m_respawnTimeMinuts).ToString("N0") + " seconds";
  }
  private static string GetRespawnTime(Pickable obj)
  {
    if (obj.m_respawnTimeMinutes == 0) return "Never";
    var elapsed = Helper.GetElapsed(obj, "picked_time") / 60;
    var picked = Helper.GetBool(obj, "picked");
    var elapsedText = picked ? Format.Int(elapsed) : Format.String("Not picked");
    return elapsedText + " / " + Format.Int(obj.m_respawnTimeMinutes) + " minutes";
  }
  public static string Get(Pickable obj)
  {
    if (!Helper.IsValid(obj) || !Settings.Pickables) return "";
    List<string> lines = new();
    var respawn = GetRespawnTime(obj);
    lines.Add("Item: " + Format.String(obj.m_itemPrefab ? Utils.GetPrefabName(obj.m_itemPrefab) : "None"));
    lines.Add("Respawn: " + respawn);
    if (obj.m_amount > 1)
      lines.Add("Amount: " + Format.Int(obj.m_amount));
    return Format.JoinLines(lines);
  }

  public static string Get(CreatureSpawner obj)
  {
    if (!Helper.IsValid(obj)) return "";
    List<string> lines = new();
    var respawn = GetRespawnTime(obj);
    var noise = obj.m_triggerNoise > 0 ? " with noise of " + Format.Int(obj.m_triggerNoise) : "";
    lines.Add("Respawn: " + Format.String(respawn));
    lines.Add(Text.GetLevel(obj.m_minLevel, obj.m_maxLevel, 10));
    var timeText = GetTime(obj);
    if (timeText.Length > 0) lines.Add(timeText);
    if (obj.m_setPatrolSpawnPoint) lines.Add("Patrol point");
    lines.Add("Activates within " + Format.Int(obj.m_triggerDistance) + " meters" + noise);
    return Format.JoinLines(lines);
  }

  private static string GetEventText()
  {
    var obj = RandEventSystem.instance;
    return Text.GetAttempt(obj.m_eventTimer, obj.m_eventIntervalMin * 60, obj.m_eventChance);
  }
  private static string GetEventsText()
  {
    var instance = RandEventSystem.instance;
    var zdo = ZDOMan.instance.GetZDO(Player.m_localPlayer.GetZDOID());
    var currentBiome = WorldGenerator.instance.GetBiome(Player.m_localPlayer.transform.position);
    var texts = instance.m_events.Where(randomEvent => randomEvent.m_enabled && randomEvent.m_random).Select(randomEvent =>
    {
      var validBiome = (currentBiome & randomEvent.m_biome) > 0;
      var validBase = instance.CheckBase(randomEvent, zdo);
      var validKeys = instance.HaveGlobalKeys(randomEvent);
      var valid = validBiome && validBase && validKeys;
      List<string> parts = new(){
          Texts.GetNames(randomEvent.m_biome, currentBiome),
          Text.GetGlobalKeys(randomEvent.m_requiredGlobalKeys, randomEvent.m_notRequiredGlobalKeys),
          randomEvent.m_nearBaseOnly ? Format.String("player base", validBase) : ""
        };
      return Format.String(randomEvent.m_name, valid) + ": " + Format.JoinRow(parts);
    });
    return Format.JoinLines(texts);
  }
  private static string GetSpawnerText(SpawnSystem obj, SpawnSystem.SpawnData spawnData, int stableHashCode)
  {
    List<string> lines = new();
    var timeSinceSpawned = Helper.GetElapsed(obj, stableHashCode, 0);
    /*var time = "";
    if (!spawnData.m_spawnAtDay)
    {
      time = ", only during " + Format.String("night");
    }
    if (!spawnData.m_spawnAtNight)
    {
      time = ", only during " + Format.String("day");
    }*/
    var forest = "";
    if (!spawnData.m_inForest)
    {
      forest = ", only outside forests";
    }
    if (!spawnData.m_outsideForest)
    {
      forest = ", only inside forests";
    }

    var instances = SpawnSystem.GetNrOfInstances(spawnData.m_prefab, Player.m_localPlayer.transform.position, 0f, true, false);
    var progress = Format.ProgressPercent("Attempt", timeSinceSpawned, spawnData.m_spawnInterval);
    var chance = Format.Percent(spawnData.m_spawnChance / 100.0) + " chance";
    //var weather = spawnData.m_requiredEnvironments.Count > 0 ? (", Weather: " + Format.String(Format.JoinRow(spawnData.m_requiredEnvironments))) : "";
    //var global = spawnData.m_requiredGlobalKey != "" ? (", Bosses: " + Format.String(spawnData.m_requiredGlobalKey)) : "";
    var spawns = Format.Progress(instances, spawnData.m_maxSpawned);
    var spawnDistance = Format.Int(spawnData.m_spawnDistance) + " meters";
    var level = Text.GetLevel(spawnData.m_minLevel, spawnData.m_maxLevel, 10, spawnData.m_levelUpMinCenterDistance);
    var group = Format.Range(spawnData.m_groupSizeMin, spawnData.m_groupSizeMax);
    var groupRadius = (spawnData.m_groupSizeMax > spawnData.m_groupSizeMin) ? " within " + Format.Int(spawnData.m_groupRadius) + " meters" : "";
    var altitude = Format.Range(spawnData.m_minAltitude, spawnData.m_maxAltitude);
    var offset = (spawnData.m_groundOffset > 0) ? ", " + Format.Int(spawnData.m_groundOffset) + " meters off ground" : "";
    var tilt = Format.Range(spawnData.m_minTilt, spawnData.m_maxTilt);
    var ocean = Format.Range(spawnData.m_minOceanDepth, spawnData.m_maxOceanDepth);
    var hunt = spawnData.m_huntPlayer ? ", forces hunt mode" : "";
    lines.Add("Creature: " + Format.String(spawnData.m_prefab.name) + hunt);
    lines.Add("Altitude: " + altitude + offset + ", Ocean depth: " + ocean + ", Tilt: " + tilt);
    lines.Add(progress + ", " + chance);
    var biomeString = GetBiomes(spawnData.m_biome, spawnData.m_biomeArea);
    if (biomeString.Length > 0)
      lines.Add(biomeString + forest);
    lines.Add("Creature limit: " + spawns + ", Distance limit: " + spawnDistance);
    lines.Add(level + ", Group size: " + group + groupRadius);
    return Format.JoinLines(lines);
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
      text += Format.JoinLines(texts);
    }
    return text;
  }

  private static string GetZoneText(SpawnSystem obj)
  {
    var position = obj.transform.position;
    var heightmap = obj.m_heightmap;
    var zone = ZoneSystem.instance.GetZone(position);
    var text = "Zone: " + Format.String(zone.x + ";" + zone.y);
    var biome = heightmap.GetBiome(position);
    var biomeArea = heightmap.GetBiomeArea();
    var biomeAreaString = biomeArea == Heightmap.BiomeArea.Median ? ", full" : "";
    // var area = heightmap.GetBiomeArea();
    text += " (" + Translate.Name(biome) + biomeAreaString + ")";
    text += "\n";
    return text;
  }
  public static string Get(SpawnSystem obj, SpawnSystem.SpawnData spawnData, int stableHashCode)
  {
    List<string> lines = new();
    var timeSinceSpawned = Helper.GetElapsed(obj, stableHashCode, 0);
    lines.Add(GetZoneText(obj));
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

    var instances = SpawnSystem.GetNrOfInstances(spawnData.m_prefab, Player.m_localPlayer.transform.position, 0f, false, false);
    var weather = spawnData.m_requiredEnvironments.Count > 0 ? (", Weather: " + Format.String(Format.JoinRow(spawnData.m_requiredEnvironments))) : "";
    var global = spawnData.m_requiredGlobalKey != "" ? (", Bosses: " + Format.String(spawnData.m_requiredGlobalKey)) : "";
    var spawns = Format.Progress(instances, spawnData.m_maxSpawned);
    var spawnDistance = Format.Int(spawnData.m_spawnDistance) + " meters";
    var level = Text.GetLevel(spawnData.m_minLevel, spawnData.m_maxLevel, 10, spawnData.m_levelUpMinCenterDistance);
    var group = Format.Range(spawnData.m_groupSizeMin, spawnData.m_groupSizeMax);
    var groupRadius = (spawnData.m_groupSizeMax > spawnData.m_groupSizeMin) ? " within " + Format.Int(spawnData.m_groupRadius) + " meters" : "";
    var altitude = Format.Range(spawnData.m_minAltitude, spawnData.m_maxAltitude);
    var offset = (spawnData.m_groundOffset > 0) ? ", " + Format.Int(spawnData.m_groundOffset) + " meters off ground" : "";
    var tilt = Format.Range(spawnData.m_minTilt, spawnData.m_maxTilt);
    var ocean = Format.Range(spawnData.m_minOceanDepth, spawnData.m_maxOceanDepth);
    var hunt = spawnData.m_huntPlayer ? ", forces hunt mode" : "";
    var spawnRadius = Format.Range(spawnData.m_spawnRadiusMin > 0 ? spawnData.m_spawnRadiusMin : SpawnSystem.m_spawnDistanceMin, spawnData.m_spawnRadiusMax > 0 ? spawnData.m_spawnRadiusMax : SpawnSystem.m_spawnDistanceMax) + " meters";
    lines.Add("Creature: " + Format.String(spawnData.m_prefab.name) + hunt);
    lines.Add(Text.GetAttempt(timeSinceSpawned, spawnData.m_spawnInterval, spawnData.m_spawnChance));
    var biomeString = Texts.GetBiomes(spawnData.m_biome, spawnData.m_biomeArea);
    lines.Add(biomeString + forest + forest + weather + global + time);
    lines.Add("Creature limit: " + spawns + ", Distance limit: " + spawnDistance + ", From players: " + spawnRadius);
    lines.Add(level + ", Group size: " + group + groupRadius);
    lines.Add("Altitude: " + altitude + offset + ", Tilt: " + tilt + ", Water: " + ocean);
    return Format.JoinLines(lines);
  }
}
