using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
namespace Service;
public class Format
{
  public const string FORMAT = "0.##";

  public static string GetValidColor(bool valid) => valid ? "#FFFF00" : "#B2BEB5";
  public static string String(string value, string color = "#FFFF00") => "<color=" + color + ">" + value + "</color>";
  public static string String(string value, bool valid) => "<color=" + GetValidColor(valid) + ">" + value + "</color>";
  public static string Float(double value, string format = FORMAT, string color = "#FFFF00") => String(value.ToString(format, CultureInfo.InvariantCulture), color);
  public static string Multiplier(double value, string color = "#FFFF00") => String(value.ToString(FORMAT, CultureInfo.InvariantCulture) + "x", color);
  public static string Meters(double value, string color = "#FFFF00") => String(value.ToString(FORMAT, CultureInfo.InvariantCulture) + " meters", color);
  public static string Degrees(double value, string color = "#FFFF00") => String(value.ToString(FORMAT, CultureInfo.InvariantCulture) + " degrees", color);
  public static string Fixed(double value)
  {
    return String(value.ToString("N2", CultureInfo.InvariantCulture).PadLeft(5, '0'));
  }
  public static string Percent(double value, string color = "#FFFF00") => String((100.0 * value).ToString(FORMAT, CultureInfo.InvariantCulture) + " %", color);
  public static string PercentInt(double value, string color = "#FFFF00") => String(value.ToString("P0", CultureInfo.InvariantCulture), color);

  public static string Range(double min, double max, string color = "#FFFF00")
  {
    if (min == max)
      return String(max.ToString(FORMAT, CultureInfo.InvariantCulture), color);
    return String(min.ToString(FORMAT, CultureInfo.InvariantCulture), color) + "-" + String(max.ToString(FORMAT, CultureInfo.InvariantCulture), color);
  }
  public static string PercentRange(double min, double max)
  {
    if (min == max)
      return max.ToString("P0", CultureInfo.InvariantCulture);
    return min.ToString("P0", CultureInfo.InvariantCulture) + "-" + max.ToString("P0", CultureInfo.InvariantCulture);
  }
  public static string Progress(double value, double limit, bool percent = false) => String(value.ToString("N0")) + "/" + String(limit.ToString("N0")) + (percent ? " (" + PercentInt(value / limit) + ")" : "");
  public static string Int(double value, string color = "#FFFF00") => String(value.ToString("N0"), color);
  public static string ProgressPercent(string header, double value, double limit) => header + ": " + Progress(value, limit) + " seconds (" + Percent(value / limit) + ")";
  public static string Coordinates(Vector3 coordinates, string format = "F0", string color = "#FFFF00")
  {
    Vector3 swapped = new(coordinates.x, coordinates.z, coordinates.y);
    var values = swapped.ToString(format).Replace("(", "").Replace(")", "").Split(',').Select(value => Format.String(value.Trim(), color));
    return Format.JoinRow(values);
  }
  public static string JoinLines(IEnumerable<string> lines) => string.Join("\n", lines.Where(line => line != ""));
  public static string JoinRow(IEnumerable<string> lines) => string.Join(", ", lines.Where(line => line != ""));
}
