using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ESP
{
  public class SpawnSystemUtils
  {
    private static Color GetBiomeColor(Heightmap.Biome biome)
    {
      if (biome == Heightmap.Biome.AshLands) return Color.red;
      if (biome == Heightmap.Biome.BlackForest) return Color.magenta;
      if (biome == Heightmap.Biome.DeepNorth) return Color.gray;
      if (biome == Heightmap.Biome.Meadows) return Color.green;
      if (biome == Heightmap.Biome.Mistlands) return Color.gray;
      if (biome == Heightmap.Biome.Mountain) return Color.white;
      if (biome == Heightmap.Biome.Ocean) return Color.blue;
      if (biome == Heightmap.Biome.Plains) return Color.yellow;
      if (biome == Heightmap.Biome.Swamp) return Color.cyan;
      return Color.black;
    }

    private static string GetBiomeName(Heightmap.Biome biome)
    {
      var names = new List<string>();
      if ((biome & Heightmap.Biome.AshLands) > 0) names.Add("Ash Lands");
      if ((biome & Heightmap.Biome.BlackForest) > 0) names.Add("Black Forest");
      if ((biome & Heightmap.Biome.DeepNorth) > 0) names.Add("Deep North");
      if ((biome & Heightmap.Biome.Meadows) > 0) names.Add("Meadows");
      if ((biome & Heightmap.Biome.Mistlands) > 0) names.Add("Mistlands");
      if ((biome & Heightmap.Biome.Mountain) > 0) names.Add("Mountain");
      if ((biome & Heightmap.Biome.Ocean) > 0) names.Add("Ocean");
      if ((biome & Heightmap.Biome.Plains) > 0) names.Add("Plains");
      if ((biome & Heightmap.Biome.Swamp) > 0) names.Add("Swamp");
      return names.Join(null, ", ");
    }
    private static void DrawMarker(GameObject parent, Vector3 position, Heightmap.Biome biome)
    {
      Drawer.DrawMarkerLine(parent, position, GetBiomeColor(biome), 0.25f, TextUtils.StringValue(GetBiomeName(biome)));
    }
    private static Heightmap.Biome GetBiome(SpawnSystem instance, Heightmap heightmap, Vector3 relative)
    {
      var position = instance.transform.position;
      var biomePosition = new Vector3(position.x + relative.x, 0f, position.z + relative.z);
      return heightmap.GetBiome(biomePosition);
    }
    public static void DrawBiomes(SpawnSystem instance, Heightmap heightmap)
    {
      if (!Settings.showBiomes)
        return;
      var num = ZoneSystem.instance.m_zoneSize * 0.5f;
      var pos1 = new Vector3(num, 0f, num);
      var pos2 = new Vector3(-num, 0f, num);
      var pos3 = new Vector3(num, 0f, -num);
      var pos4 = new Vector3(-num, 0f, -num);
      var biome1 = GetBiome(instance, heightmap, pos1);
      var biome2 = GetBiome(instance, heightmap, pos2);
      var biome3 = GetBiome(instance, heightmap, pos3);
      var biome4 = GetBiome(instance, heightmap, pos4);
      DrawMarker(instance.gameObject, pos1, biome1);
      DrawMarker(instance.gameObject, pos2, biome2);
      DrawMarker(instance.gameObject, pos3, biome3);
      DrawMarker(instance.gameObject, pos4, biome4);
    }
    private static string GetZoneText(SpawnSystem instance, Heightmap heightmap)
    {
      var zone = ZoneSystem.instance.GetZone(instance.transform.position);
      var text = "Zone: " + TextUtils.StringValue(zone.x + ";" + zone.y);
      var biome = heightmap.GetBiome(instance.transform.position);
      var biomeArea = heightmap.GetBiomeArea();
      var biomeAreaString = ((biomeArea == Heightmap.BiomeArea.Median) ? " (full)" : "");
      var area = heightmap.GetBiomeArea();
      text += " Biome: " + TextUtils.StringValue(GetBiomeName(biome)) + biomeAreaString;
      text += " Weather: " + TextUtils.StringValue(EnvMan.instance.GetCurrentEnvironment().m_name);
      text += "\n";
      return text;
    }
    public static List<string> GetSpawnSystemTexts(SpawnSystem instance, Heightmap heightmap, ZNetView nview)
    {
      var zoneText = GetZoneText(instance, heightmap);
      var time = ZNet.instance.GetTime();
      var biome = heightmap.GetBiome(instance.transform.position);
      var num = 0;
      var texts = new List<string>();
      foreach (SpawnSystem.SpawnData spawnData in instance.m_spawners)
      {
        num++;
        if (!spawnData.m_enabled || !heightmap.HaveBiome(spawnData.m_biome)) continue;
        if (!spawnData.m_spawnAtDay && !spawnData.m_spawnAtNight) continue;
        var text = zoneText;
        var stableHashCode = ("b_" + spawnData.m_prefab.name + num.ToString()).GetStableHashCode();
        DateTime d = new DateTime(nview.GetZDO().GetLong(stableHashCode, 0L));
        var timer = (time - d).TotalSeconds;
        var timeString = "";
        if (!spawnData.m_spawnAtDay)
        {
          timeString = ", only during " + TextUtils.StringValue("night");
        }
        if (!spawnData.m_spawnAtNight)
        {
          timeString = ", only during " + TextUtils.StringValue("day");
        }
        var forestString = "";
        if (!spawnData.m_inForest)
        {
          forestString = ", only outside forests";
        }
        if (!spawnData.m_outsideForest)
        {
          forestString = ", only inside forests";
        }
        var groupString = (spawnData.m_groupSizeMax > spawnData.m_groupSizeMin) ? spawnData.m_groupSizeMin + "-" + spawnData.m_groupSizeMax : spawnData.m_groupSizeMax.ToString();
        var groupRadiusString = (spawnData.m_groupSizeMax > spawnData.m_groupSizeMin) ? " within " + TextUtils.IntValue(spawnData.m_groupRadius) + " meters" : "";
        var levelString = (spawnData.m_maxLevel > spawnData.m_minLevel) ? spawnData.m_minLevel + "-" + spawnData.m_maxLevel : spawnData.m_maxLevel.ToString();
        var levelLimitString = (spawnData.m_levelUpMinCenterDistance > 0) ? " after " + TextUtils.IntValue(spawnData.m_levelUpMinCenterDistance) + " meters" : "";
        var instances = SpawnSystem.GetNrOfInstances(spawnData.m_prefab, Vector3.zero, 0f, false, false);
        var offsetString = (spawnData.m_groundOffset > 0) ? ", " + TextUtils.IntValue(spawnData.m_groundOffset) + " meters off ground" : "";
        var biomeAreaString = ((spawnData.m_biomeArea == Heightmap.BiomeArea.Median) ? ", only full biomes" : "");
        var weatherString = spawnData.m_requiredEnvironments.Count > 0 ? (", Weather: " + TextUtils.StringValue(spawnData.m_requiredEnvironments.Join(null, ", "))) : "";
        var globalString = spawnData.m_requiredGlobalKey != "" ? (", Bosses: " + TextUtils.StringValue(spawnData.m_requiredGlobalKey)) : "";
        text += "\nCreature: " + TextUtils.StringValue(spawnData.m_prefab.name);
        text += "\n" + TextUtils.ProgressValue("Attempt", timer, spawnData.m_spawnInterval) + ", " + TextUtils.PercentValue(spawnData.m_spawnChance / 100.0) + " chance";
        text += "\nBiome: " + TextUtils.StringValue(GetBiomeName(spawnData.m_biome)) + biomeAreaString + forestString + weatherString + globalString;
        text += "\nCreature limit: " + TextUtils.StringValue(instances + "/" + spawnData.m_maxSpawned) + ", Distance limit: " + TextUtils.IntValue(spawnData.m_spawnDistance) + " meters";
        text += "\nLevel: " + TextUtils.StringValue(levelString) + levelLimitString + ", Group size: " + TextUtils.StringValue(groupString) + groupRadiusString;
        text += "\nAltitude: " + TextUtils.StringValue(spawnData.m_minAltitude + "-" + spawnData.m_maxAltitude) + offsetString;
        texts.Add(text);
      }
      return texts;
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
    public static void DrawSpawnSystems(SpawnSystem instance, Heightmap heightmap, ZNetView nview)
    {
      if (!Settings.showSpawnSystems) return;
      var texts = GetSpawnSystemTexts(instance, heightmap, nview);
      var totalAmount = GetTotalAmountOfSpawnSystems(instance, heightmap);
      var counter = -totalAmount / 2;
      var num = 0;
      var biome = GetBiome(instance, heightmap, instance.transform.position);
      foreach (SpawnSystem.SpawnData spawnData in instance.m_spawners)
      {
        num++;
        if (!spawnData.m_enabled || !heightmap.HaveBiome(spawnData.m_biome)) continue;
        if (!spawnData.m_spawnAtDay && !spawnData.m_spawnAtNight) return;
        var text = texts[totalAmount / 2 + counter];
        Drawer.DrawMarkerLine(instance.gameObject, new Vector3(counter * 2, 0, 0), GetBiomeColor(biome), 1f, text);
        counter++;
      }
    }
  }

  [HarmonyPatch(typeof(SpawnSystem), "Awake")]
  public class SpawnSystem_Awake
  {
    public static void Postfix(SpawnSystem __instance, Heightmap ___m_heightmap, ZNetView ___m_nview)
    {
      SpawnSystemUtils.DrawBiomes(__instance, ___m_heightmap);
      SpawnSystemUtils.DrawSpawnSystems(__instance, ___m_heightmap, ___m_nview);
    }
  }
  [HarmonyPatch(typeof(SpawnSystem), "UpdateSpawning")]
  public class SpawnSystem_UpdateSpawning
  {
    public static void Postfix(SpawnSystem __instance, Heightmap ___m_heightmap, ZNetView ___m_nview)
    {
      if (!Settings.showSpawnSystems) return;
      var texts = SpawnSystemUtils.GetSpawnSystemTexts(__instance, ___m_heightmap, ___m_nview);
      Drawer.UpdateTexts(__instance.gameObject, texts);
    }
  }
}