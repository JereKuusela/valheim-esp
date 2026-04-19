using System.Collections.Generic;
using UnityEngine;

namespace Visualization;

public static class CircleAreaManager
{
  private sealed class Area(CircleRuler owner, Vector3 center, float radius)
  {
    public CircleRuler Owner = owner;
    public Vector3 Center = center;
    public float Radius = radius;
  }

  private static readonly Dictionary<CircleRuler, Area> Areas = [];

  public static void Register(CircleRuler owner, Vector3 center, float radius)
  {
    if (Areas.ContainsKey(owner)) return;
    Areas[owner] = new(owner, center, radius);
  }

  public static void UpdateRadius(CircleRuler owner, float radius, Vector3 centerIfMissing)
  {
    if (!Areas.TryGetValue(owner, out var area))
    {
      Register(owner, centerIfMissing, radius);
      return;
    }

    area.Radius = radius;
  }

  public static void Unregister(CircleRuler owner)
  {
    Areas.Remove(owner);
  }

  public static void RefreshColors()
  {
    foreach (var area in Areas.Values)
    {
      area.Owner.ApplyColor();
    }
  }

  public static bool IsInsideOtherCircle(CircleRuler owner, Vector3 position)
  {
    var pos2D = new Vector2(position.x, position.z);
    foreach (var area in Areas.Values)
    {
      if (area.Owner == owner) continue;
      var center2D = new Vector2(area.Center.x, area.Center.z);
      if ((pos2D - center2D).sqrMagnitude <= area.Radius * area.Radius)
        return true;
    }
    return false;
  }
}