using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Visualization;
public class Visibility : Component
{

  ///<summary Global visibility (intended for admin checks).</summary>
  private static bool visible = false;
  private static readonly HashSet<string> visibleTags = [];
  private static readonly HashSet<string> tags = [];
  public static List<string> GetTags => [.. tags];
  private static readonly Dictionary<string, string> tagToGroup = [];
  ///<summary>Returns whether a given visual tag is shown.</summary>
  public static bool IsTag(string name) => visible && visibleTags.Contains(name) && (!tagToGroup.ContainsKey(name));
  public static void Set(bool visibility)
  {
    visible = visibility;
    UpdateVisibility();
  }
  ///<summary>Toggles visibility of a tag.</summary>
  public static void ToggleTag(string tag) => SetTag(tag, !visibleTags.Contains(tag));
  ///<summary>Sets visibility of a tag.</summary>
  public static void SetTag(string tag, bool visibility)
  {
    tags.Add(tag);
    if (visibility) visibleTags.Add(tag);
    else visibleTags.Remove(tag);
    UpdateTagVisibility(tag);
  }
  private static void UpdateTagVisibility(string name)
  {
    foreach (var obj in Utils.GetVisualizations())
    {
      if (obj.Tag != name) continue;
      UpdateVisibility(obj);
    }
  }
  private static void UpdateVisibility()
  {
    foreach (var obj in Utils.GetVisualizations())
    {
      UpdateVisibility(obj);
    }
  }
  private static void UpdateVisibility(Visualization obj)
  {
    var gameObj = obj.gameObject;
    var visible = IsTag(obj.Tag);
    if (gameObj.activeSelf != visible)
      gameObj.SetActive(visible);
  }
}
