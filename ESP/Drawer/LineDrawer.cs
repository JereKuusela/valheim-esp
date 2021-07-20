using UnityEngine;
using System;

namespace ESP
{
  public partial class Drawer
  {
    ///<summary>Adds a box collider to a given line.</summary>
    private static void AddBoxCollider(GameObject obj)
    {
      var renderers = obj.GetComponentsInChildren<LineRenderer>();
      Array.ForEach(renderers, renderer =>
      {
        var start = renderer.GetPosition(0);
        var end = renderer.GetPosition(renderer.positionCount - 1);
        var width = renderer.widthMultiplier;
        var collider = obj.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.center = start + (end - start) / 2;
        var size = (end - start);
        size.x = Math.Abs(size.x);
        size.y = Math.Abs(size.y);
        size.z = Math.Abs(size.z);
        collider.size = size + 2 * new Vector3(width, width, width);
      });
    }
    ///<summary>Creates a line.</summary>
    private static GameObject DrawLineSub(GameObject obj, Vector3 start, Vector3 end, Color color, float width)
    {
      var renderer = CreateRenderer(obj, color, width);
      renderer.SetPosition(0, start);
      renderer.SetPosition(1, end);
      return obj;
    }
    ///<summary>Creates a renderer with a line that doesn't rotate with the object.</summary>
    public static GameObject DrawLineWithFixedRotation(MonoBehaviour parent, Vector3 start, Vector3 end, Color color, float width, string name)
    {
      // Box colliders don't work with non-perpendicular lines so the line must be rotated from a forward line.
      var parentObj = CreateLineRotater(CreateObject(parent.gameObject, name, true), start, end);
      var forwardEnd = Vector3.forward * (end - start).magnitude;
      var obj = DrawLineSub(parentObj, Vector3.zero, forwardEnd, color, width);
      Drawer.AddBoxCollider(obj);
      return obj;
    }
    ///<summary>Creates a renderer with a vertical line (relative to the object) starting from the object center.</summary>
    public static GameObject DrawMarkerLine(MonoBehaviour parent, Color color, float width, string name) => DrawMarkerLine(parent, color, width, name, Vector3.zero);

    ///<summary>Creates a renderer with a vertical line (relative to the object).</summary>
    public static GameObject DrawMarkerLine(MonoBehaviour parent, Color color, float width, string name, Vector3 start)
    {
      var end = new Vector3(start.x, 500f, start.z);
      var obj = DrawLineSub(CreateObject(parent.gameObject, name, true), start, end, color, width);
      Drawer.AddBoxCollider(obj);
      return obj;
    }
  }
}