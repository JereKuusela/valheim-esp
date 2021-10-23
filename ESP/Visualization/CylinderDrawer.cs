using UnityEngine;

namespace Visualization {
  public partial class Draw {
    ///<summary>Creates a renderer with a cylinder (vertical).</summary>
    public static GameObject DrawCylinder(string tag, MonoBehaviour parent, float radius) {
      var obj = CreateObject(parent.gameObject, tag);
      DrawArcY(CreateObject(obj, tag), Vector3.zero, radius, 360f);
      var width = GetLineWidth(tag);
      AddSphereCollider(obj, radius - width / 2f);
      var start = new Vector3(radius, -500f, 0);
      var end = new Vector3(radius, 500f, 0);
      var line = DrawLineSub(CreateObject(obj, tag), start, end);
      Draw.AddBoxCollider(line);
      start = new Vector3(-radius, -500f, 0);
      end = new Vector3(-radius, 500f, 0);
      line = DrawLineSub(CreateObject(obj, tag), start, end);
      Draw.AddBoxCollider(line);
      start = new Vector3(0, -500f, radius);
      end = new Vector3(0, 500f, radius);
      line = DrawLineSub(CreateObject(obj, tag), start, end);
      Draw.AddBoxCollider(line);
      start = new Vector3(0, -500f, -radius);
      end = new Vector3(0, 500f, -radius);
      line = DrawLineSub(CreateObject(obj, tag), start, end);
      Draw.AddBoxCollider(line);
      return obj;
    }
  }
}