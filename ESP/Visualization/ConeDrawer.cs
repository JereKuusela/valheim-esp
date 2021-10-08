using UnityEngine;

namespace Visualization {
  public partial class Draw {
    ///<summary>Creates a cone profile on the X plane.</summary>
    public static void DrawConeX(GameObject obj, Vector3 position, float radius, float angle, Color color, float width) {
      var renderer = CreateRenderer(obj, color, width);
      var segments = GetArcSegmentsX(position, angle, radius - width / 2f);
      renderer.positionCount = segments.Length + 2;
      renderer.SetPosition(0, position);
      for (var i = 0; i < segments.Length; i++)
        renderer.SetPosition(i + 1, segments[i]);
      renderer.SetPosition(segments.Length + 1, position);
    }
    ///<summary>Creates a cone profile on the Z plane.</summary>
    public static void DrawConeY(GameObject obj, Vector3 position, float radius, float angle, Color color, float width) {
      var renderer = CreateRenderer(obj, color, width);
      var segments = GetArcSegmentsY(position, angle, radius - width / 2f);
      renderer.positionCount = segments.Length + 2;
      renderer.SetPosition(0, position);
      for (var i = 0; i < segments.Length; i++)
        renderer.SetPosition(i + 1, segments[i]);
      renderer.SetPosition(segments.Length + 1, position);
    }
    ///<summary>Creates a cone profile on the Z plane.</summary>
    public static void DrawConeZ(GameObject obj, Vector3 position, float radius, float angle, Color color, float width) {
      var renderer = CreateRenderer(obj, color, width);
      var segments = GetArcSegmentsZ(position, angle, radius - width / 2f);
      renderer.positionCount = segments.Length + 2;
      renderer.SetPosition(0, position);
      for (var i = 0; i < segments.Length; i++)
        renderer.SetPosition(i + 1, segments[i]);
      renderer.SetPosition(segments.Length + 1, position);
    }
    ///<summary>Creates a renderer with a cone (vertical and horizontal profile).</summary>
    public static GameObject DrawCone(string tag, MonoBehaviour parent, Vector3 position, float radius, float angle, Color color, float width) {
      var obj = Draw.CreateObject(parent.gameObject, tag);
      Draw.DrawConeY(Draw.CreateObject(obj), position, radius, angle, color, width);
      Draw.DrawConeX(Draw.CreateObject(obj), position, radius, angle, color, width);
      Draw.AddMeshCollider(obj);
      return obj;
    }
  }
}