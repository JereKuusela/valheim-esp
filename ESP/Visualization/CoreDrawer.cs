using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Visualization;
public partial class Draw : Component
{
  public const string TriggerLayer = "character_trigger";

  ///<summary>Creates the base object for drawing.</summary>
  private static GameObject CreateObject(GameObject parent, string tag, Quaternion? fixedRotation = null)
  {
    GameObject obj = new();
    obj.layer = LayerMask.NameToLayer(TriggerLayer);
    obj.transform.parent = parent.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
    if (tag != "")
    {
      obj.name = tag;
      var visual = obj.AddComponent<Visualization>();
      visual.Tag = tag;
      visual.FixedRotation = fixedRotation;
      obj.SetActive(Visibility.IsTag(tag));
    }
    return obj;
  }
  ///<summary>Creates the line renderer object.</summary>
  private static LineRenderer CreateRenderer(GameObject obj)
  {
    var renderer = obj.AddComponent<LineRenderer>();
    renderer.useWorldSpace = false;
    Material material = new(Shader.Find("Particles/Standard Unlit2"));
    material.SetColor("_Color", GetColor(obj.name));
    material.SetFloat("_BlendOp", (float)UnityEngine.Rendering.BlendOp.Subtract);
    Texture2D texture = new(1, 1);
    texture.SetPixel(0, 0, Color.gray);
    material.SetTexture("_MainTex", texture);
    renderer.material = material;
    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    renderer.widthMultiplier = GetLineWidth(obj.name);
    return renderer;
  }
  ///<summary>Changes object color.</summary>
  private static void ChangeColor(GameObject obj)
  {
    var color = GetColor(obj.name);
    var renderer = obj.GetComponent<LineRenderer>();
    if (renderer) renderer.material.SetColor("_Color", color);
  }
  ///<summary>Changes object line width.</summary>
  private static void ChangeLineWidth(GameObject obj)
  {
    var width = GetLineWidth(obj.name);
    var renderer = obj.GetComponent<LineRenderer>();
    if (renderer) renderer.widthMultiplier = width;
  }
  ///<summary>Adds an advanced collider to a complex shape (like cone).</summary>
  public static void AddMeshCollider(GameObject obj)
  {
    var renderers = obj.GetComponentsInChildren<LineRenderer>();
    Array.ForEach(renderers, renderer =>
    {
      var collider = obj.AddComponent<MeshCollider>();
      collider.convex = true;
      collider.isTrigger = true;
      Mesh mesh = new();
      renderer.BakeMesh(mesh);
      collider.sharedMesh = mesh;
    });
  }
  ///<summary>Adds a custom text with a title and text to a given object.</summary>
  public static void AddText(GameObject obj, string title, string text)
  {
    var component = obj.AddComponent<StaticText>();
    component.text = text;
    component.title = title;
  }
  ///<summary>Returns renderers with a given tag.</summary>
  public static LineRenderer[] GetRenderers(MonoBehaviour obj, string tag) => GetRenderers(obj, new[] { tag });
  ///<summary>Returns renderers with a given tag.</summary>
  public static LineRenderer[] GetRenderers(MonoBehaviour obj, string[] tags)
  {
    var set = tags.ToHashSet();
    return obj.GetComponentsInChildren<LineRenderer>(true).Where(renderer =>
    {
      var visualization = renderer.GetComponent<Visualization>();
      if (!visualization) return false;
      return set.Contains(visualization.Tag);
    }).ToArray();
  }
  private static Dictionary<string, Color> colors = new();
  public static Color GetColor(string tag) => colors.ContainsKey(tag) ? colors[tag] : Color.white;
  ///<summary>Sets colors to visuals with a given tag.</summary>
  public static void SetColor(string tag, Color color)
  {
    colors[tag] = color;
    foreach (var obj in Utils.GetVisualizations())
    {
      if (obj.Tag == tag) ChangeColor(obj.gameObject);
    }
  }
  private static Dictionary<string, int> lineWidths = new();
  public static float GetLineWidth(string tag)
  {
    var width = Math.Max(1, lineWidths.ContainsKey(tag) ? lineWidths[tag] : 0);
    return (float)width / 100f;
  }
  ///<summary>Sets line width to visuals with a given tag.</summary>
  public static void SetLineWidth(string tag, int width)
  {
    lineWidths[tag] = width;
    foreach (var obj in Utils.GetVisualizations())
    {
      if (obj.Tag == tag) ChangeLineWidth(obj.gameObject);
    }
  }
}
