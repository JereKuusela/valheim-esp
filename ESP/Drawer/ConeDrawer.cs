using UnityEngine;

namespace ESP
{
  public partial class Drawer
  {
    ///<summary>Creates a cone profile on the X plane.</summary>
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
    ///<summary>Creates a cone profile on the Z plane.</summary>
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
    ///<summary>Creates a cone profile on the Z plane.</summary>
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
    ///<summary>Creates a renderer with a cone (vertical and horizontal profile).</summary>
    public static GameObject DrawCone(MonoBehaviour parent, Vector3 position, float radius, float angle, Color color, float width, string name)
    {
      var obj = Drawer.CreateObject(parent.gameObject, name);
      Drawer.DrawConeY(Drawer.CreateObject(obj, name), position, radius, angle, color, width);
      Drawer.DrawConeX(Drawer.CreateObject(obj, name), position, radius, angle, color, width);
      Drawer.AddMeshCollider(obj);
      return obj;
    }
  }
}