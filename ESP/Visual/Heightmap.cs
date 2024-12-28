using System.Collections.Generic;
using System.Globalization;
using HarmonyLib;
using UnityEngine;

namespace ESP;
public partial class Visual
{
  private static readonly List<GameObject> renderers = [];
  // Copy paste from old Valheim code.
  private static Vector3 CalcNormal(Heightmap hm, int x, int y)
  {
    Vector3 vector3_1 = hm.CalcVertex(x, y);
    Vector3 rhs;
    if (x == hm.m_width)
    {
      Vector3 vector3_2 = hm.CalcVertex(x - 1, y);
      rhs = vector3_1 - vector3_2;
    }
    else
      rhs = hm.CalcVertex(x + 1, y) - vector3_1;
    Vector3 lhs;
    if (y == hm.m_width)
    {
      Vector3 vector3_3 = hm.CalcVertex(x, y - 1);
      lhs = vector3_1 - vector3_3;
    }
    else
      lhs = hm.CalcVertex(x, y + 1) - vector3_1;
    return Vector3.Cross(lhs, rhs).normalized;
  }
  public static void DrawHeightmap(Vector3 pos, float radius)
  {
    List<Heightmap> hms = [];
    Heightmap.FindHeightmap(pos, radius, hms);
    foreach (var renderer in renderers)
      Object.Destroy(renderer);
    renderers.Clear();
    foreach (var hm in hms)
    {
      var size = hm.m_width + 1;
      var tc = TerrainComp.FindTerrainCompiler(hm.transform.position);
      for (var i = 0; i < size; i++)
      {
        for (var j = 0; j < size; j++)
        {
          var vector = hm.transform.position + hm.CalcVertex(j, i);
          var vertex = hm.CalcVertex(j, i);
          if (Utils.DistanceXZ(vector, pos) > radius) continue;
          var normal = CalcNormal(hm, j, i);
          var line = Visualization.Draw.DrawLineWithFixedRotation(Tag.Terrain, hm, vertex, vertex + normal * 1f);
          var paint = hm.GetPaintMask(j, i);
          line.GetComponent<LineRenderer>().material.SetColor("_Color", paint);
          renderers.Add(line);
          var height = hm.m_heights[j * size + i].ToString("F4", CultureInfo.InvariantCulture);
          var level = "";
          var smooth = "";
          var index = i * size + j;
          if (tc)
          {
            level = tc.m_levelDelta[index].ToString("F4", CultureInfo.InvariantCulture);
            smooth = tc.m_smoothDelta[index].ToString("F4", CultureInfo.InvariantCulture);
          }
          var text = $"Index: {index}, i: {i}, j: {j}\nHeight: {height}\nLevel: {level}\nSmooth: {smooth}\nPaint: {paint:F4}";
          Visualization.Draw.AddText(line, "Terrain", text);
        }
      }
    }
  }
  public static void DrawPaintmap(Vector3 pos, float radius)
  {
    List<Heightmap> hms = [];
    Heightmap.FindHeightmap(pos, radius, hms);
    foreach (var renderer in renderers)
      Object.Destroy(renderer);
    renderers.Clear();
    foreach (var hm in hms)
    {
      // Texture has 65 pixels over 64 meters.
      // So the center should be at heightmap position, and then every 64/65 meters.
      var half = hm.m_width / 2;
      var size = hm.m_width + 1;
      var dist = (float)hm.m_width / size;
      for (var i = -half; i <= half; i++)
      {
        for (var j = -half; j <= half; j++)
        {
          var x = j * dist;
          var z = i * dist;
          var y = hm.GetWorldHeight(new(hm.transform.position.x + x, 0, hm.transform.position.z + z), out var h) ? h : 0;
          Vector3 vertex = new(x, y, z);
          var vector = hm.transform.position + vertex;
          if (Utils.DistanceXZ(vector, pos) > radius) continue;
          var line = Visualization.Draw.DrawLineWithFixedRotation(Tag.Terrain, hm, vertex, vertex + Vector3.up);
          var indexI = i + half;
          var indexJ = j + half;
          var paint = hm.GetPaintMask(indexJ, indexI);
          line.GetComponent<LineRenderer>().material.SetColor("_Color", paint);
          renderers.Add(line);
          var index = indexI * size + indexJ;
          var text = $"Index: {index}, i: {indexI}, j: {indexJ}\nPaint: {paint:F4}";
          Visualization.Draw.AddText(line, "Terrain", text);
        }
      }
    }
  }
}


[HarmonyPatch(typeof(Heightmap))]
public class TestPatches
{
  [HarmonyPatch("WorldToVertexMask"), HarmonyPrefix]
  static bool PatchWorldToVertexMask(Heightmap __instance, Vector3 worldPos, ref int x, ref int y)
  {
    WorldToVertexMask(__instance, worldPos, ref x, ref y);
    return false;
  }
  private const float scale = 64f / 65f;
  private const int half = 32;
  static void WorldToVertexMask(Heightmap hm, Vector3 worldPos, ref int x, ref int y)
  {
    Vector3 vector = worldPos - hm.transform.position;
    x = Mathf.FloorToInt(vector.x / scale + 0.5f) + half;
    y = Mathf.FloorToInt(vector.z / scale + 0.5f) + half;
  }


  [HarmonyPatch("GetVegetationMask"), HarmonyPrefix]
  static bool PatchGetVegetationMask(Heightmap __instance, Vector3 worldPos, ref float __result)
  {
    __result = GetVegetationMask(__instance, worldPos);
    return false;
  }
  static float GetVegetationMask(Heightmap hm, Vector3 worldPos)
  {
    //worldPos.x -= 0.5f;
    //worldPos.z -= 0.5f;
    hm.WorldToVertexMask(worldPos, out var x, out var y);
    return hm.m_paintMask.GetPixel(x, y).a;
  }
}