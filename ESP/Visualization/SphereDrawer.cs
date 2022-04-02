using System;
using UnityEngine;
namespace Visualization;
public partial class Draw {
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
  public static void UpdateSphere(MonoBehaviour parent, float radius) {
    var renderers = parent.GetComponentsInChildren<LineRenderer>();
    if (renderers.Length != 3) return;
    var width = renderers[0].widthMultiplier;
    UpdateArcX(renderers[0], Vector3.zero, radius, 360f, width);
    UpdateArcY(renderers[1], Vector3.zero, radius, 360f, width);
    UpdateArcZ(renderers[2], Vector3.zero, radius, 360f, width);
    UpdateSphereCollider(parent, radius - width / 2f);
  }
  ///<summary>Creates a renderer with a sphere (x, y and z profiles).</summary>
  public static GameObject DrawSphere(string tag, MonoBehaviour parent, float radius)
    => DrawSphere(tag, parent.gameObject, radius);

  ///<summary>Creates a renderer with a sphere (x, y and z profiles).</summary>
  public static GameObject DrawSphere(string tag, GameObject parent, float radius) {
    var obj = CreateObject(parent, tag);
    DrawArcX(CreateObject(obj, tag), Vector3.zero, radius, 360f);
    var width = GetLineWidth(tag);
    AddSphereCollider(obj, radius - width / 2f);
    DrawArcY(CreateObject(obj, tag), Vector3.zero, radius, 360f);
    DrawArcZ(CreateObject(obj, tag), Vector3.zero, radius, 360f);
    return obj;
  }
}
