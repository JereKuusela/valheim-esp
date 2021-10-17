using Service;
using UnityEngine;
using Visualization;

namespace ESP {
  public class Ruler : MonoBehaviour {
    private static GameObject ruler = null;
    public static void Reset() {
      if (ruler) Destroy(ruler);
      ruler = null;
    }
    public static void Set(Vector3 position) {
      if (ruler) Reset();
      var obj = new GameObject();
      obj.layer = LayerMask.NameToLayer(Draw.TriggerLayer);
      obj.transform.position = position;
      ruler = obj;
      var line = Draw.DrawSphere("", obj, Settings.RulerRadius, Settings.RulerColor, Settings.RulerRadius);
      Draw.AddText(line, Format.Coordinates(position), "Ruler");
    }
    public static void Toggle(Vector3 position) {
      if (!ruler) Set(position);
      else Reset();
    }
    public static string GetText(Vector3 position) {
      if (Settings.RulerRadius == 0f) return "";
      if (ruler == null) return Format.String("J") + ": Set ruler point.";
      var delta = position - ruler.transform.position;
      var distXZ = Utils.DistanceXZ(position, ruler.transform.position);
      return Format.String("J") + ": Reset ruler, Distance (" + Format.Coordinates(delta) + "): " + Format.Float(delta.magnitude) + " m, XZ: " + Format.Float(distXZ) + " m";
    }
  }
}