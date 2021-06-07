using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ESP
{
  public class RandEventSystemUtils
  {
    public static float timer = 0;

  }
  [HarmonyPatch(typeof(RandEventSystem), "UpdateRandomEvent")]
  public class RandEventSystem_UpdateRandomEvent
  {
    public static void Postfix(float ___m_eventTimer)
    {
      RandEventSystemUtils.timer = ___m_eventTimer;
    }
  }

  public class RandEventSystemText : MonoBehaviour, Hoverable
  {
    private string GetEventText()
    {
      var instance = RandEventSystem.instance;
      return TextUtils.GetAttempt(RandEventSystemUtils.timer, instance.m_eventIntervalMin * 60, instance.m_eventChance);
    }
    private string GetEventsText()
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
          BiomeUtils.GetNames(randomEvent.m_biome, currentBiome),
          TextUtils.GetGlobalKeys(randomEvent.m_requiredGlobalKeys, randomEvent.m_notRequiredGlobalKeys),
          randomEvent.m_nearBaseOnly ? TextUtils.String("player base", validBase) : ""
        };
        return TextUtils.String(randomEvent.m_name, valid) + ": " + String.Join(", ", parts.Where(part => part.Length > 0));
      });
      return String.Join("\n", texts);
    }
    private double GetTimeSinceSpawned(int stableHashCode)
    {
      var time = ZNet.instance.GetTime();
      DateTime d = new DateTime(nview.GetZDO().GetLong(stableHashCode, 0L));
      return (time - d).TotalSeconds;
    }
    private string GetSpawnerText(SpawnSystem.SpawnData spawnData, int stableHashCode)
    {
      var timeSinceSpawned = GetTimeSinceSpawned(stableHashCode);
      var text = "";
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

      var instances = SpawnSystem.GetNrOfInstances(spawnData.m_prefab, Vector3.zero, 0f, true, false);
      var progress = TextUtils.ProgressPercent("Attempt", timeSinceSpawned, spawnData.m_spawnInterval);
      var chance = TextUtils.Percent(spawnData.m_spawnChance / 100.0) + " chance";
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
      text += "\n" + progress + ", " + chance;
      var biomeString = TextUtils.GetBiomes(spawnData.m_biome, spawnData.m_biomeArea);
      if (biomeString.Length > 0)
        text += "\n" + biomeString + forest;
      text += "\nCreature limit: " + spawns + ", Distance limit: " + spawnDistance;
      text += "\n" + level + ", Group size: " + group + groupRadius;
      //text += "\nAltitude: " + altitude + offset + ", Tilt: " + tilt + ", Water: " + ocean;
      return text;
    }
    public string GetHoverText()
    {
      var instance = RandEventSystem.instance;
      var text = GetEventText() + "\n\n";
      var num = 0;
      var currentEvent = instance.GetCurrentRandomEvent();
      if (currentEvent == null || instance.GetCurrentSpawners() == null)
        text += GetEventsText();
      else
      {
        text += TextUtils.ProgressPercent(currentEvent.m_name, currentEvent.m_time, currentEvent.m_duration) + "\n";
        var spawners = instance.GetCurrentSpawners();
        var texts = spawners.Select(spawnData =>
        {
          num++;
          var stableHashCode = ("e_" + spawnData.m_prefab.name + num.ToString()).GetStableHashCode();
          return GetSpawnerText(spawnData, stableHashCode);
        });
        text += String.Join("\n", texts);
      }
      return text;
    }
    public string GetHoverName() => "Random events";
    public SpawnSystem spawnSystem;
    public Heightmap heightmap;
    public ZNetView nview;
  }
}