using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch(typeof(Player), "UpdateHover")]
  public class Player_UpdateHover
  {
    // Extra hover search for drawn objects if no other hover object.
    public static void Postfix(ref GameObject ___m_hovering, ref GameObject ___m_hoveringCreature)
    {
      if (___m_hovering || ___m_hoveringCreature) return;
      var distance = 50f;
      var mask = LayerMask.GetMask(new string[] { "character_trigger" });
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

  public class StaticText : MonoBehaviour, Hoverable
  {

    public string GetHoverText() => Format.String(title) + "\n" + text;
    public string GetHoverName() => title;
    public string title;
    public string text;
  }

  public partial class Drawer : Component
  {
    public const string OTHER = "ESP_Other";
    public const string ZONE = "ESP_Zone";
    public const string CREATURE = "ESP_Creature";
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
    public static void ToggleOtherVisibility()
    {
      showOthers = !showOthers;
      CheckVisibility(OTHER);
    }
    public static void ToggleZoneVisibility()
    {
      showZones = !showZones;
      CheckVisibility(ZONE);
    }
    public static void ToggleCreatureVisibility()
    {
      showCreatures = !showCreatures;
      CheckVisibility(CREATURE);
    }
    private static void CheckVisibility(string name)
    {
      var activate = IsShown(name);
      foreach (var gameObj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
      {
        if (gameObj.name == name && gameObj.activeSelf != activate)
          gameObj.SetActive(activate);
      }
    }
    public static void CheckVisibility()
    {
      CheckVisibility(OTHER);
      CheckVisibility(CREATURE);
      CheckVisibility(ZONE);
    }
    private static bool IsShown(string name)
    {
      if (name == ZONE) return showZones;
      if (name == CREATURE) return showCreatures;
      return showOthers;
    }
    private static GameObject CreateObject(GameObject parent, string name)
    {
      var obj = new GameObject();
      obj.layer = LayerMask.NameToLayer("character_trigger");
      obj.name = name;
      obj.transform.parent = parent.transform;
      obj.transform.localPosition = Vector3.zero;
      obj.transform.localRotation = Quaternion.identity;
      obj.SetActive(IsShown(name));
      return obj;
    }
    private static LineRenderer CreateRenderer(GameObject obj, Color color, float width)
    {
      var component = obj.AddComponent<LineRenderer>();
      component.useWorldSpace = false;
      component.material = new Material(Shader.Find("Standard TwoSided"));
      component.material.SetColor("_Color", color);
      component.widthMultiplier = width;
      return component;
    }
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
    public static void AddText(GameObject obj, string text = "")
    {
      obj.AddComponent<HoverText>().m_text = text;
    }
    public static void AddText(GameObject obj, string title, string text)
    {
      var component = obj.AddComponent<StaticText>();
      component.text = text;
      component.title = title;
    }
  }
}