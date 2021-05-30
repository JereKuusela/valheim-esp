using System;
using System.Globalization;

namespace ESP
{
  public class TextUtils
  {
    private const string format = "0.##";
    public static string String(string value) => "<color=yellow>" + value + "</color>";
    public static string Float(double value) => String(value.ToString(format, CultureInfo.InvariantCulture));
    public static string Percent(double value) => String(value.ToString("P0", CultureInfo.InvariantCulture));

    public static string Range(double min, double max)
    {
      if (max > min)
        return String(min.ToString(format, CultureInfo.InvariantCulture) + "-" + max.ToString(format, CultureInfo.InvariantCulture));
      return String(max.ToString(format, CultureInfo.InvariantCulture));
    }
    public static string Progress(double value, double limit) => String(value.ToString("N0") + "/" + limit.ToString("N0"));
    public static string Int(double value) => String(value.ToString(format));
    public static string ProgressPercent(string header, double value, double limit) => header + ": " + Progress(value, limit) + " seconds (" + Percent(value / limit) + ")";
  }
}