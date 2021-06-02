using UnityEngine;
using System;
using System.Collections.Generic;

namespace ESP
{
  public class BiomeUtils
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
    public static string GetNames(Heightmap.Biome biomes, Heightmap.Biome validBiome = Heightmap.Biome.None)
    {
      var names = new List<string>();
      foreach (var biome in BIOMES)
        if ((biomes & biome) > 0) names.Add(TextUtils.String(GetName(biome), ((validBiome & biome) > 0)));
      if (names.Count == BIOMES.Length) return "";
      return String.Join(", ", names);
    }
  }
}

