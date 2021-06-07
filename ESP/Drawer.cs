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

  public class Drawer : Component
  {
    private static bool Shown = Settings.showVisualization;
    public static void ToggleVisibility()
    {
      Shown = !Shown;
      foreach (var gameObj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
      {
        if (gameObj.name == "ESP")
        {
          gameObj.SetActive(Shown);
        }
      }
    }
    private static int GetSegments(float angle) => (int)Math.Floor(32 * angle / 360);
    private static GameObject CreateObject(GameObject parent)
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
    private static LineRenderer CreateComponent(GameObject obj, Color color, float width)
    {
      var component = obj.AddComponent<LineRenderer>();
      component.useWorldSpace = false;
      component.material = new Material(Shader.Find("Standard TwoSided"));
      component.material.SetColor("_Color", color);
      component.widthMultiplier = width;
      return component;
    }

    public static void DrawLine(GameObject parent, Vector3 start, Vector3 end, Color color, float width, string text = "")
    {
      var obj = CreateObject(parent);
      if (text != "")
      {
        obj.AddComponent<HoverText>().m_text = text;
        var collider = obj.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.center = start + (end - start) / 2;
        collider.size = (end - start) + 2 * new Vector3(width, width, width);

      }
      var component = CreateComponent(obj, color, width);
      component.SetPosition(0, start);
      component.SetPosition(1, end);
    }
    public static void DrawLine(GameObject parent, Vector3 start, Vector3 end, Color color, float width, Action<GameObject> textCallback)
    {
      var obj = CreateObject(parent);
      textCallback(obj);
      var collider = obj.AddComponent<BoxCollider>();
      collider.isTrigger = true;
      collider.center = start + (end - start) / 2;
      collider.size = (end - start) + 2 * new Vector3(width, width, width);
      var component = CreateComponent(obj, color, width);
      component.SetPosition(0, start);
      component.SetPosition(1, end);
    }

    private static void UpdateSphereText(GameObject obj, float radius, string text)
    {
      if (text == "") return;
      obj.GetComponentInChildren<HoverText>().m_text = text;
      var collider = obj.GetComponentInChildren<SphereCollider>();
      collider.isTrigger = true;
      collider.radius = radius;
    }
    private static void AddSphereText(GameObject obj, float radius, string text)
    {
      if (text == "") return;
      obj.AddComponent<HoverText>().m_text = text;
      var collider = obj.AddComponent<SphereCollider>();
      collider.isTrigger = true;
      collider.radius = radius;
    }
    private static void AddSphereText(GameObject obj, float radius, Action<GameObject> textCallback)
    {
      textCallback(obj);
      var collider = obj.AddComponent<SphereCollider>();
      collider.isTrigger = true;
      collider.radius = radius;
    }
    public static void UpdateArcX(LineRenderer renderer, Vector3 position, float radius, float angle)
    {
      var currentAngle = -angle / 2f;
      var segments = GetSegments(angle);
      renderer.positionCount = segments + 1;
      for (int i = 0; i < (segments + 1); i++)
      {
        var y = position.y + Mathf.Sin(Mathf.Deg2Rad * currentAngle) * radius;
        var z = position.z + Mathf.Cos(Mathf.Deg2Rad * currentAngle) * radius;
        renderer.SetPosition(i, new Vector3(position.x, y, z));
        currentAngle += (angle / segments);
      }
    }
    public static void DrawArcX(GameObject parent, Vector3 position, float radius, float angle, Color color, float width, string text = "")
    {
      var obj = CreateObject(parent);
      AddSphereText(obj, radius, text);
      var component = CreateComponent(obj, color, width);
      UpdateArcX(component, position, radius, angle);
    }
    public static void DrawArcX(GameObject parent, Vector3 position, float radius, float angle, Color color, float width, Action<GameObject> textCallback)
    {
      var obj = CreateObject(parent);
      AddSphereText(obj, radius, textCallback);
      var component = CreateComponent(obj, color, width);
      UpdateArcX(component, position, radius, angle);
    }
    public static void UpdateArcY(LineRenderer renderer, Vector3 position, float radius, float angle)
    {
      var currentAngle = -angle / 2f;
      var segments = GetSegments(angle);
      renderer.positionCount = segments + 1;
      for (int i = 0; i < (segments + 1); i++)
      {
        var x = position.x + Mathf.Sin(Mathf.Deg2Rad * currentAngle) * radius;
        var z = position.z + Mathf.Cos(Mathf.Deg2Rad * currentAngle) * radius;
        renderer.SetPosition(i, new Vector3(x, position.y, z));
        currentAngle += (angle / segments);
      }
    }
    public static void DrawArcY(GameObject parent, Vector3 position, float radius, float angle, Color color, float width, string text = "")
    {
      var obj = CreateObject(parent);
      AddSphereText(obj, radius, text);
      var component = CreateComponent(obj, color, width);
      UpdateArcY(component, position, radius, angle);
    }
    public static void UpdateArcZ(LineRenderer renderer, Vector3 position, float radius, float angle)
    {
      var currentAngle = -angle / 2f;
      var segments = GetSegments(angle);
      renderer.positionCount = segments + 1;
      for (int i = 0; i < (segments + 1); i++)
      {
        var x = position.x + Mathf.Sin(Mathf.Deg2Rad * currentAngle) * radius;
        var y = position.y + Mathf.Cos(Mathf.Deg2Rad * currentAngle) * radius;
        renderer.SetPosition(i, new Vector3(x, y, position.z));
        currentAngle += (angle / segments);
      }
    }
    public static void DrawArcZ(GameObject parent, Vector3 position, float radius, float angle, Color color, float width, string text = "")
    {
      var obj = CreateObject(parent);
      AddSphereText(obj, radius, text);
      var component = CreateComponent(obj, color, width);
      UpdateArcZ(component, position, radius, angle);
    }
    private static void UpdateCircleX(LineRenderer renderer, Vector3 position, float radius)
    {
      UpdateArcX(renderer, position, radius, 360f);
    }
    private static void DrawCircleX(GameObject parent, Vector3 position, float radius, Color color, float width, string text = "")
    {
      DrawArcX(parent, position, radius, 360f, color, width, text);
    }
    private static void DrawCircleX(GameObject parent, Vector3 position, float radius, Color color, float width, Action<GameObject> textCallback)
    {
      DrawArcX(parent, position, radius, 360f, color, width, textCallback);
    }
    private static void UpdateCircleY(LineRenderer renderer, Vector3 position, float radius)
    {
      UpdateArcY(renderer, position, radius, 360f);
    }
    private static void DrawCircleY(GameObject parent, Vector3 position, float radius, Color color, float width, string text = "")
    {
      DrawArcY(parent, position, radius, 360f, color, width, text);
    }
    private static void UpdateCircleZ(LineRenderer renderer, Vector3 position, float radius)
    {
      UpdateArcZ(renderer, position, radius, 360f);
    }
    private static void DrawCircleZ(GameObject parent, Vector3 position, float radius, Color color, float width, string text = "")
    {
      DrawArcZ(parent, position, radius, 360f, color, width, text);
    }
    public static void DrawCircle(GameObject parent, Vector3 position, float radius, Color color, float width, string text = "")
    {
      DrawCircleY(parent, position, radius, color, width, text);
    }
    public static void UpdateSphere(GameObject parent, Vector3 position, float radius, string text = "")
    {
      UpdateSphereText(parent, radius, text);
      var renderers = parent.GetComponentsInChildren<LineRenderer>();
      if (renderers.Length != 3) return;
      UpdateCircleX(renderers[0], position, radius);
      UpdateCircleY(renderers[1], position, radius);
      UpdateCircleZ(renderers[2], position, radius);
    }
    public static void DrawSphere(GameObject parent, Vector3 position, float radius, Color color, float width, string text = "")
    {
      DrawCircleX(parent, position, radius, color, width, text);
      DrawCircleY(parent, position, radius, color, width);
      DrawCircleZ(parent, position, radius, color, width);
    }
    public static void DrawSphere(GameObject parent, Vector3 position, float radius, Color color, float width, Action<GameObject> textCallback)
    {
      DrawCircleX(parent, position, radius, color, width, textCallback);
      DrawCircleY(parent, position, radius, color, width);
      DrawCircleZ(parent, position, radius, color, width);
    }


    public static void DrawMarkerLine(GameObject parent, Vector3 start, Color color, float width, string text = "")
    {
      var end = new Vector3(start.x, 500f, start.z);
      DrawLine(parent, start, end, color, width, text);
    }
    public static void DrawMarkerLine(GameObject parent, Vector3 start, Color color, float width, Action<GameObject> textCallback)
    {
      var end = new Vector3(start.x, 500f, start.z);
      DrawLine(parent, start, end, color, width, textCallback);
    }
    public static void DrawConeY(GameObject parent, Vector3 position, float radius, float angle, Color color, float width, string text = "")
    {
      var obj = CreateObject(parent);
      AddSphereText(obj, radius, text);
      var component = CreateComponent(obj, color, width);
      var currentAngle = -angle / 2f;
      var segments = GetSegments(angle);
      component.positionCount = segments + 3;
      component.SetPosition(0, position);
      for (int i = 1; i < (segments + 2); i++)
      {
        var x = position.x + Mathf.Sin(Mathf.Deg2Rad * currentAngle) * radius;
        var z = position.z + Mathf.Cos(Mathf.Deg2Rad * currentAngle) * radius;
        component.SetPosition(i, new Vector3(x, position.y, z));
        currentAngle += (angle / segments);
      }
      component.SetPosition(segments + 2, position);
    }
    public static void DrawConeX(GameObject parent, Vector3 position, float radius, float angle, Color color, float width, string text = "")
    {
      var obj = CreateObject(parent);
      AddSphereText(obj, radius, text);
      var component = CreateComponent(obj, color, width);
      var currentAngle = -angle / 2f;
      var segments = GetSegments(angle);
      component.positionCount = segments + 3;
      component.SetPosition(0, position);
      for (int i = 1; i < (segments + 2); i++)
      {
        var y = position.y + Mathf.Sin(Mathf.Deg2Rad * currentAngle) * radius;
        var z = position.z + Mathf.Cos(Mathf.Deg2Rad * currentAngle) * radius;
        component.SetPosition(i, new Vector3(position.x, y, z));
        currentAngle += (angle / segments);
      }
      component.SetPosition(segments + 2, position);
    }

    public static void UpdateTexts(GameObject parent, string text)
    {
      Array.ForEach(parent.GetComponentsInChildren<HoverText>(), item => item.m_text = text);
    }

    public static void UpdateTexts(GameObject parent, List<string> texts)
    {
      var items = parent.GetComponentsInChildren<HoverText>();
      for (var i = 0; i < items.Length && i < texts.Count; i++)
        items[i].m_text = texts[i];
    }
  }
}