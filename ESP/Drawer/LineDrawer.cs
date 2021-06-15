using UnityEngine;
using System;

namespace ESP
{
  public partial class Drawer
  {
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
        collider.size = (end - start) + 2 * new Vector3(width, width, width);
      });
    }
    private static GameObject DrawLine(GameObject parent, Vector3 start, Vector3 end, Color color, float width, string name)
    {
      var obj = CreateObject(parent, name);
      var renderer = CreateRenderer(obj, color, width);
      renderer.SetPosition(0, start);
      renderer.SetPosition(1, end);
      return obj;
    }
    public static GameObject DrawMarkerLine(GameObject parent, Vector3 start, Color color, float width, string name)
    {
      var end = new Vector3(start.x, 500f, start.z);
      var obj = DrawLine(parent, start, end, color, width, name);
      Drawer.AddBoxCollider(obj);
      return obj;
    }
  }
}