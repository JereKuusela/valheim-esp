using UnityEngine;

namespace ESP
{
  public partial class Drawer
  {
    public static GameObject DrawLine(GameObject parent, Vector3 start, Vector3 end, Color color, float width)
    {
      var obj = CreateObject(parent);
      var renderer = CreateRenderer(obj, color, width);
      renderer.SetPosition(0, start);
      renderer.SetPosition(1, end);
      return obj;
    }
    public static GameObject DrawMarkerLine(GameObject parent, Vector3 start, Color color, float width)
    {
      var end = new Vector3(start.x, 500f, start.z);
      return DrawLine(parent, start, end, color, width);
    }
  }
}