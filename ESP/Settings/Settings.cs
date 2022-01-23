using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using Visualization;

namespace ESP {
  public partial class Settings {
    public static void Init(ConfigFile config) {
      InitDev(config);
      InitHUD(config);
      InitTooltips(config);
      InitVisuals(config);
      InitExcluded(config);
      InitColors(config);
      InitVisualWidth(config);
      InitCommands();
    }


    private static void InitCommands() {
      new Terminal.ConsoleCommand("esp_enable", "[name1] [name2] [name3] ... - Enables given settings.", delegate (Terminal.ConsoleEventArgs args) {
        if (args.Length < 2) {
          args.Context.AddString("Missing name.");
          return;
        }
        foreach (var arg in args[1].Split(' '))
          SetEntry(arg, true);
      }, optionsFetcher: OptionsFetcher);
      new Terminal.ConsoleCommand("esp_toggle", "[name1] [name2] [name3] ... - Toggles given settings.", delegate (Terminal.ConsoleEventArgs args) {
        if (args.Length < 2) {
          args.Context.AddString("Missing name.");
          return;
        }
        foreach (var arg in args[1].Split(' '))
          ToggleEntry(arg);
      }, optionsFetcher: OptionsFetcher);
      new Terminal.ConsoleCommand("esp_disable", "[name1] [name2] [name3] ... - Disbables given settings.", delegate (Terminal.ConsoleEventArgs args) {
        if (args.Length < 2) {
          args.Context.AddString("Missing name.");
          return;
        }
        foreach (var arg in args[1].Split(' '))
          SetEntry(arg, false);
      }, optionsFetcher: OptionsFetcher);
    }
    private static List<string> OptionsFetcher() {
      var options = new List<string>();
      options.AddRange(Visibility.GetTags);
      options = options.Where(tag => tag == Tag.ZoneCorner || tag == Tag.SpawnZone || (!tag.StartsWith(Tag.ZoneCorner) && tag.StartsWith(Tag.SpawnZone))).ToList();
      options.Add(Tool.ExtraInfo);
      options.Add(Tool.TimeAndWeather);
      options.Add(Tool.Position);
      options.Add(Tool.HUD);
      options.Sort();
      return options;
    }
    private static ConfigEntry<bool> GetOtherEntry(string name) {
      name = name.ToLower();
      if (name == Tool.ExtraInfo.ToLower()) return configExtraInfo;
      if (name == Tool.TimeAndWeather.ToLower()) return configShowTimeAndWeather;
      if (name == Tool.Position.ToLower()) return configShowPosition;
      if (name == Tool.HUD.ToLower()) return configShowHud;
      throw new NotImplementedException(name);
    }
    private static void SetEntry(string name, bool value) {
      try {
        var entry = GetTagEntry(name);
        if (entry.Value < 0) return;
        var intValue = value ? 1 : 0;
        if (entry.Value != intValue)
          entry.Value = intValue;
        return;
      } catch (NotImplementedException) { }
      try {
        var entry = GetOtherEntry(name);
        if (entry.Value != value)
          entry.Value = value;
        return;
      } catch (NotImplementedException) { }
      throw new NotImplementedException(name);
    }
    private static void ToggleEntry(string name) {
      try {
        var entry = GetTagEntry(name);
        if (entry.Value < 0) return;
        entry.Value = entry.Value > 0 ? 0 : 1;
        return;
      } catch (NotImplementedException) { }
      try {
        var entry = GetOtherEntry(name);
        entry.Value = !entry.Value;
        return;
      } catch (NotImplementedException) { }
      throw new NotImplementedException(name);
    }
  }
}
