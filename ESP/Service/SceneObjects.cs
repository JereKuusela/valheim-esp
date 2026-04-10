using System.Collections.Generic;
using UnityEngine;

namespace Service;

public static class SceneObjects
{
  public static IEnumerable<T> FindLoaded<T>() where T : MonoBehaviour
  {
    var seen = new HashSet<int>();
    foreach (var view in EnumerateViews())
    {
      if (!view || !view.gameObject.scene.IsValid()) continue;
      if (view.TryGetComponent<T>(out var single) && single && seen.Add(single.GetInstanceID()))
        yield return single;
      var children = view.GetComponentsInChildren<T>(true);
      foreach (var child in children)
      {
        if (!child || !child.gameObject.scene.IsValid()) continue;
        if (seen.Add(child.GetInstanceID()))
          yield return child;
      }
    }
  }

  private static IEnumerable<ZNetView> EnumerateViews()
  {
    if (!ZNetScene.instance) yield break;
    foreach (var kvp in ZNetScene.instance.m_instances)
    {
      var view = kvp.Value;
      if (!view) continue;
      yield return view;
    }
  }
}