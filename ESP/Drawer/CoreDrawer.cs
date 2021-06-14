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

    public string GetHoverText() => TextUtils.String(title) + "\n" + text;
    public string GetHoverName() => title;
    public string title;
    public string text;
  }

  public partial class Drawer : Component
  {
    private static bool Shown = Settings.showVisualization;
    public static void ToggleVisibility()
    {
      Shown = !Shown;
      foreach (var gameObj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
      {
        if (gameObj.name == "ESP")
          gameObj.SetActive(Shown);
      }
    }
    public static GameObject CreateObject(GameObject parent)
    {
      var obj = new GameObject();
      obj.layer = LayerMask.NameToLayer("character_trigger");
      obj.name = "ESP";
      obj.transform.parent = parent.transform;
      obj.transform.localPosition = Vector3.zero;
      obj.transform.localRotation = Quaternion.identity;
      obj.SetActive(Shown);
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
      var renderers = obj.GetComponents<LineRenderer>();
      Array.ForEach(renderers, renderer =>
      {
        var collider = obj.AddComponent<MeshCollider>();
        var mesh = new Mesh();
        renderer.BakeMesh(mesh, true);
        collider.sharedMesh = mesh;
        collider.isTrigger = true;
      });
    }
    public static void AddText(GameObject obj, string text)
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