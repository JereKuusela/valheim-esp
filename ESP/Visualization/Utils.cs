
using System;
using HarmonyLib;
using Service;
using UnityEngine;
namespace Visualization;
public class Utils {
  public static Visualization[] GetVisualizations() => Resources.FindObjectsOfTypeAll<Visualization>();
}
[HarmonyPatch(typeof(Player), nameof(Player.UpdateHover))]
public class Player_AddHoverForVisuals {

  /// <summary>Extra hover search for drawn objects if no other hover object.</summary>
  static void Postfix(ref GameObject ___m_hovering, ref GameObject ___m_hoveringCreature) {
    if (___m_hovering || ___m_hoveringCreature) return;
    var distance = 50f;
    var mask = LayerMask.GetMask(new[] { Draw.TriggerLayer });
    var hits = Physics.RaycastAll(GameCamera.instance.transform.position, GameCamera.instance.transform.forward, distance, mask);
    // Reverse search is used to find edge when inside colliders.
    var reverseHits = Physics.RaycastAll(GameCamera.instance.transform.position + GameCamera.instance.transform.forward * distance, -GameCamera.instance.transform.forward, distance, mask);
    hits = hits.AddRangeToArray(reverseHits);
    Array.Sort<RaycastHit>(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
    foreach (var hit in hits) {
      if (hit.collider.GetComponent<Hoverable>() != null) {
        ___m_hovering = hit.collider.gameObject;
        return;
      }
    }
  }
}

/// <summary>Custom text that also shows the title.</summary>
public class StaticText : MonoBehaviour, Hoverable {
  public string GetHoverText() => Format.String(title) + "\n" + text;
  public string GetHoverName() => title;
  public string title = "";
  public string text = "";
}
/// <summary>Custom component to allow finding visualizations more easily.</summary>
public class Visualization : MonoBehaviour {
  public string Tag = "";
}
