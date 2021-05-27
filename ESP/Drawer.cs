using System;
using UnityEngine;

namespace ESP
{
  public class Drawer
  {
    private static int GetSegments(float angle) => (int)Math.Floor(32 * angle / 360);
    private static GameObject CreateObject(GameObject parent)
    {
      var obj = new GameObject();
      obj.transform.position = parent.transform.position;
      obj.transform.rotation = parent.transform.rotation;
      obj.transform.parent = parent.transform;
      return obj;
    }
    private static LineRenderer CreateComponent(GameObject obj, Color color, float width)
    {
      var component = obj.AddComponent<LineRenderer>();
      component.useWorldSpace = false;
      component.material = new Material(Shader.Find("Standard"));
      component.material.SetColor("_Color", color);
      component.widthMultiplier = width;
      return component;
    }

    private static Vector3 GetLineCollider(Vector3 start, Vector3 end, float width)
    {
      return new Vector3(Math.Abs(end.x - start.x) + width, Math.Abs(end.y - start.y) + width, Math.Abs(end.z - start.z) + width);
    }
    public static void DrawLine(GameObject parent, Vector3 start, Vector3 end, Color color, float width, String text = "")
    {
      var obj = CreateObject(parent);
      if (text != "")
      {
        obj.AddComponent<HoverText>().m_text = text;
        var collider = obj.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = GetLineCollider(start, end, width);

      }
      var component = CreateComponent(obj, color, width);
      component.SetPosition(0, start);
      component.SetPosition(1, end);
    }

    private static void UpdateSphereText(GameObject obj, float radius, String text)
    {
      if (text == "") return;
      obj.GetComponentInChildren<HoverText>().m_text = text;
      var collider = obj.GetComponentInChildren<SphereCollider>();
      collider.isTrigger = true;
      collider.radius = radius;
    }
    private static void AddSphereText(GameObject obj, float radius, String text)
    {
      if (text == "") return;
      obj.AddComponent<HoverText>().m_text = text;
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
    public static void DrawArcX(GameObject parent, Vector3 position, float radius, float angle, Color color, float width, String text = "")
    {
      var obj = CreateObject(parent);
      AddSphereText(obj, radius, text);
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
    public static void DrawArcY(GameObject parent, Vector3 position, float radius, float angle, Color color, float width, String text = "")
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
    public static void DrawArcZ(GameObject parent, Vector3 position, float radius, float angle, Color color, float width, String text = "")
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
    private static void DrawCircleX(GameObject parent, Vector3 position, float radius, Color color, float width, String text = "")
    {
      DrawArcX(parent, position, radius, 360f, color, width, text);
    }
    private static void UpdateCircleY(LineRenderer renderer, Vector3 position, float radius)
    {
      UpdateArcY(renderer, position, radius, 360f);
    }
    private static void DrawCircleY(GameObject parent, Vector3 position, float radius, Color color, float width, String text = "")
    {
      DrawArcY(parent, position, radius, 360f, color, width, text);
    }
    private static void UpdateCircleZ(LineRenderer renderer, Vector3 position, float radius)
    {
      UpdateArcZ(renderer, position, radius, 360f);
    }
    private static void DrawCircleZ(GameObject parent, Vector3 position, float radius, Color color, float width, String text = "")
    {
      DrawArcZ(parent, position, radius, 360f, color, width, text);
    }
    public static void DrawCircle(GameObject parent, Vector3 position, float radius, Color color, float width, String text = "")
    {
      DrawCircleY(parent, position, radius, color, width, text);
    }
    public static void UpdateSphere(GameObject parent, Vector3 position, float radius, String text = "")
    {
      UpdateSphereText(parent, radius, text);
      var renderers = parent.GetComponentsInChildren<LineRenderer>();
      if (renderers.Length != 3) return;
      UpdateCircleX(renderers[0], position, radius);
      UpdateCircleY(renderers[1], position, radius);
      UpdateCircleZ(renderers[2], position, radius);
    }
    public static void DrawSphere(GameObject parent, Vector3 position, float radius, Color color, float width, String text = "")
    {
      DrawCircleX(parent, position, radius, color, width, text);
      DrawCircleY(parent, position, radius, color, width);
      DrawCircleZ(parent, position, radius, color, width);
    }


    public static void DrawMarkerLine(GameObject parent, Vector3 start, Color color, float width, String text = "")
    {
      var end = new Vector3(start.x, 500f, start.z);
      DrawLine(parent, start, end, color, width, text);
    }
    public static void DrawConeY(GameObject parent, Vector3 position, float radius, float angle, Color color, float width, String text = "")
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
    public static void DrawConeX(GameObject parent, Vector3 position, float radius, float angle, Color color, float width, String text = "")
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
  }
}