using UnityEngine;

namespace ESP
{
  public partial class Drawer
  {
    public static void DrawConeX(GameObject obj, Vector3 position, float radius, float angle, Color color, float width)
    {
      var renderer = CreateRenderer(obj, color, width);
      var segments = GetArcSegmentsX(position, angle, radius - width / 2f);
      renderer.positionCount = segments.Length + 2;
      renderer.SetPosition(0, position);
      for (var i = 0; i < segments.Length; i++)
        renderer.SetPosition(i + 1, segments[i]);
      renderer.SetPosition(segments.Length + 1, position);
    }
    public static void DrawConeY(GameObject obj, Vector3 position, float radius, float angle, Color color, float width)
    {
      var renderer = CreateRenderer(obj, color, width);
      var segments = GetArcSegmentsY(position, angle, radius - width / 2f);
      renderer.positionCount = segments.Length + 2;
      renderer.SetPosition(0, position);
      for (var i = 0; i < segments.Length; i++)
        renderer.SetPosition(i + 1, segments[i]);
      renderer.SetPosition(segments.Length + 1, position);
    }
    public static void DrawConeZ(GameObject obj, Vector3 position, float radius, float angle, Color color, float width)
    {
      var renderer = CreateRenderer(obj, color, width);
      var segments = GetArcSegmentsZ(position, angle, radius - width / 2f);
      renderer.positionCount = segments.Length + 2;
      renderer.SetPosition(0, position);
      for (var i = 0; i < segments.Length; i++)
        renderer.SetPosition(i + 1, segments[i]);
      renderer.SetPosition(segments.Length + 1, position);
    }

    public static GameObject DrawCone(GameObject parent, Vector3 position, float radius, float angle, Color color, float width)
    {
      var obj = Drawer.CreateObject(parent);
      Drawer.DrawConeY(obj, position, radius, angle, color, width);
      Drawer.DrawConeX(obj, position, radius, angle, color, width);
      Drawer.AddMeshCollider(obj);
      return obj;
    }
  }
}