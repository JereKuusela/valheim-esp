using UnityEngine;

namespace ESP {
  public partial class Drawer {
    ///<summary>Creates a renderer with a cylinder (vertical).</summary>
    public static GameObject DrawCylinder(MonoBehaviour parent, float radius, Color color, float width, string name) {
      var obj = CreateObject(parent.gameObject, name);
      DrawArcY(CreateObject(obj, name), Vector3.zero, radius, 360f, color, width);
      AddSphereCollider(obj, radius - width / 2f);
      var start = new Vector3(radius, -500f, 0);
      var end = new Vector3(radius, 500f, 0);
      var line = DrawLineSub(CreateObject(obj, name), start, end, color, width);
      Drawer.AddBoxCollider(line);
      start = new Vector3(-radius, -500f, 0);
      end = new Vector3(-radius, 500f, 0);
      line = DrawLineSub(CreateObject(obj, name), start, end, color, width);
      Drawer.AddBoxCollider(line);
      start = new Vector3(0, -500f, radius);
      end = new Vector3(0, 500f, radius);
      line = DrawLineSub(CreateObject(obj, name), start, end, color, width);
      Drawer.AddBoxCollider(line);
      start = new Vector3(0, -500f, -radius);
      end = new Vector3(0, 500f, -radius);
      line = DrawLineSub(CreateObject(obj, name), start, end, color, width);
      Drawer.AddBoxCollider(line);
      return obj;
    }
  }
}