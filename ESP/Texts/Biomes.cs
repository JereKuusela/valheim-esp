using System;
using System.Collections.Generic;
using System.Linq;
using Service;
namespace ESP;
public partial class Texts
{
  public static string GetNames(Heightmap.Biome biome, Heightmap.Biome validBiome = (Heightmap.Biome)(-1))
  {
    if (biome == Heightmap.Biome.None) return "None";
    var number = 1;
    var biomeNumber = (int)biome;
    List<string> names = [];
    while (number <= biomeNumber)
    {
      if ((number & biomeNumber) > 0)
        names.Add(Format.String(Enum.GetName(typeof(Heightmap.Biome), biome), (validBiome & biome) > 0));
      number *= 2;
    }
    return Format.JoinRow(names);
  }
  public static string Get(Heightmap.Biome obj)
  {
    var text = Translate.Name(obj) + "\n" + EnvUtils.GetTime() + ", " + EnvUtils.GetCurrentEnvironment();
    var envs = EnvMan.instance.GetAvailableEnvironments(obj);
    var totalWeight = envs.Sum(env => env.m_weight);
    var avgWind = envs.Sum(EnvUtils.GetAvgWind) / totalWeight;
    text += "\n" + EnvUtils.GetWind() + " (" + Format.Percent(avgWind) + " on average), Current roll: " + EnvUtils.GetWindRoll();
    text += "\n\n" + EnvUtils.GetProgress() + ", Current roll: " + EnvUtils.GetEnvironmentRoll();
    var texts = envs.Select(env => EnvUtils.GetEnvironment(env, totalWeight));
    return text + "\n" + Format.JoinLines(texts);
  }
  public static string GetBiomes(Heightmap.Biome biome, Heightmap.BiomeArea area = Heightmap.BiomeArea.Edge, bool addLabel = true)
  {
    var biomeText = GetNames(biome);
    if (biomeText.Length == 0) return "";
    var label = addLabel ? "Biomes: " : "";
    var edges = (area & Heightmap.BiomeArea.Edge) > 0;
    var centers = (area & Heightmap.BiomeArea.Median) > 0;
    var biomeArea = (edges && centers) ? "" : centers ? ", only full biomes" : edges ? ", only edge biomes" : "invalid biome area";
    return label + biomeText + biomeArea;
  }
}
