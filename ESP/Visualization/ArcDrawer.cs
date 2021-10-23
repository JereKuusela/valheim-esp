using System;
using UnityEngine;

namespace Visualization {
  public partial class Draw {
    ///<summary>Returns how many straigth lines are used to draw the arc</summary>
    private static int GetSegments(float angle) => (int)Math.Floor(32 * angle / 360);
    ///<summary>Returns the arc point at a given angle and radius on the X plane</summary>
    private static Vector3 GetArcSegmentX(float angle, float radius) =>
     new Vector3(0f, Mathf.Sin(Mathf.Deg2Rad * angle) * radius, Mathf.Cos(Mathf.Deg2Rad * angle) * radius);
    ///<summary>Returns the arc point at a given angle and radius on the Y plane</summary>
    private static Vector3 GetArcSegmentY(float angle, float radius) =>
     new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle) * radius, 0f, Mathf.Cos(Mathf.Deg2Rad * angle) * radius);
    ///<summary>Returns the arc point at a given angle and radius on the Z plane</summary>
    private static Vector3 GetArcSegmentZ(float angle, float radius) =>
     new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle) * radius, Mathf.Cos(Mathf.Deg2Rad * angle) * radius, 0f);
    ///<summary>Returns points of an arc witha  given angle and angle on the X plane</summary>
    private static Vector3[] GetArcSegmentsX(Vector3 position, float angle, float radius) {
      var currentAngle = -angle / 2f;
      var segments = GetSegments(angle);
      var points = new Vector3[segments + 1];
      for (int i = 0; i <= segments; i++) {
        points[i] = position + GetArcSegmentX(currentAngle, radius);
        currentAngle += (angle / segments);
      }
      return points;
    }
    ///<summary>Returns points of an arc witha  given angle and angle on the Y plane</summary>
    private static Vector3[] GetArcSegmentsY(Vector3 position, float angle, float radius) {
      var currentAngle = -angle / 2f;
      var segments = GetSegments(angle);
      var points = new Vector3[segments + 1];
      for (int i = 0; i <= segments; i++) {
        points[i] = position + GetArcSegmentY(currentAngle, radius);
        currentAngle += (angle / segments);
      }
      return points;
    }
    ///<summary>Returns points of an arc witha  given angle and angle on the Z plane</summary>
    private static Vector3[] GetArcSegmentsZ(Vector3 position, float angle, float radius) {
      var currentAngle = -angle / 2f;
      var segments = GetSegments(angle);
      var points = new Vector3[segments + 1];
      for (int i = 0; i <= segments; i++) {
        points[i] = position + GetArcSegmentZ(currentAngle, radius);
        currentAngle += (angle / segments);
      }
      return points;
    }
    ///<summary>Updates existing arc on the X plane.</summary>
    private static void UpdateArcX(LineRenderer renderer, Vector3 position, float radius, float angle, float width) {
      var segments = GetArcSegmentsX(position, angle, radius - width / 2f);
      renderer.positionCount = segments.Length;
      renderer.SetPositions(segments);
    }
    ///<summary>Creates an arc on the X plane.</summary>
    public static void DrawArcX(GameObject obj, Vector3 position, float radius, float angle) {
      var renderer = CreateRenderer(obj);
      UpdateArcX(renderer, position, radius, angle, GetLineWidth(obj.name));
    }
    ///<summary>Updates existing arc on the Y plane.</summary>
    private static void UpdateArcY(LineRenderer renderer, Vector3 position, float radius, float angle, float width) {
      var segments = GetArcSegmentsY(position, angle, radius - width / 2f);
      renderer.positionCount = segments.Length;
      renderer.SetPositions(segments);
    }
    ///<summary>Creates an arc on the Y plane.</summary>
    public static void DrawArcY(GameObject obj, Vector3 position, float radius, float angle) {
      var renderer = CreateRenderer(obj);
      UpdateArcY(renderer, position, radius, angle, GetLineWidth(obj.name));
    }
    ///<summary>Updates existing arc on the Z plane.</summary>
    private static void UpdateArcZ(LineRenderer renderer, Vector3 position, float radius, float angle, float width) {
      var segments = GetArcSegmentsZ(position, angle, radius - width / 2f);
      renderer.positionCount = segments.Length;
      renderer.SetPositions(segments);
    }
    ///<summary>Creates an arc on the Z plane.</summary>
    public static void DrawArcZ(GameObject obj, Vector3 position, float radius, float angle) {
      var renderer = CreateRenderer(obj);
      UpdateArcZ(renderer, position, radius, angle, GetLineWidth(obj.name));
    }
    ///<summary>Creates a renderer with two frontal arcs (vertical and horizontal).</summary>
    public static GameObject DrawArc(string tag, MonoBehaviour parent, Vector3 position, float radius, float angle) {
      var obj = CreateObject(parent.gameObject, tag);
      DrawArcY(CreateObject(obj, tag), position, radius, angle);
      DrawArcX(CreateObject(obj, tag), position, radius, angle);
      AddMeshCollider(obj);
      return obj;
    }
  }
}