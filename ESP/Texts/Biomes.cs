using System.Collections.Generic;
using System.Linq;
using Service;
namespace ESP;
public partial class Texts
{
  private static Heightmap.Biome[] BIOMES = new[]{
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

  public static string GetNames(Heightmap.Biome biomes, Heightmap.Biome validBiome = BIOME_MAX)
  {
    List<string> names = new();
    foreach (var biome in BIOMES)
      if ((biomes & biome) > 0) names.Add(Format.String(Translate.Name(biome), ((validBiome & biome) > 0)));
    if (names.Count == BIOMES.Length) return "";
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
