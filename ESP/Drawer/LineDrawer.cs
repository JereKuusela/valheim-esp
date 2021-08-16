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

    private static void AddBoxCollider(GameObject obj, Vector3 center, Vector3 size)
    {
      var collider = obj.AddComponent<BoxCollider>();
      collider.isTrigger = true;
      collider.center = center;
      collider.size = size;
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
    ///<summary>Creates a renderer with a vertical line (relative to the object).</summary>
    public static GameObject DrawBox(MonoBehaviour parent, Color color, float width, string name, Vector3 center, Vector3 extents)
    {
      var corners = new Vector3[] {
        new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z),
        new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z),
        new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z),
        new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z),
        new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z),
        new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z),
        new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z),
        new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z),
      };
      var obj = CreateObject(parent.gameObject, name, true);
      for (var i = 0; i < corners.Length; i++)
      {
        var start = corners[i];
        for (var j = i + 1; j < corners.Length; j++)
        {
          var end = corners[j];
          var same = 0;
          if (start.x == end.x) same++;
          if (start.y == end.y) same++;
          if (start.z == end.z) same++;
          if (same != 2) continue;
          DrawLineSub(CreateObject(obj, name), corners[i], corners[j], color, width);
        }
      }
      Drawer.AddBoxCollider(obj, center, extents * 2.0f);
      return obj;
    }
  }
}