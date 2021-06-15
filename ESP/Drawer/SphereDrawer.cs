using UnityEngine;
using System;

namespace ESP
{
  public partial class Drawer
  {

    public static void AddSphereCollider(GameObject obj, float radius)
    {
      var renderers = obj.GetComponentsInChildren<LineRenderer>();
      Array.ForEach(renderers, renderer =>
      {
        var collider = obj.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.center = Vector3.zero;
        collider.radius = radius;
      });
    }

    public static void UpdateSphereCollider(GameObject obj, float radius)
    {
      var colliders = obj.GetComponentsInChildren<SphereCollider>();
      Array.ForEach(colliders, collider =>
      {
        collider.radius = radius;
      });
    }
    public static void UpdateSphere(GameObject parent, float radius, float width)
    {
      var renderers = parent.GetComponentsInChildren<LineRenderer>();
      if (renderers.Length != 3) return;
      UpdateArcX(renderers[0], Vector3.zero, radius, 360f, width);
      UpdateArcY(renderers[1], Vector3.zero, radius, 360f, width);
      UpdateArcZ(renderers[2], Vector3.zero, radius, 360f, width);
      UpdateSphereCollider(parent, radius - width / 2f);
    }
    public static GameObject DrawSphere(GameObject parent, float radius, Color color, float width)
    {
      var obj = CreateObject(parent);
      DrawArcX(CreateObject(obj), Vector3.zero, radius, 360f, color, width);
      AddSphereCollider(obj, radius - width / 2f);
      DrawArcY(CreateObject(obj), Vector3.zero, radius, 360f, color, width);
      DrawArcZ(CreateObject(obj), Vector3.zero, radius, 360f, color, width);
      return obj;
    }
  }
}