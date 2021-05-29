using System;
using System.Globalization;

namespace ESP
{
  public class TextUtils
  {
    public static String StringValue(string value) => "<color=yellow>" + value + "</color>";
    public static String FloatValue(double value) => StringValue(value.ToString("N1", CultureInfo.InvariantCulture));
    public static String PercentValue(double value) => StringValue(value.ToString("P0", CultureInfo.InvariantCulture));
    public static String IntValue(double value) => StringValue(value.ToString("N0"));
    public static String ProgressValue(string header, double value, double limit) => header + ": " + StringValue(value.ToString("N0") + "/" + limit.ToString("N0")) + " seconds (" + PercentValue(value / limit) + ")";
  }
}