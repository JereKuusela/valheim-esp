namespace Service;
public class Names
{
  public static string GetName(Heightmap.Biome biome)
  {
    return biome switch
    {
      Heightmap.Biome.AshLands => "Ash Lands",
      Heightmap.Biome.BlackForest => "Black Forest",
      Heightmap.Biome.DeepNorth => "Deep North",
      Heightmap.Biome.Meadows => "Meadows",
      Heightmap.Biome.Mistlands => "Mistlands",
      Heightmap.Biome.Mountain => "Mountain",
      Heightmap.Biome.Ocean => "Ocean",
      Heightmap.Biome.Plains => "Plains",
      Heightmap.Biome.Swamp => "Swamp",
      _ => "",
    };
  }
}
