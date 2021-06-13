using UnityEngine;

namespace ESP
{
  public partial class Drawer
  {
    public static void UpdateSphere(GameObject parent, float radius, float width, string text = "")
    {
      UpdateText(parent, text);
      var renderers = parent.GetComponentsInChildren<LineRenderer>();
      if (renderers.Length != 3) return;
      UpdateArcX(renderers[0], Vector3.zero, radius, 360f, width);
      UpdateArcY(renderers[1], Vector3.zero, radius, 360f, width);
      UpdateArcZ(renderers[2], Vector3.zero, radius, 360f, width);
    }
    public static GameObject DrawSphere(GameObject parent, float radius, Color color, float width)
    {
      var obj = CreateObject(parent);
      DrawArcX(parent, Vector3.zero, radius, 360f, color, width);
      DrawArcY(parent, Vector3.zero, radius, 360f, color, width);
      DrawArcZ(parent, Vector3.zero, radius, 360f, color, width);
      Drawer.AddSphereCollider(obj, radius - width / 2f);
      return obj;
    }
  }
}