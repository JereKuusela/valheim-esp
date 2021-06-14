using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;


namespace ESP
{
  public class TextUtils
  {
    public const string FORMAT = "0.##";

    public static string GetValidColor(bool valid) => valid ? "yellow" : "grey";
    public static string String(string value, string color = "yellow") => "<color=" + color + ">" + value + "</color>";
    public static string String(string value, bool valid) => "<color=" + GetValidColor(valid) + ">" + value + "</color>";
    public static string Float(double value, string format = FORMAT, string color = "yellow") => String(value.ToString(format, CultureInfo.InvariantCulture), color);
    public static string Multiplier(double value, string color = "yellow") => String(value.ToString(FORMAT, CultureInfo.InvariantCulture) + "x", color);
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
    public static string Range(double min, double max) => String(PlainRange(min, max));
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
        level = TextUtils.PlainRange(max, min);
      else
        level = TextUtils.PlainRange(min, max);
      var chanceText = level.Contains("-") ? " (" + TextUtils.Percent(chance / 100f) + " per star)" : "";
      var levelLimit = (limit > 0) ? " after " + TextUtils.Int(limit) + " meters" : "";
      return "Stars: " + String(level) + chanceText + levelLimit;
    }

    public static string GetBiomes(Heightmap.Biome biome, Heightmap.BiomeArea area = Heightmap.BiomeArea.Edge)
    {
      var biomeText = BiomeUtils.GetNames(biome);
      if (biomeText.Length == 0) return "";
      var biomeArea = (area == Heightmap.BiomeArea.Median) ? ", only full biomes" : "";
      return "Biomes: " + biomeText + biomeArea;
    }
    public static string GetAttempt(double time, double limit, double chance) =>
      TextUtils.ProgressPercent("Attempt", time, limit) + ", " + TextUtils.Percent(chance / 100.0) + " chance";

    public static string GetGlobalKeys(List<string> required, List<string> notRequired)
    {
      required = required.Select(key => String(key, ZoneSystem.instance.GetGlobalKey(key))).ToList();
      notRequired = notRequired.Select(key => String("not " + key, !ZoneSystem.instance.GetGlobalKey(key))).ToList();
      var keys = required.Concat(notRequired);
      return System.String.Join(", ", keys);
    }
    public static string GetHealth(double health, double limit)
      => "Health: " + TextUtils.Progress(health, limit) + " (" + TextUtils.Percent(health / limit) + ")";

    public static string Name(string name) => String(Localization.instance.Localize(name));
    public static string Name(GameObject obj) => Name(Utils.GetPrefabName(obj));
    public static string Name(Character obj) => Name(obj.m_name);
    public static string Name(ItemDrop.ItemData obj) => Name(obj.m_shared.m_name);

    public static string Radius(float radius) => "Radius: " + TextUtils.Float(radius);
  }
}