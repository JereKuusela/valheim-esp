using System;
using HarmonyLib;
using UnityEngine;
using System.Linq;

namespace ESP
{
  [HarmonyPatch(typeof(Player), "UpdateHover")]
  public class Player_AddHoverForVisuals
  {

    /// <summary>Extra hover search for drawn objects if no other hover object.</summary>
    public static void Postfix(ref GameObject ___m_hovering, ref GameObject ___m_hoveringCreature)
    {
      if (___m_hovering || ___m_hoveringCreature) return;
      var distance = 50f;
      var mask = LayerMask.GetMask(new string[] { Constants.TriggerLayer });
      var hits = Physics.RaycastAll(GameCamera.instance.transform.position, GameCamera.instance.transform.forward, distance, mask);
      // Reverse search is used to find edge when inside colliders.
      var reverseHits = Physics.RaycastAll(GameCamera.instance.transform.position + GameCamera.instance.transform.forward * distance, -GameCamera.instance.transform.forward, distance, mask);
      hits = hits.AddRangeToArray(reverseHits);
      Array.Sort<RaycastHit>(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
      foreach (var hit in hits)
      {
        if (hit.collider.GetComponent<Hoverable>() != null)
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
    public string title;
    public string text;
  }
  /// <summary>Provides a way to distinguish renderers.</summary>
  public class CustomTag : MonoBehaviour
  {
    public string customTag;
  }

  public partial class Drawer : Component
  {
    public const string OTHER = "ESP_Other";
    public const string ZONE = "ESP_Zone";
    public const string CREATURE = "ESP_Creature";
    ///<summary>Setting for other visual visibility. Forced to off for non-admins on servers.</summary>
    public static bool showOthers
    {
      get => Settings.showOthers && Cheats.IsAdmin;
      set
      {
        if (value)
          Cheats.CheckAdmin();
        Settings.configShowOthers.Value = value;
      }
    }
    ///<summary>Setting for creature visual visibility. Forced to off for non-admins on servers.</summary>
    public static bool showCreatures
    {
      get => Settings.showCreatures && Cheats.IsAdmin;
      set
      {
        if (value)
          Cheats.CheckAdmin();
        Settings.configShowCreatures.Value = value;
      }
    }
    ///<summary>Setting for zone visual visibility. Forced to off for non-admins on servers.</summary>
    public static bool showZones
    {
      get => Settings.showZones && Cheats.IsAdmin;
      set
      {
        if (value)
          Cheats.CheckAdmin();
        Settings.configShowZones.Value = value;
      }
    }
    ///<summary>Toggles visibility of other visuals.</summary>
    public static void ToggleOtherVisibility()
    {
      showOthers = !showOthers;
      CheckVisibility(OTHER);
    }
    ///<summary>Toggles visibility of zone visuals.</summary>
    public static void ToggleZoneVisibility()
    {
      showZones = !showZones;
      CheckVisibility(ZONE);
    }
    ///<summary>Toggles visibility of creature visuals.</summary>
    public static void ToggleCreatureVisibility()
    {
      showCreatures = !showCreatures;
      CheckVisibility(CREATURE);
    }
    ///<summary>Checks visibility of a given visual type.</summary>
    private static void CheckVisibility(string name)
    {
      var activate = IsShown(name);
      foreach (var gameObj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
      {
        if (gameObj.name == name && gameObj.activeSelf != activate)
          gameObj.SetActive(activate);
      }
    }
    ///<summary>Checks visibility of all visuals.</summary>
    public static void CheckVisibility()
    {
      CheckVisibility(OTHER);
      CheckVisibility(CREATURE);
      CheckVisibility(ZONE);
    }
    ///<summary>Returns whether a given visual type is shown.</summary>
    private static bool IsShown(string name)
    {
      if (name == ZONE) return showZones;
      if (name == CREATURE) return showCreatures;
      return showOthers;
    }
    ///<summary>Creates the base object for drawing.</summary>
    private static GameObject CreateObject(GameObject parent, string name, bool fixRotation = false)
    {
      var obj = new GameObject();
      obj.layer = LayerMask.NameToLayer(Constants.TriggerLayer);
      obj.name = name;
      obj.transform.parent = parent.transform;
      obj.transform.localPosition = Vector3.zero;
      if (!fixRotation)
        obj.transform.localRotation = Quaternion.identity;
      obj.SetActive(IsShown(name));
      return obj;
    }
    ///<summary>Creates a transform that rotates a forward line to a given direction.</summary>
    private static GameObject CreateLineRotater(GameObject parent, Vector3 start, Vector3 end)
    {
      var obj = new GameObject();
      obj.layer = LayerMask.NameToLayer(Constants.TriggerLayer);
      obj.transform.parent = parent.transform;
      obj.transform.localPosition = start;
      obj.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, end - start);
      return obj;
    }
    ///<summary>Creates the line renderer object.</summary>
    private static LineRenderer CreateRenderer(GameObject obj, Color color, float width)
    {
      var component = obj.AddComponent<LineRenderer>();
      component.useWorldSpace = false;
      component.material = new Material(Shader.Find("Standard TwoSided"));
      component.material.SetColor("_Color", color);
      component.widthMultiplier = width;
      return component;
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
        var mesh = new Mesh();
        renderer.BakeMesh(mesh);
        collider.sharedMesh = mesh;
      });
    }
    ///<summary>Adds a self-updating text to a given object.</summary>
    public static void AddText(GameObject obj)
    {
      obj.AddComponent<CustomHoverText>();
    }
    ///<summary>Adds a text to a given object (uses the in-game text).</summary>
    public static void AddText(GameObject obj, string text)
    {
      obj.AddComponent<HoverText>().m_text = text;
    }
    ///<summary>Adds a custom text with a title and text to a given object.</summary>
    public static void AddText(GameObject obj, string title, string text)
    {
      var component = obj.AddComponent<StaticText>();
      component.text = text;
      component.title = title;
    }
    ///<summary>Adds a tag to a given renderer so it can be found later.</summary>
    public static void AddTag(GameObject obj, string tag)
    {
      obj.AddComponent<CustomTag>().customTag = tag;
    }
    ///<summary>Returns renderers with a given tag.</summary>
    public static LineRenderer[] GetRenderers(MonoBehaviour obj, string tag)
    {
      return obj.GetComponentsInChildren<LineRenderer>().Where(renderer =>
      {
        var customTag = renderer.GetComponent<CustomTag>();
        return customTag != null && customTag.customTag == tag;
      }).ToArray();
    }
  }
}