﻿using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using UnityEngine;
using Visualization;
namespace ESP;
public partial class Settings
{
  public static void Init(ConfigFile config)
  {
    InitHUD(config);
    InitTooltips(config);
    InitVisuals(config);
    InitExcluded(config);
    InitColors(config);
    InitVisualWidth(config);
    InitCommands();
  }


  private static void InitCommands()
  {
    new Terminal.ConsoleCommand("esp_enable", "[name1] [name2] [name3] ... - Enables given settings.", args =>
    {
      if (args.Length < 2)
      {
        args.Context.AddString("Missing name.");
        return;
      }
      var entries = args[1].Split(' ').ToList();
      if (args[1] == "*")
        entries = OptionsFetcher();
      foreach (var arg in entries)
        SetEntry(arg, 1);
    }, optionsFetcher: OptionsFetcher);
    new Terminal.ConsoleCommand("esp_toggle", "[name1] [name2] [name3] ... - Toggles given settings.", args =>
    {
      if (args.Length < 2)
      {
        args.Context.AddString("Missing name.");
        return;
      }
      var entries = args[1].Split(' ').ToList();
      if (args[1] == "*")
        entries = OptionsFetcher();
      foreach (var arg in entries)
        ToggleEntry(arg);
    }, optionsFetcher: OptionsFetcher);
    new Terminal.ConsoleCommand("esp_disable", "[name1] [name2] [name3] ... - Disables given settings.", args =>
    {
      if (args.Length < 2)
      {
        args.Context.AddString("Missing name.");
        return;
      }
      var entries = args[1].Split(' ').ToList();
      if (args[1] == "*")
        entries = OptionsFetcher();
      foreach (var arg in entries)
        SetEntry(arg, -1);
    }, optionsFetcher: OptionsFetcher);
    new Terminal.ConsoleCommand("esp_terrain", "[radius] - Inspects terrain.", args =>
    {
      if (!Player.m_localPlayer) return;
      var pos = Player.m_localPlayer.transform.position;
      var radius = args.TryParameterFloat(1, 10f);
      Visual.DrawHeightmap(pos, radius);
    });
    new Terminal.ConsoleCommand("esp_paint", "[radius] - Inspects paint.", args =>
    {
      if (!Player.m_localPlayer) return;
      var pos = Player.m_localPlayer.transform.position;
      var radius = args.TryParameterFloat(1, 10f);
      Visual.DrawPaintmap(pos, radius);
    });
    new Terminal.ConsoleCommand("esp_check", "[x,z] - Inspects terrain at specific coordinates.", args =>
    {
      if (!Player.m_localPlayer) return;
      if (args.Length < 2)
      {
        args.Context.AddString("Missing coordinates.");
        return;
      }
      var coords = args[1].Split(',');
      if (coords.Length < 2)
      {
        args.Context.AddString("Invalid coordinates.");
        return;
      }
      if (!float.TryParse(coords[0], out var x) || !float.TryParse(coords[1], out var z))
      {
        args.Context.AddString("Invalid coordinates.");
        return;
      }
      Vector3 pos = new(x, 0, z);
      var hm = Heightmap.FindHeightmap(pos);
      if (!hm)
      {
        args.Context.AddString("No heightmap found.");
        return;
      }
      var veg = hm.GetVegetationMask(pos);
      var lava = hm.IsLava(pos);
      var cultivated = hm.IsCultivated(pos);
      var cleared = hm.IsCleared(pos);
      args.Context.AddString($"Vegetation: {veg:F4}, Lava: {lava}, Cultivated: {cultivated}, Cleared: {cleared}");
    });
    new Terminal.ConsoleCommand("esp_custom", "[file] - Sets the custom text file.", args =>
    {
      if (args.Length < 2)
      {
        args.Context.AddString("Missing file.");
        return;
      }
      if (!FormatLoading.Keys.Contains(args[1]))
      {
        args.Context.AddString("Invalid file.");
        return;
      }
      configCustom.Value = args[1];
    }, optionsFetcher: () => FormatLoading.Keys);
    new Terminal.ConsoleCommand("esp_custom_prev", "Sets the previous custom text file.", args =>
    {
      var index = FormatLoading.Keys.IndexOf(configCustom.Value);
      if (index < 0) index = 0;
      else if (index == 0) index = FormatLoading.Keys.Count - 1;
      else index--;
      configCustom.Value = FormatLoading.Keys[index];
    });
    new Terminal.ConsoleCommand("esp_custom_next", "Sets the next custom text file.", args =>
    {
      var index = FormatLoading.Keys.IndexOf(configCustom.Value);
      if (index < 0) index = 0;
      else if (index == FormatLoading.Keys.Count - 1) index = 0;
      else index++;
      configCustom.Value = FormatLoading.Keys[index];
    });
  }
  private static List<string> OptionsFetcher()
  {
    List<string> options = [.. Visibility.GetTags];
    options = options.Where(tag => !tag.StartsWith(Tag.ZoneCorner) && !tag.StartsWith(Tag.SpawnZone) && !tag.StartsWith(Tag.Terrain)).ToList();
    // Collection tags are not listed automatically.
    options.Add(Tag.ZoneCorner);
    options.Add(Tag.SpawnZone);
    options.Add(Tool.ExtraInfo);
    options.Add(Tool.HUD);
    options.Add(Tool.Time);
    options.Add(Tool.Weather);
    options.Add(Tool.Wind);
    options.Add(Tool.Position);
    options.Add(Tool.Altitude);
    options.Add(Tool.Forest);
    options.Add(Tool.Blocked);
    options.Add(Tool.Stagger);
    options.Add(Tool.Heat);
    options.Add(Tool.Speed);
    options.Add(Tool.Stealth);
    options.Sort();
    return options;
  }
  private static ConfigEntry<bool> GetOtherEntry(string name)
  {
    name = name.ToLower();
    if (name == Tool.ExtraInfo.ToLower()) return configExtraInfo;
    if (name == Tool.HUD.ToLower()) return configShowHud;
    if (name == Tool.Time.ToLower()) return configShowTime;
    if (name == Tool.Weather.ToLower()) return configShowWeather;
    if (name == Tool.Wind.ToLower()) return configShowWind;
    if (name == Tool.Position.ToLower()) return configShowPosition;
    if (name == Tool.Altitude.ToLower()) return configShowAltitude;
    if (name == Tool.Forest.ToLower()) return configShowForest;
    if (name == Tool.Blocked.ToLower()) return configShowBlocked;
    if (name == Tool.Stagger.ToLower()) return configShowStagger;
    if (name == Tool.Heat.ToLower()) return configShowHeat;
    if (name == Tool.Speed.ToLower()) return configShowSpeed;
    if (name == Tool.Stealth.ToLower()) return configShowStealth;
    throw new NotImplementedException(name);
  }
  private static void SetEntry(string name, int value)
  {
    try
    {
      var entry = GetTagEntry(name);
      if (entry.Value != value)
        entry.Value = value;
      return;
    }
    catch (NotImplementedException) { }
    try
    {
      var entry = GetOtherEntry(name);
      if (entry.Value != value > 0)
        entry.Value = value > 0;
      return;
    }
    catch (NotImplementedException) { }
    throw new NotImplementedException(name);
  }
  private static void ToggleEntry(string name)
  {
    try
    {
      var entry = GetTagEntry(name);
      if (entry.Value < 0) return;
      entry.Value = entry.Value > 0 ? 0 : 1;
      return;
    }
    catch (NotImplementedException) { }
    try
    {
      var entry = GetOtherEntry(name);
      entry.Value = !entry.Value;
      return;
    }
    catch (NotImplementedException) { }
    throw new NotImplementedException(name);
  }
}
