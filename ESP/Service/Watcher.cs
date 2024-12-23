using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;

namespace Service;

public class Watcher
{
  public static string BaseDirectory = Paths.ConfigPath;

  public static void SetupWatcher(ConfigFile config)
  {
    FileSystemWatcher watcher = new(Path.GetDirectoryName(config.ConfigFilePath), Path.GetFileName(config.ConfigFilePath));
    watcher.Changed += (s, e) => ReadConfigValues(e.FullPath, config);
    watcher.Created += (s, e) => ReadConfigValues(e.FullPath, config);
    watcher.Renamed += (s, e) => ReadConfigValues(e.FullPath, config);
    watcher.IncludeSubdirectories = true;
    watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
    watcher.EnableRaisingEvents = true;
  }
  private static void ReadConfigValues(string path, ConfigFile config)
  {
    if (!File.Exists(path)) return;
    try
    {
      config.Reload();
    }
    catch
    {
      Log.Error($"There was an issue loading your {config.ConfigFilePath}");
      Log.Error("Please check your config entries for spelling and format!");
    }
  }
  public static void SetupWatcher(string pattern, Action<string> action) => SetupWatcher(Paths.ConfigPath, pattern, action);
  public static void SetupWatcher(string folder, string pattern, Action<string> action)
  {
    FileSystemWatcher watcher = new(folder, pattern);
    watcher.Created += (s, e) => action(e.FullPath);
    watcher.Changed += (s, e) => action(e.FullPath);
    watcher.Renamed += (s, e) => action(e.FullPath);
    watcher.Deleted += (s, e) => action(e.FullPath);
    watcher.IncludeSubdirectories = true;
    watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
    watcher.EnableRaisingEvents = true;
  }
  public static void SetupWatcher(string folder, string pattern, Action action)
  {
    FileSystemWatcher watcher = new(folder, pattern);
    watcher.Created += (s, e) => action();
    watcher.Changed += (s, e) => action();
    watcher.Renamed += (s, e) => action();
    watcher.Deleted += (s, e) => action();
    watcher.IncludeSubdirectories = true;
    watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
    watcher.EnableRaisingEvents = true;
  }
  public static void SetupWatcher(string pattern, Action action) => SetupWatcher(BaseDirectory, pattern, action);
}
