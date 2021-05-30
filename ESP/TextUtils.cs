using System;
using System.Globalization;

namespace ESP
{
  public class TextUtils
  {
    private const string format = "0.##";
    public static String String(string value) => "<color=yellow>" + value + "</color>";
    public static String Float(double value) => String(value.ToString(format, CultureInfo.InvariantCulture));
    public static String Percent(double value) => String(value.ToString("P0", CultureInfo.InvariantCulture));

    public static String Range(double min, double max)
    {
      if (max > min)
        return String(min.ToString(format, CultureInfo.InvariantCulture) + "-" + max.ToString(format, CultureInfo.InvariantCulture));
      return String(max.ToString(format, CultureInfo.InvariantCulture));
    }
    public static String Progress(double value, double limit) => String(value.ToString("N0") + "/" + limit.ToString("N0"));
    public static String Int(double value) => String(value.ToString(format));
    public static String ProgressPercent(string header, double value, double limit) => header + ": " + Progress(value, limit) + " seconds (" + Percent(value / limit) + ")";
  }
}