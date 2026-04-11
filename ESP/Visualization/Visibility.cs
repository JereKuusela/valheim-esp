using System.Collections.Generic;
using Service;
using UnityEngine;

namespace Visualization;

public class Visibility : Component
{
  private static readonly HashSet<int> enabledTags = [];
  private static readonly HashSet<int> previousTags = [];
  private static readonly HashSet<int> tagHashes = [];
  private static readonly Dictionary<int, string> tags = [];
  private static readonly Dictionary<int, List<System.Action>> rebuilders = [];
  private static readonly Dictionary<string, int> tagHashCache = new(System.StringComparer.OrdinalIgnoreCase);
  public static List<string> GetTags => [.. tags.Values];

  public static int GetTagHash(string name)
  {
    if (tagHashCache.TryGetValue(name, out var hash)) return hash;
    hash = name.ToLowerInvariant().GetStableHashCode();
    tagHashCache[name] = hash;
    return hash;
  }

  ///<summary>Returns whether a given visual tag is shown.</summary>
  public static bool IsTag(string name)
  {
    var hash = GetTagHash(name);
    return PermissionManager.IsFeatureEnabledByHash(hash, enabledTags.Contains(hash));
  }
  ///<summary>Sets visibility of a tag.</summary>
  public static void SetTag(string tag, bool visibility)
  {
    var hash = GetTagHash(tag);
    tagHashes.Add(hash);
    tags[hash] = tag;
    if (visibility) enabledTags.Add(hash);
    else enabledTags.Remove(hash);
    ApplyTagState(hash);
  }
  public static void RegisterRebuilder(string tag, System.Action rebuilder)
  {
    var hash = GetTagHash(tag);
    if (!rebuilders.TryGetValue(hash, out var handlers))
    {
      handlers = [];
      rebuilders[hash] = handlers;
    }
    if (!handlers.Contains(rebuilder))
      handlers.Add(rebuilder);
  }
  private static void RebuildTag(string tag)
  {
    var hash = GetTagHash(tag);
    if (!rebuilders.TryGetValue(hash, out var handlers)) return;
    foreach (var handler in handlers)
      handler();
  }
  private static void DestroyTag(string name)
  {
    foreach (var obj in Utils.GetVisualizations())
    {
      if (obj.Tag != name) continue;
      UnityEngine.Object.Destroy(obj.gameObject);
    }
  }
  private static void UpdateTagVisibility(string name)
  {
    foreach (var obj in Utils.GetVisualizations())
    {
      if (obj.Tag != name) continue;
      UpdateVisibility(obj);
    }
  }
  public static void Reload()
  {
    foreach (var hash in tagHashes)
      ApplyTagState(hash);
  }
  private static void ApplyTagState(int hash)
  {
    if (!tags.TryGetValue(hash, out var tag)) return;

    var previous = previousTags.Contains(hash);
    var enabled = PermissionManager.IsFeatureEnabledByHash(hash, enabledTags.Contains(hash));

    if (enabled) previousTags.Add(hash);
    else previousTags.Remove(hash);

    if (!enabled)
      DestroyTag(tag);
    else if (!previous)
      RebuildTag(tag);

    UpdateTagVisibility(tag);
  }
  private static void UpdateVisibility(Visualization obj)
  {
    var gameObj = obj.gameObject;
    var visible = IsTag(obj.Tag);
    if (gameObj.activeSelf != visible)
      gameObj.SetActive(visible);
  }
}
