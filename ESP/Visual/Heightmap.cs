using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace ESP;
public partial class Visual
{
  private static List<GameObject> renderers = new List<GameObject>();
  public static void DrawHeightmap(Vector3 pos, float radius)
  {
    List<Heightmap> hms = new();
    Heightmap.FindHeightmap(pos, radius, hms);
    foreach (var renderer in renderers)
      UnityEngine.Object.Destroy(renderer);
    renderers.Clear();
    foreach (var hm in hms)
    {
      var size = (hm.m_width + 1);
      var tc = TerrainComp.FindTerrainCompiler(hm.transform.position);
      for (var i = 0; i < size; i++)
      {
        for (var j = 0; j < size; j++)
        {
          var vector = hm.transform.position + hm.CalcVertex(j, i);
          var vertex = hm.CalcVertex(j, i);
          if (Utils.DistanceXZ(vector, pos) > radius) continue;
          var normal = hm.CalcNormal(j, i);
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
          var text = $"Index: {num2}, i: {i}, j: {j}\nHeight: {height}\nLevel: {level}\nSmooth: {smooth}\nPaint: {paint.ToString("F4")}";
          Visualization.Draw.AddText(line, "Terrain", text);
        }
      }
    }
  }
}