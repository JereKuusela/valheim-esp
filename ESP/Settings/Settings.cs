using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using Modules;
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
      MinerockSupport.Init(config);
      InitCommands();
    }


    private static void InitCommands() {
      new Terminal.ConsoleCommand("esp_enable", "[name] - Enables a given setting.", delegate (Terminal.ConsoleEventArgs args) {
        if (args.Length < 2) {
          args.Context.AddString("Missing name.");
          return;
        }
        SetEntry(args[1], true);
      }, isCheat: true, optionsFetcher: OptionsFetcher);
      new Terminal.ConsoleCommand("esp_toggle", "[name] - Toggles a given setting.", delegate (Terminal.ConsoleEventArgs args) {
        if (args.Length < 2) {
          args.Context.AddString("Missing name.");
          return;
        }
        ToggleEntry(args[1]);
      }, isCheat: true, optionsFetcher: OptionsFetcher);
      new Terminal.ConsoleCommand("esp_disable", "[name] - Disbables a given setting.", delegate (Terminal.ConsoleEventArgs args) {
        if (args.Length < 2) {
          args.Context.AddString("Missing name.");
          return;
        }
        SetEntry(args[1], false);
      }, isCheat: true, optionsFetcher: OptionsFetcher);
    }
    private static List<string> OptionsFetcher() {
      var options = new List<string>();
      options.AddRange(Visibility.GetTags);
      options.AddRange(Visibility.GetGroups);
      options.Add(Tool.ExtraInfo);
      options.Add(Tool.DPS);
      options.Add(Tool.Experience);
      options.Add(Tool.TimeAndWeather);
      options.Add(Tool.Position);
      options.Add(Tool.HUD);
      options.Add(Tool.Ruler);
      options.Sort();
      return options;
    }
    private static ConfigEntry<bool> GetOtherEntry(string name) {
      name = name.ToLower();
      if (name == Tool.ExtraInfo.ToLower()) return configExtraInfo;
      if (name == Tool.DPS.ToLower()) return configShowDPS;
      if (name == Tool.Experience.ToLower()) return configShowExperienceMeter;
      if (name == Tool.TimeAndWeather.ToLower()) return configShowTimeAndWeather;
      if (name == Tool.Position.ToLower()) return configShowPosition;
      if (name == Tool.HUD.ToLower()) return configShowHud;
      if (name == Tool.Ruler.ToLower()) return configShowRuler;
      throw new NotImplementedException();
    }
    private static ConfigEntry<bool> GetEntry(string name) {
      try {
        return GetTagEntry(name);
      } catch (NotImplementedException) { }
      try {
        return GetGroupEntry(name);
      } catch (NotImplementedException) { }
      try {
        return GetOtherEntry(name);
      } catch (NotImplementedException) { }
      throw new NotImplementedException();
    }
    private static void SetEntry(string name, bool value) {
      var entry = GetEntry(name);
      if (entry.Value != value)
        entry.Value = value;
    }
    private static void ToggleEntry(string name) {
      var entry = GetEntry(name);
      entry.Value = !entry.Value;
    }
  }
}
