
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Service;
using UnityEngine;
namespace Visualization;

[HarmonyPatch(typeof(Player), nameof(Player.UpdateHover))]
public class Player_AddHoverForVisuals
{

  /// <summary>Extra hover search for drawn objects if no other hover object.</summary>
  static void Postfix(ref GameObject ___m_hovering, ref GameObject ___m_hoveringCreature)
  {
    if (___m_hovering || ___m_hoveringCreature) return;
    var distance = 100f;
    var mask = LayerMask.GetMask(new[] { Draw.TriggerLayer });
    var hits = Physics.RaycastAll(GameCamera.instance.transform.position, GameCamera.instance.transform.forward, distance, mask);
    // Reverse search is used to find edge when inside colliders.
    var reverseHits = Physics.RaycastAll(GameCamera.instance.transform.position + GameCamera.instance.transform.forward * distance, -GameCamera.instance.transform.forward, distance, mask);
    hits = hits.AddRangeToArray(reverseHits);
    Array.Sort(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
    foreach (var hit in hits)
    {
      if (hit.collider.GetComponent<Visualization>() != null)
      {
        ___m_hovering = hit.collider.gameObject;
        return;
      }
    }
  }
}

/// <summary>Custom text that also shows the title.</summary>
public class StaticText : MonoBehaviour, Hoverable
{
  public string GetHoverText() => Format.String(title) + "\n" + text;
  public string GetHoverName() => title;
  public string title = "";
  public string text = "";
}
/// <summary>Custom component to allow finding visualizations more easily.</summary>
public class Visualization : MonoBehaviour
{
  public static Visualization[] Get(string tag) => [.. Visualizations.Where(v => v.Tag == tag)];
  public static IEnumerable<Visualization> Get() => Visualizations;
  public static void Remove(string tag)
  {
    foreach (var obj in Visualizations.Where(v => v.Tag == tag).ToArray())
    {
      Destroy(obj.gameObject);
    }
  }
  public static void Remove(GameObject obj, string tag)
  {
    var vis = obj.GetComponentsInChildren<Visualization>(true);
    foreach (var visualization in vis)
    {
      if (!visualization || visualization.Tag != tag) continue;
      Destroy(visualization.gameObject);
    }
  }
  public string Tag = "";
  public string? SubTag;

  private static readonly HashSet<Visualization> Visualizations = [];
  private static readonly HashSet<Visualization> FixedVisualizations = [];
  private Quaternion? FixedRotation;

  public void OnEnable()
  {
    Visualizations.Add(this);
  }

  public void OnDisable()
  {
    Visualizations.Remove(this);
    FixedVisualizations.Remove(this);
  }

  public static void SharedUpdate()
  {
    foreach (var visualization in FixedVisualizations)
    {
      if (!visualization) continue;
      visualization.transform.rotation = visualization.FixedRotation!.Value;
    }
  }
  public void OnDestroy()
  {
    Visualizations.Remove(this);
    FixedVisualizations.Remove(this);
  }

  public void SetFixed(Quaternion rotation)
  {
    FixedRotation = rotation;
    Visualizations.Add(this);
    FixedVisualizations.Add(this);
  }
}
