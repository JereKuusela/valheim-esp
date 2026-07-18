using System.Collections.Generic;
using ESP;
using HarmonyLib;
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
  private static readonly Dictionary<string, int> tagHashCache = [];
  public static List<string> GetTags => [.. tags.Values];

  public static void CleanUp()
  {
    // Previous info depends on permissions that are server specific. So must be cleared to avoid stale cache.
    previousTags.Clear();
  }
  public static int GetTagHash(string name)
  {
    if (tagHashCache.TryGetValue(name, out var hash)) return hash;

    hash = name.ToLowerInvariant().GetStableHashCode();

    tagHashCache[name] = hash;
    return hash;
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
  public static void Reload()
  {
    foreach (var hash in tagHashes)
      ApplyTagState(hash);
  }
  public static bool IsTagEnabled(string tag)
  {
    var hash = GetTagHash(tag);
    return PermissionManager.IsVisualFeatureEnabled(hash, enabledTags.Contains(hash));
  }
  private static void ApplyTagState(int hash)
  {
    if (!tags.TryGetValue(hash, out var tag)) return;

    var previous = previousTags.Contains(hash);
    var enabled = PermissionManager.IsVisualFeatureEnabled(hash, enabledTags.Contains(hash));

    if (enabled == previous) return;

    if (enabled) previousTags.Add(hash);
    else previousTags.Remove(hash);
    Visualization.Remove(tag);
    if (enabled)
      RebuildTag(tag);
  }
}

[HarmonyPatch(typeof(WorldGenerator), nameof(WorldGenerator.Initialize))]
public class CleanupOnStart
{
  static void Postfix()
  {
    Visibility.CleanUp();
  }
}