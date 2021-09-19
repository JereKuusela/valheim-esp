using System;
using UnityEngine;

namespace ESP {
  public partial class Drawer {
    ///<summary>Adds a collider to a sphere</summary>
    public static void AddSphereCollider(GameObject obj, float radius) {
      var renderers = obj.GetComponentsInChildren<LineRenderer>();
      Array.ForEach(renderers, renderer => {
        var collider = obj.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.center = Vector3.zero;
        collider.radius = radius;
      });
    }
    ///<summary>Updates the collider of a sphere</summary>
    public static void UpdateSphereCollider(MonoBehaviour obj, float radius) {
      var colliders = obj.GetComponentsInChildren<SphereCollider>();
      Array.ForEach(colliders, collider => {
        collider.radius = radius;
      });
    }
    ///<summary>Updates an existing sphere.</summary>
    public static void UpdateSphere(MonoBehaviour parent, float radius, float width) {
      var renderers = parent.GetComponentsInChildren<LineRenderer>();
      if (renderers.Length != 3) return;
      UpdateArcX(renderers[0], Vector3.zero, radius, 360f, width);
      UpdateArcY(renderers[1], Vector3.zero, radius, 360f, width);
      UpdateArcZ(renderers[2], Vector3.zero, radius, 360f, width);
      UpdateSphereCollider(parent, radius - width / 2f);
    }
    ///<summary>Creates a renderer with a sphere (x, y and z profiles).</summary>
    public static GameObject DrawSphere(MonoBehaviour parent, float radius, Color color, float width, string name)
      => DrawSphere(parent.gameObject, radius, color, width, name);

    ///<summary>Creates a renderer with a sphere (x, y and z profiles).</summary>
    public static GameObject DrawSphere(GameObject parent, float radius, Color color, float width, string name) {
      var obj = CreateObject(parent, name);
      DrawArcX(CreateObject(obj, name), Vector3.zero, radius, 360f, color, width);
      AddSphereCollider(obj, radius - width / 2f);
      DrawArcY(CreateObject(obj, name), Vector3.zero, radius, 360f, color, width);
      DrawArcZ(CreateObject(obj, name), Vector3.zero, radius, 360f, color, width);
      return obj;
    }
  }
}