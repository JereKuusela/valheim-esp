using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESP
{
  public partial class Texts
  {
    public static Color GetColor(Heightmap.Biome biome)
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
    public static string GetName(Heightmap.Biome biome)
    {
      if (biome == Heightmap.Biome.AshLands) return "Ash Lands";
      if (biome == Heightmap.Biome.BlackForest) return "Black Forest";
      if (biome == Heightmap.Biome.DeepNorth) return "Deep North";
      if (biome == Heightmap.Biome.Meadows) return "Meadows";
      if (biome == Heightmap.Biome.Mistlands) return "Mistlands";
      if (biome == Heightmap.Biome.Mountain) return "Mountain";
      if (biome == Heightmap.Biome.Ocean) return "Ocean";
      if (biome == Heightmap.Biome.Plains) return "Plains";
      if (biome == Heightmap.Biome.Swamp) return "Swamp";
      return "";
    }
    private const Heightmap.Biome BIOME_MAX = Heightmap.Biome.AshLands | Heightmap.Biome.BlackForest
     | Heightmap.Biome.DeepNorth | Heightmap.Biome.Meadows | Heightmap.Biome.Mistlands
     | Heightmap.Biome.Mountain | Heightmap.Biome.Ocean | Heightmap.Biome.Plains | Heightmap.Biome.Swamp;

    public static string GetNames(Heightmap.Biome biomes, Heightmap.Biome validBiome = BIOME_MAX)
    {
      var names = new List<string>();
      foreach (var biome in BIOMES)
        if ((biomes & biome) > 0) names.Add(Format.String(GetName(biome), ((validBiome & biome) > 0)));
      if (names.Count == BIOMES.Length) return "";
      return Format.JoinRow(names);
    }
    public static string Get(Heightmap.Biome obj)
    {
      var text = Format.Name(obj) + "\n" + EnvUtils.GetTime() + ", " + EnvUtils.GetCurrentEnvironment();
      var envs = Patch.EnvMan_GetAvailableEnvironments(EnvMan.instance, obj);
      var totalWeight = envs.Sum(env => env.m_weight);
      var avgWind = envs.Sum(EnvUtils.GetAvgWind) / totalWeight;
      text += "\n" + EnvUtils.GetWind() + " (" + Format.Percent(avgWind) + " on average)";
      text += "\n\n" + EnvUtils.GetProgress() + ", Current roll: " + EnvUtils.GetEnvironmentRoll(totalWeight);
      var texts = envs.Select(env => EnvUtils.GetEnvironment(env, totalWeight));
      return text + "\n" + Format.JoinLines(texts);
    }
    public static string GetBiomes(Heightmap.Biome biome, Heightmap.BiomeArea area = Heightmap.BiomeArea.Edge)
    {
      var biomeText = GetNames(biome);
      if (biomeText.Length == 0) return "";
      var biomeArea = (area == Heightmap.BiomeArea.Median) ? ", only full biomes" : "";
      return "Biomes: " + biomeText + biomeArea;
    }
  }
}

