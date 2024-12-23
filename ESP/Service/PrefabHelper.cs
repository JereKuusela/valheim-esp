
using System;
using System.Collections.Generic;
using System.Linq;
using Service;

namespace Data;

// Separate class because parsing and caching is quite complicated.
// Input string can contain wildcards, multiple values and value groups.
// There should be a single entry point that handles all of these.

// Caching simply saves results for the same input string.
// Since value groups can be changed, cache should be cleared when needed.

public class PrefabHelper
{
  public static int? GetPrefab(string value)
  {
    var prefabs = GetPrefabs(value);
    if (prefabs.Count == 0) return null;
    if (prefabs.Count == 1) return prefabs[0];
    return prefabs[UnityEngine.Random.Range(0, prefabs.Count)];
  }
  private static readonly Dictionary<string, List<int>> ResultCache = [];

  public static void ClearCache()
  {
    ResultCache.Clear();
    PrefabCache.Clear();
    LoadGroups();
  }


  private static readonly Dictionary<int, List<int>> Groups = [];
  private static readonly int WearNTearHash = "wearntear".GetStableHashCode();
  private static readonly int HumanoidHash = "humanoid".GetStableHashCode();
  private static readonly int CreatureHash = "creature".GetStableHashCode();
  private static readonly int StructureHash = "structure".GetStableHashCode();
  private static void LoadGroups()
  {
    if (Groups.Count > 0) return;
    foreach (var kvp in ZNetScene.instance.m_namedPrefabs)
    {
      if (!kvp.Value) continue;
      kvp.Value.GetComponentsInChildren(ZNetView.m_tempComponents);
      foreach (var component in ZNetView.m_tempComponents)
      {
        var hash = component.GetType().Name.ToLowerInvariant().GetStableHashCode();
        if (!Groups.ContainsKey(hash))
          Groups[hash] = [];
        Groups[hash].Add(kvp.Key);
      }
    }
    // Some key codes are hardcoded for legacy reasons.
    Groups[CreatureHash] = Groups[HumanoidHash];
    Groups[StructureHash] = Groups[WearNTearHash];
  }
  public static List<int> GetPrefabs(string value)
  {
    if (ResultCache.ContainsKey(value)) return ResultCache[value];
    var values = Parse.ToList(value);
    var prefabs = GetPrefabs(values);
    // No point to cache error results from users.
    if (prefabs == null) return [];
    ResultCache[value] = prefabs;
    return prefabs;
  }
  // Called by PrefabValue that handles the caching.
  // Ideally this would be cached here but difficult with parameters.

  public static List<int>? GetPrefabs(List<string> values)
  {
    if (values.Count == 0) return null;
    if (values.Count == 1) return ParsePrefabs(values[0]);
    var prefabs = values.Select(ParsePrefabs).Where(s => s != null).ToList();
    HashSet<int> value = [];
    foreach (var p in prefabs)
    {
      if (p == null) continue;
      foreach (var i in p) value.Add(i);
    }
    return value.Count == 0 ? null : [.. value];
  }

  private static Dictionary<string, int> PrefabCache = [];
  private static List<int>? ParsePrefabs(string prefab)
  {
    var p = prefab.ToLowerInvariant();
    if (PrefabCache.Count == 0)
      PrefabCache = ZNetScene.instance.m_namedPrefabs.ToDictionary(pair => pair.Value.name, pair => pair.Key);
    if (p == "*")
      return [.. PrefabCache.Values];
    if (p[0] == '*' && p[p.Length - 1] == '*')
    {
      p = p.Substring(1, p.Length - 2);
      return PrefabCache.Where(pair => pair.Key.ToLowerInvariant().Contains(p)).Select(pair => pair.Value).ToList();
    }
    if (p[0] == '*')
    {
      p = p.Substring(1);
      return PrefabCache.Where(pair => pair.Key.EndsWith(p, StringComparison.OrdinalIgnoreCase)).Select(pair => pair.Value).ToList();
    }
    if (p[p.Length - 1] == '*')
    {
      p = p.Substring(0, p.Length - 1);
      return PrefabCache.Where(pair => pair.Key.StartsWith(p, StringComparison.OrdinalIgnoreCase)).Select(pair => pair.Value).ToList();
    }
    if (PrefabCache.ContainsKey(prefab))
      return [PrefabCache[prefab]];
    var group = p.GetStableHashCode();
    if (Groups.ContainsKey(group))
      return Groups[group];
    Log.Warning($"Failed to resolve prefab: {prefab}");
    return null;
  }
}
