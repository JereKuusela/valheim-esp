using UnityEngine;

namespace ESP
{
  public partial class Drawer
  {
    private static Vector3 GetArcSegmentX(float angle, float radius) =>
     new Vector3(0f, Mathf.Sin(Mathf.Deg2Rad * angle) * radius, Mathf.Cos(Mathf.Deg2Rad * angle) * radius);
    private static Vector3 GetArcSegmentY(float angle, float radius) =>
     new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle) * radius, 0f, Mathf.Cos(Mathf.Deg2Rad * angle) * radius);
    private static Vector3 GetArcSegmentZ(float angle, float radius) =>
     new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle) * radius, Mathf.Cos(Mathf.Deg2Rad * angle) * radius, 0f);
    private static Vector3[] GetArcSegmentsX(Vector3 position, float angle, float radius)
    {
      var currentAngle = -angle / 2f;
      var segments = GetSegments(angle);
      var points = new Vector3[segments + 1];
      for (int i = 0; i <= segments; i++)
      {
        points[i] = position + GetArcSegmentX(currentAngle, radius);
        currentAngle += (angle / segments);
      }
      return points;
    }
    private static Vector3[] GetArcSegmentsY(Vector3 position, float angle, float radius)
    {
      var currentAngle = -angle / 2f;
      var segments = GetSegments(angle);
      var points = new Vector3[segments + 1];
      for (int i = 0; i <= segments; i++)
      {
        points[i] = position + GetArcSegmentY(currentAngle, radius);
        currentAngle += (angle / segments);
      }
      return points;
    }
    private static Vector3[] GetArcSegmentsZ(Vector3 position, float angle, float radius)
    {
      var currentAngle = -angle / 2f;
      var segments = GetSegments(angle);
      var points = new Vector3[segments + 1];
      for (int i = 0; i <= segments; i++)
      {
        points[i] = position + GetArcSegmentZ(currentAngle, radius);
        currentAngle += (angle / segments);
      }
      return points;
    }
    private static void UpdateArcX(LineRenderer renderer, Vector3 position, float radius, float angle, float width)
    {
      var segments = GetArcSegmentsX(position, angle, radius - width / 2f);
      renderer.SetPositions(segments);
    }
    public static void DrawArcX(GameObject obj, Vector3 position, float radius, float angle, Color color, float width)
    {
      var renderer = CreateRenderer(obj, color, width);
      UpdateArcX(renderer, position, radius, angle, width);
    }
    private static void UpdateArcY(LineRenderer renderer, Vector3 position, float radius, float angle, float width)
    {
      var segments = GetArcSegmentsY(position, angle, radius - width / 2f);
      renderer.SetPositions(segments);
    }
    public static void DrawArcY(GameObject obj, Vector3 position, float radius, float angle, Color color, float width)
    {
      var renderer = CreateRenderer(obj, color, width);
      UpdateArcY(renderer, position, radius, angle, width);
    }

    private static void UpdateArcZ(LineRenderer renderer, Vector3 position, float radius, float angle, float width)
    {
      var segments = GetArcSegmentsZ(position, angle, radius - width / 2f);
      renderer.SetPositions(segments);
    }
    public static void DrawArcZ(GameObject obj, Vector3 position, float radius, float angle, Color color, float width)
    {
      var renderer = CreateRenderer(obj, color, width);
      UpdateArcZ(renderer, position, radius, angle, width);
    }
  }
}