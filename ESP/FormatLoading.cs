using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using Data;
using HarmonyLib;
using Service;
namespace ESP;

public class FormatLoading
{
  private static readonly string GamePath = Path.GetFullPath(Path.Combine("BepInEx", "config"));
  private static readonly string ProfilePath = Path.GetFullPath(Paths.ConfigPath);

  public static Dictionary<string, Dictionary<int, List<string>>> Data = [];
  public static readonly Dictionary<int, List<string>> ValueGroups = [];

  public static List<string>? Get(string file, int prefab)
  {
    if (Data.ContainsKey(file))
      return Data[file].TryGetValue(prefab, out var values1) ? values1 : null;
    var lower = file.ToLowerInvariant();
    if (file != lower)
    {
      Settings.configCustom.Value = lower;
      return Get(lower, prefab);
    }
    return Data.TryGetValue("", out var prefabs) ? prefabs.TryGetValue(prefab, out var values2) ? values2 : null : null;
  }

  public static List<string> Keys => [.. Data.Keys];

  public static void LoadEntries()
  {
    PrefabHelper.ClearCache();
    var prev = Data;
    Data = [];
    var files = Directory.GetFiles(GamePath, "esp*.yaml")
      .Concat(Directory.GetFiles(ProfilePath, "esp*.yaml"))
      .Select(Path.GetFullPath).Distinct().ToList();
    var data = Read(files);
    foreach (var kvp in data)
      LoadEntry(kvp.Key, kvp.Value);

    Log.Info($"Loaded {Data.Count} format files.");
  }
  private static Dictionary<string, Dictionary<string, List<string>>> Read(List<string> files)
  {
    Dictionary<string, Dictionary<string, List<string>>> result = [];
    foreach (var file in files)
    {
      Dictionary<string, List<string>> data = [];
      result[Path.GetFileNameWithoutExtension(file)] = data;
      try
      {
        var lines = File.ReadAllLines(file);
        var key = "";
        int row = 0;
        foreach (var line in lines)
        {
          var trimmed = line.Trim();
          if (trimmed.Length == 0 || trimmed[0] == '#') continue;
          if (trimmed.EndsWith(":", StringComparison.Ordinal))
          {
            key = trimmed.Substring(0, trimmed.Length - 1).Trim();
            if (key == "\"*\"") key = "*";
            if (!data.ContainsKey(key))
              data[key] = [];
            continue;
          }
          if (key == "")
          {
            Log.Error($"Failed to read {Path.GetFileName(file)}, missing key on row {row}");
            break;
          }
          if (trimmed.StartsWith("- ", StringComparison.Ordinal))
          {
            var value = trimmed.Substring(2);
            data[key].Add(value);
            continue;
          }
          if (trimmed.StartsWith("-", StringComparison.Ordinal))
          {
            var value = trimmed.Substring(1);
            data[key].Add(value);
            continue;
          }
          Log.Error($"Failed to read {Path.GetFileName(file)}, invalid row {row}");
        }
      }
      catch (Exception ex)
      {
        Log.Error($"Failed to read {Path.GetFileName(file)}: {ex.Message}");
      }
    }
    return result;
  }
  private static void LoadEntry(string file, Dictionary<string, List<string>> data)
  {
    var name = (file.Length > 3 && file[3] == '_' ? file.Substring(4) : file.Substring(3)).ToLowerInvariant();
    if (Data.ContainsKey(name))
      Log.Warning($"Duplicate format file: {file}");
    if (!Data.ContainsKey(name))
      Data[name] = [];
    var d = Data[name];
    foreach (var kvp in data)
    {
      var prefabs = PrefabHelper.GetPrefabs(kvp.Key);
      if (prefabs == null) continue;
      foreach (var prefab in prefabs)
      {
        if (!d.ContainsKey(prefab))
          d[prefab] = [];
        d[prefab].AddRange(kvp.Value);
      }
    }
  }

  public static string Pattern = "esp*.yaml";
  public static void SetupWatcher()
  {
    if (!Directory.Exists(GamePath))
      Directory.CreateDirectory(GamePath);
    if (!Directory.Exists(ProfilePath))
      Directory.CreateDirectory(ProfilePath);
    Watcher.SetupWatcher(GamePath, Pattern, LoadEntries);
    if (GamePath != ProfilePath)
      Watcher.SetupWatcher(ProfilePath, Pattern, LoadEntries);
  }

}

[HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.Start)), HarmonyPriority(Priority.VeryLow)]
public class InitializeContent
{
  static void Postfix()
  {
    FormatLoading.LoadEntries();
  }
}