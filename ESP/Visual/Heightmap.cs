using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace ESP;
public partial class Visual
{
  private static readonly List<GameObject> renderers = new();
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
    List<Heightmap> hms = new();
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
          var num2 = i * size + j;
          if (tc)
          {
            level = tc.m_levelDelta[num2].ToString("F4", CultureInfo.InvariantCulture);
            smooth = tc.m_smoothDelta[num2].ToString("F4", CultureInfo.InvariantCulture);
          }
          var text = $"Index: {num2}, i: {i}, j: {j}\nHeight: {height}\nLevel: {level}\nSmooth: {smooth}\nPaint: {paint:F4}";
          Visualization.Draw.AddText(line, "Terrain", text);
        }
      }
    }
  }
}