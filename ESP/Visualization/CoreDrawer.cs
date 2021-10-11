using System;
using System.Linq;
using UnityEngine;

namespace Visualization {
  public partial class Draw : Component {
    public const string TriggerLayer = "character_trigger";

    ///<summary>Creates the base object for drawing.</summary>
    private static GameObject CreateObject(GameObject parent, string tag = "", bool fixRotation = false) {
      var obj = new GameObject();
      obj.layer = LayerMask.NameToLayer(TriggerLayer);
      obj.transform.parent = parent.transform;
      obj.transform.localPosition = Vector3.zero;
      if (!fixRotation)
        obj.transform.localRotation = Quaternion.identity;
      if (tag != "") {
        obj.name = tag;
        obj.AddComponent<Visualization>().Tag = tag;
        obj.SetActive(Visibility.IsTag(tag));
      }
      return obj;
    }
    ///<summary>Creates a transform that rotates a forward line to a given direction.</summary>
    private static GameObject CreateLineRotater(GameObject parent, Vector3 start, Vector3 end) {
      var obj = new GameObject();
      obj.layer = LayerMask.NameToLayer(TriggerLayer);
      obj.transform.parent = parent.transform;
      obj.transform.localPosition = start;
      obj.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, end - start);
      return obj;
    }
    ///<summary>Creates the line renderer object.</summary>
    private static LineRenderer CreateRenderer(GameObject obj, Color color, float width) {
      var renderer = obj.AddComponent<LineRenderer>();
      renderer.useWorldSpace = false;
      var material = new Material(Shader.Find("Particles/Standard Unlit"));
      material.SetColor("_Color", color);
      material.SetFloat("_BlendOp", (float)UnityEngine.Rendering.BlendOp.Subtract);
      var texture = new Texture2D(1, 1);
      texture.SetPixel(0, 0, Color.gray);
      material.SetTexture("_MainTex", texture);
      renderer.material = material;
      renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
      renderer.widthMultiplier = width;
      return renderer;
    }
    ///<summary>Changes object color.</summary>
    private static void ChangeColor(GameObject obj, Color color) {
      foreach (var renderer in obj.GetComponentsInChildren<LineRenderer>(true))
        renderer.material.SetColor("_Color", color);
    }
    ///<summary>Changes object line width.</summary>
    private static void ChangeLineWidth(GameObject obj, float width) {
      foreach (var renderer in obj.GetComponentsInChildren<LineRenderer>(true))
        renderer.widthMultiplier = width;
    }
    ///<summary>Adds an advanced collider to a complex shape (like cone).</summary>
    public static void AddMeshCollider(GameObject obj) {
      var renderers = obj.GetComponentsInChildren<LineRenderer>();
      Array.ForEach(renderers, renderer => {
        var collider = obj.AddComponent<MeshCollider>();
        collider.convex = true;
        collider.isTrigger = true;
        var mesh = new Mesh();
        renderer.BakeMesh(mesh);
        collider.sharedMesh = mesh;
      });
    }
    ///<summary>Adds a custom text with a title and text to a given object.</summary>
    public static void AddText(GameObject obj, string title, string text) {
      var component = obj.AddComponent<StaticText>();
      component.text = text;
      component.title = title;
    }
    ///<summary>Returns renderers with a given tag.</summary>
    public static LineRenderer[] GetRenderers(MonoBehaviour obj, string tag) {
      return obj.GetComponentsInChildren<LineRenderer>(true).Where(renderer => {
        var visualization = renderer.GetComponent<Visualization>();
        return visualization != null && visualization.Tag == tag;
      }).ToArray();
    }
    ///<summary>Sets colors to visuals with a given tag.</summary>
    public static void SetColor(string tag, Color color) {
      foreach (var obj in Utils.GetVisualizations()) {
        if (obj.Tag == tag) ChangeColor(obj.gameObject, color);
      }
    }
    ///<summary>Sets line width to visuals with a given tag.</summary>
    public static void SetLineWidth(string tag, float width) {
      foreach (var obj in Utils.GetVisualizations()) {
        if (obj.Tag == tag) ChangeLineWidth(obj.gameObject, width);
      }
    }
  }
}