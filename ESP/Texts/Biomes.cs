using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ESP {
  public partial class Texts {
    public static Color GetColor(Heightmap.Biome biome) {
      switch (biome) {
        case Heightmap.Biome.AshLands:
          return Settings.BiomeAshlandsColor;
        case Heightmap.Biome.BlackForest:
          return Settings.BiomeBlackForestColor;
        case Heightmap.Biome.DeepNorth:
          return Settings.BiomeDeepNorthColor;
        case Heightmap.Biome.Meadows:
          return Settings.BiomeMeadowsColor;
        case Heightmap.Biome.Mistlands:
          return Settings.BiomeMistlandsColor;
        case Heightmap.Biome.Mountain:
          return Settings.BiomeMountainColor;
        case Heightmap.Biome.Ocean:
          return Settings.BiomeOceanColor;
        case Heightmap.Biome.Plains:
          return Settings.BiomePlainsColor;
        case Heightmap.Biome.Swamp:
          return Settings.BiomeSwampColor;
        default:
          return Settings.BiomeOtherColor;
      }
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
    public static string GetName(Heightmap.Biome biome) {
      switch (biome) {
        case Heightmap.Biome.AshLands:
          return "Ash Lands";
        case Heightmap.Biome.BlackForest:
          return "Black Forest";
        case Heightmap.Biome.DeepNorth:
          return "Deep North";
        case Heightmap.Biome.Meadows:
          return "Meadows";
        case Heightmap.Biome.Mistlands:
          return "Mistlands";
        case Heightmap.Biome.Mountain:
          return "Mountain";
        case Heightmap.Biome.Ocean:
          return "Ocean";
        case Heightmap.Biome.Plains:
          return "Plains";
        case Heightmap.Biome.Swamp:
          return "Swamp";
        default:
          return "";
      }
    }
    private const Heightmap.Biome BIOME_MAX = Heightmap.Biome.AshLands | Heightmap.Biome.BlackForest
     | Heightmap.Biome.DeepNorth | Heightmap.Biome.Meadows | Heightmap.Biome.Mistlands
     | Heightmap.Biome.Mountain | Heightmap.Biome.Ocean | Heightmap.Biome.Plains | Heightmap.Biome.Swamp;

    public static string GetNames(Heightmap.Biome biomes, Heightmap.Biome validBiome = BIOME_MAX) {
      var names = new List<string>();
      foreach (var biome in BIOMES)
        if ((biomes & biome) > 0) names.Add(Format.String(GetName(biome), ((validBiome & biome) > 0)));
      if (names.Count == BIOMES.Length) return "";
      return Format.JoinRow(names);
    }
    public static string Get(Heightmap.Biome obj) {
      var text = Format.Name(obj) + "\n" + EnvUtils.GetTime() + ", " + EnvUtils.GetCurrentEnvironment();
      var envs = Patch.EnvMan_GetAvailableEnvironments(EnvMan.instance, obj);
      var totalWeight = envs.Sum(env => env.m_weight);
      var avgWind = envs.Sum(EnvUtils.GetAvgWind) / totalWeight;
      text += "\n" + EnvUtils.GetWind() + " (" + Format.Percent(avgWind) + " on average)";
      text += "\n\n" + EnvUtils.GetProgress() + ", Current roll: " + EnvUtils.GetEnvironmentRoll(totalWeight);
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
}

