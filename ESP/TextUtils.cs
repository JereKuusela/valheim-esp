using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;


namespace ESP
{
  public class Format
  {
    public const string FORMAT = "0.##";

    public static string GetValidColor(bool valid) => valid ? "yellow" : "grey";
    public static string String(string value, string color = "yellow") => "<color=" + color + ">" + value + "</color>";
    public static string String(string value, bool valid) => "<color=" + GetValidColor(valid) + ">" + value + "</color>";
    public static string Float(double value, string format = FORMAT, string color = "yellow") => String(value.ToString(format, CultureInfo.InvariantCulture), color);
    public static string Multiplier(double value, string color = "yellow") => String(value.ToString(FORMAT, CultureInfo.InvariantCulture) + "x", color);
    public static string Meters(double value, string color = "yellow") => String(value.ToString(FORMAT, CultureInfo.InvariantCulture) + " meters", color);
    public static string Degrees(double value, string color = "yellow") => String(value.ToString(FORMAT, CultureInfo.InvariantCulture) + " degrees", color);
    public static string Fixed(double value)
    {
      return String(value.ToString("N2", CultureInfo.InvariantCulture).PadLeft(5, '0'));
    }
    public static string Percent(double value) => String(value.ToString("P0", CultureInfo.InvariantCulture));

    private static string PlainRange(double min, double max)
    {
      if (min == max)
        return max.ToString(FORMAT, CultureInfo.InvariantCulture);
      return min.ToString(FORMAT, CultureInfo.InvariantCulture) + "-" + max.ToString(FORMAT, CultureInfo.InvariantCulture);
    }
    public static string Range(double min, double max, string color = "yellow") => String(PlainRange(min, max), color);
    public static string PercentRange(double min, double max)
    {
      if (min == max)
        return max.ToString("P0", CultureInfo.InvariantCulture);
      return min.ToString("P0", CultureInfo.InvariantCulture) + "-" + max.ToString("P0", CultureInfo.InvariantCulture);
    }
    public static string Progress(double value, double limit) => String(value.ToString("N0")) + "/" + String(limit.ToString("N0"));
    public static string Int(double value, string color = "yellow") => String(value.ToString("N0"), color);
    public static string ProgressPercent(string header, double value, double limit) => header + ": " + Progress(value, limit) + " seconds (" + Percent(value / limit) + ")";

    public static string GetLevel(int min, int max, double chance, double limit = 0)
    {
      min--;
      max--;
      var level = "";
      if (max < min && Settings.fixInvalidLevelData)
        level = Format.PlainRange(max, min);
      else
        level = Format.PlainRange(min, max);
      var chanceText = level.Contains("-") ? " (" + Format.Percent(chance / 100f) + " per star)" : "";
      var levelLimit = (limit > 0) ? " after " + Format.Int(limit) + " meters" : "";
      return "Stars: " + String(level) + chanceText + levelLimit;
    }

    public static string GetAttempt(double time, double limit, double chance) =>
      Format.ProgressPercent("Attempt", time, limit) + ", " + Format.Percent(chance / 100.0) + " chance";

    public static string GetGlobalKeys(List<string> required, List<string> notRequired)
    {
      required = required.Select(key => String(key, ZoneSystem.instance.GetGlobalKey(key))).ToList();
      notRequired = notRequired.Select(key => String("not " + key, !ZoneSystem.instance.GetGlobalKey(key))).ToList();
      var keys = required.Concat(notRequired);
      return System.String.Join(", ", keys);
    }
    public static string GetHealth(double health, double limit)
      => "Health: " + Format.Progress(health, limit) + " (" + Format.Percent(health / limit) + ")";

    public static string Name(string name, string color = "yellow") => String(Localization.instance.Localize(name), color);
    public static string Name(GameObject obj, string color = "yellow") => Name(Utils.GetPrefabName(obj), color);
    public static string Name(Character obj, string color = "yellow") => Name(obj.m_name, color);
    public static string Name(ItemDrop.ItemData obj, string color = "yellow") => Name(obj.m_shared.m_name, color);
    public static string Name(Heightmap.Biome obj, string color = "yellow") => Name(Texts.GetName(obj), color);
    public static string Name(Pickable obj, string color = "yellow") => Name(obj.m_itemPrefab.name, color);

    public static string Radius(float radius) => "Radius: " + Format.Float(radius);
  }
}