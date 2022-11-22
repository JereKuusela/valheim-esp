namespace Service;
public class Names
{
  public static string GetName(Heightmap.Biome biome)
  {
    switch (biome)
    {
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
}
