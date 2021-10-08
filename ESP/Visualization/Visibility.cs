using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Visualization {
  public class Visibility : Component {

    ///<summary Global visibility (intended for admin checks).</summary>
    private static bool visible = false;
    private static HashSet<string> visibleGroups = new HashSet<string>();
    private static HashSet<string> visibleTags = new HashSet<string>();
    private static HashSet<string> groups = new HashSet<string>();
    public static List<string> GetGroups => groups.ToList();
    private static HashSet<string> tags = new HashSet<string>();
    public static List<string> GetTags => tags.ToList();
    private static Dictionary<string, string> tagToGroup = new Dictionary<string, string>();
    public static void AddTagToGroup(string group, string tag) {
      tagToGroup[tag] = group;
      tags.Add(tag);
      groups.Add(group);
    }
    ///<summary>Returns whether a given visual tag is shown.</summary>
    public static bool IsTag(string name) => visible && visibleTags.Contains(name) && (!tagToGroup.ContainsKey(name) || visibleGroups.Contains(tagToGroup[name]));
    public static bool IsGroup(string name) => visible && visibleGroups.Contains(name);
    public static void Set(bool visibility) {
      visible = visibility;
      UpdateVisibility();
    }
    ///<summary>Toggles visibility of a tag.</summary>
    public static void ToggleTag(string tag) => SetTag(tag, !visibleTags.Contains(tag));
    ///<summary>Sets visibility of a tag.</summary>
    public static void SetTag(string tag, bool visibility) {
      if (visibility) visibleTags.Add(tag);
      else visibleTags.Remove(tag);
      UpdateTagVisibility(tag);

      //??? SupportUtils.UpdateVisibility();
    }
    ///<summary>Toggles visibility of a group.</summary>
    public static void ToggleGroup(string name) => SetGroup(name, !visibleGroups.Contains(name));
    ///<summary>Sets visibility of a group.</summary>
    public static void SetGroup(string name, bool visibility) {
      if (visibility) visibleGroups.Add(name);
      else visibleGroups.Remove(name);
      UpdateGroupVisibility(name);
    }
    private static void UpdateGroupVisibility(string name) {
      foreach (var obj in Utils.GetVisualizations()) {
        if (!tagToGroup.ContainsKey(obj.Tag)) continue;
        var group = tagToGroup[obj.Tag];
        if (group != name) continue;
        UpdateVisibility(obj);
      }
    }
    private static void UpdateTagVisibility(string name) {
      foreach (var obj in Utils.GetVisualizations()) {
        if (obj.Tag != name) continue;
        UpdateVisibility(obj);
      }
    }
    private static void UpdateVisibility() {
      foreach (var obj in Utils.GetVisualizations()) {
        UpdateVisibility(obj);
      }
    }
    private static void UpdateVisibility(Visualization obj) {
      var gameObj = obj.gameObject;
      var visible = IsTag(obj.Tag);
      if (gameObj.activeSelf != visible)
        gameObj.SetActive(visible);
    }
  }
}