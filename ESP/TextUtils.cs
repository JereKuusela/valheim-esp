using System;
using System.Globalization;

namespace ESP
{
  public class TextUtils
  {
    private const string FORMAT = "0.##";

    public static string String(string value) => "<color=yellow>" + value + "</color>";
    public static string Float(double value, string format = FORMAT) => String(value.ToString(format, CultureInfo.InvariantCulture));
    public static string Fixed(double value)
    {
      return String(value.ToString("N2", CultureInfo.InvariantCulture).PadLeft(5, '0'));
    }
    public static string Percent(double value) => String(value.ToString("P0", CultureInfo.InvariantCulture));

    public static string Range(double min, double max)
    {
      if (max > min)
        return String(min.ToString(FORMAT, CultureInfo.InvariantCulture) + "-" + max.ToString(FORMAT, CultureInfo.InvariantCulture));
      return String(max.ToString(FORMAT, CultureInfo.InvariantCulture));
    }
    public static string Progress(double value, double limit) => String(value.ToString("N0") + "/" + limit.ToString("N0"));
    public static string Int(double value) => String(value.ToString("N0"));
    public static string ProgressPercent(string header, double value, double limit) => header + ": " + Progress(value, limit) + " seconds (" + Percent(value / limit) + ")";
  }
}