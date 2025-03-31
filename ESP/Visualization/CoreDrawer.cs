using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Visualization;
public partial class Draw
{
  public const string TriggerLayer = "character_trigger";

  private static readonly Texture2D Texture = new(1, 1);

  private static Shader LineShader => lineShader
    ??= Resources.FindObjectsOfTypeAll<Shader>().FirstOrDefault(shader => shader.name == shaderName) ?? Resources.FindObjectsOfTypeAll<Shader>().FirstOrDefault(shader => shader.name == "Sprites/Default") ?? throw new Exception("Shader not found.");
  private static Shader? lineShader;
  private static string shaderName = "Sprites/Default";
  public static void SetShader(string name)
  {
    shaderName = name;
    lineShader = null;
    materials.Clear();
    foreach (var obj in Utils.GetVisualizations()) ChangeColor(obj.gameObject);
  }
  public static void Init()
  {
    Texture.SetPixel(0, 0, Color.gray);
  }

  ///<summary>Creates the base object for drawing.</summary>
  private static GameObject CreateObject(GameObject parent, string tag, Quaternion? fixedRotation = null)
  {
    GameObject obj = new()
    {
      layer = LayerMask.NameToLayer(TriggerLayer)
    };
    obj.transform.parent = parent.transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
    if (tag != "")
    {
      obj.name = tag;
      var visual = obj.AddComponent<Visualization>();
      visual.Tag = tag;
      if (fixedRotation.HasValue)
        visual.SetFixed(fixedRotation.Value);
      obj.SetActive(Visibility.IsTag(tag));
    }
    return obj;
  }

  private static readonly Dictionary<Color, Material> materials = [];
  private static Material GetMaterial(Color color)
  {
    if (materials.ContainsKey(color)) return materials[color];
    var material = new Material(LineShader);
    material.SetColor("_Color", color);
    material.SetFloat("_BlendOp", (float)UnityEngine.Rendering.BlendOp.Subtract);
    material.SetTexture("_MainTex", Texture);
    materials[color] = material;
    return material;
  }

  ///<summary>Creates the line renderer object.</summary>
  private static LineRenderer CreateRenderer(GameObject obj)
  {
    var renderer = obj.AddComponent<LineRenderer>();
    renderer.useWorldSpace = false;
    renderer.sharedMaterial = GetMaterial(GetColor(obj.name));
    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    renderer.widthMultiplier = GetLineWidth(obj.name);
    return renderer;
  }

  ///<summary>Changes object color.</summary>
  private static void ChangeColor(GameObject obj)
  {
    var renderer = obj.GetComponent<LineRenderer>();
    if (renderer) renderer.sharedMaterial = GetMaterial(GetColor(obj.name));
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
  private static readonly Dictionary<string, Color> colors = [];
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
  private static readonly Dictionary<string, int> lineWidths = [];
  public static float GetLineWidth(string tag)
  {
    var width = Math.Max(1, lineWidths.ContainsKey(tag) ? lineWidths[tag] : 0);
    return width / 100f;
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
