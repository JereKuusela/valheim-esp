using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Service;
using UnityEngine;

namespace ESP {
  public partial class Texts {
    private static Heightmap.Biome[] BIOMES = new Heightmap.Biome[]{
      Heightmap.Biome.AshLands,
      Heightmap.Biome.BlackForest,
      Heightmap.Biome.DeepNorth,
      Heightmap.Biome.Meadows,
      Heightmap.Biome.Mistlands,
      Heightmap.Biome.Mountain,
      Heightmap.Biome.Ocean,
      Heightmap.Biome.Plains,
      Heightmap.Biome.Swamp
    };

    private const Heightmap.Biome BIOME_MAX = Heightmap.Biome.AshLands | Heightmap.Biome.BlackForest
     | Heightmap.Biome.DeepNorth | Heightmap.Biome.Meadows | Heightmap.Biome.Mistlands
     | Heightmap.Biome.Mountain | Heightmap.Biome.Ocean | Heightmap.Biome.Plains | Heightmap.Biome.Swamp;

    public static string GetNames(Heightmap.Biome biomes, Heightmap.Biome validBiome = BIOME_MAX) {
      var names = new List<string>();
      foreach (var biome in BIOMES)
        if ((biomes & biome) > 0) names.Add(Format.String(Translate.Name(biome), ((validBiome & biome) > 0)));
      if (names.Count == BIOMES.Length) return "";
      return Format.JoinRow(names);
    }
    public static string Get(Heightmap.Biome obj) {
      var text = Translate.Name(obj) + "\n" + EnvUtils.GetTime() + ", " + EnvUtils.GetCurrentEnvironment();
      var envs = EnvMan.instance.GetAvailableEnvironments(obj);
      var totalWeight = envs.Sum(env => env.m_weight);
      var avgWind = envs.Sum(EnvUtils.GetAvgWind) / totalWeight;
      text += "\n" + EnvUtils.GetWind() + " (" + Format.Percent(avgWind) + " on average), Current roll: " + EnvUtils.GetWindRoll();
      text += "\n\n" + EnvUtils.GetProgress() + ", Current roll: " + EnvUtils.GetEnvironmentRoll();
      var texts = envs.Select(env => EnvUtils.GetEnvironment(env, totalWeight));
      return text + "\n" + Format.JoinLines(texts);
    }
    public static string GetBiomes(Heightmap.Biome biome, Heightmap.BiomeArea area = Heightmap.BiomeArea.Edge, bool addLabel = true) {
      var biomeText = GetNames(biome);
      if (biomeText.Length == 0) return "";
      var label = addLabel ? "Biomes: " : "";
      var biomeArea = (area == Heightmap.BiomeArea.Median) ? ", only full biomes" : "";
      return label + biomeText + biomeArea;
    }
  }

  [HarmonyPatch(typeof(Minimap), "UpdateBiome")]

  public class Minimap_ShowPos {
    // Text doesn't always get updated so extra stuff must be reseted manually.
    private static string previousText = "";
    public static void Prefix(Minimap __instance) {
      __instance.m_biomeNameLarge.text = previousText;

    }
    public static void Postfix(Minimap __instance, Player player) {
      previousText = __instance.m_biomeNameLarge.text;
      var mode = __instance.m_mode;
      var position = player.transform.position;
      if (mode == Minimap.MapMode.Large)
        position = __instance.ScreenToWorldPoint(ZInput.IsMouseActive() ? Input.mousePosition : new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2)));
      var zone = ZoneSystem.instance.GetZone(position);
      var positionText = "x: " + position.x.ToString("F0") + " z: " + position.z.ToString("F0");
      var zoneText = "zone: " + zone.x + "/" + zone.y;
      var text = "\n\n" + previousText + "\n" + zoneText + "\n" + positionText;
      __instance.m_biomeNameLarge.text = text;
    }
  }
}

