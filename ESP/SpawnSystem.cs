using HarmonyLib;
using UnityEngine;

namespace ESP
{

  [HarmonyPatch(typeof(SpawnSystem), "Awake")]
  public class SpawnSystem_Awake
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
      if (biome == Heightmap.Biome.AshLands) return "Ash Lands";
      if (biome == Heightmap.Biome.BlackForest) return "Black Forest";
      if (biome == Heightmap.Biome.DeepNorth) return "Deep North";
      if (biome == Heightmap.Biome.Meadows) return "Meadows";
      if (biome == Heightmap.Biome.Mistlands) return "Mistlands";
      if (biome == Heightmap.Biome.Mountain) return "Mountain";
      if (biome == Heightmap.Biome.Ocean) return "Ocean";
      if (biome == Heightmap.Biome.Plains) return "Plains";
      if (biome == Heightmap.Biome.Swamp) return "Swamp";
      return "Unknown";
    }
    private static void DrawMarker(GameObject parent, Vector3 position, Heightmap.Biome biome)
    {
      Drawer.DrawMarkerLine(parent, position, GetBiomeColor(biome), 0.25f, GetBiomeName(biome));
    }
    private static Heightmap.Biome GetBiome(SpawnSystem instance, Heightmap heightmap, Vector3 relative)
    {
      var position = instance.transform.position;
      var biomePosition = new Vector3(position.x + relative.x, 0f, position.z + relative.z);
      return heightmap.GetBiome(biomePosition);
    }
    public static void Postfix(SpawnSystem __instance, Heightmap ___m_heightmap)
    {
      if (!Settings.showBiomes)
        return;
      var num = ZoneSystem.instance.m_zoneSize * 0.5f;
      var pos1 = new Vector3(num, 0f, num);
      var pos2 = new Vector3(-num, 0f, num);
      var pos3 = new Vector3(num, 0f, -num);
      var pos4 = new Vector3(-num, 0f, -num);
      var biome1 = GetBiome(__instance, ___m_heightmap, pos1);
      var biome2 = GetBiome(__instance, ___m_heightmap, pos2);
      var biome3 = GetBiome(__instance, ___m_heightmap, pos3);
      var biome4 = GetBiome(__instance, ___m_heightmap, pos4);
      DrawMarker(__instance.gameObject, pos1, biome1);
      DrawMarker(__instance.gameObject, pos2, biome2);
      DrawMarker(__instance.gameObject, pos3, biome3);
      DrawMarker(__instance.gameObject, pos4, biome4);
    }
  }

}