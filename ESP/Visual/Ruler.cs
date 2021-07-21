using UnityEngine;

namespace ESP
{
  public class Ruler : MonoBehaviour
  {
    private static GameObject ruler = null;
    private static void Reset()
    {
      if (ruler) Destroy(ruler);
      ruler = null;
    }
    private static void Set(Vector3 position)
    {
      if (ruler) Reset();
      var obj = new GameObject();
      obj.layer = LayerMask.NameToLayer(Constants.TriggerLayer);
      obj.transform.position = position * 1.0f;
      ruler = obj;
      var line = Drawer.DrawSphere(obj, Settings.rulerRadius, Color.red, Settings.rulerRadius, "");
      Drawer.AddText(line, Format.Coordinates(position), "Ruler");
    }
    public static void Toggle(Vector3 position)
    {
      if (!ruler) Set(position);
      else Reset();
    }
    public static string GetText(Vector3 position)
    {
      if (Settings.rulerRadius == 0f) return "";
      if (ruler == null) return Format.String("J") + ": Set ruler point.";
      var delta = position - ruler.transform.position;
      var distXZ = Utils.DistanceXZ(position, ruler.transform.position);
      return Format.String("J") + ": Reset ruler, Distance (" + Format.Coordinates(delta) + "):" + Format.Float(delta.magnitude) + " m, XZ: " + Format.Float(distXZ) + " m";
    }
  }
}