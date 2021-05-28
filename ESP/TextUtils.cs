using System;
using System.Globalization;

namespace ESP
{
  public class TextUtils
  {
    public static String StringValue(String value) => "<color=yellow>" + value + "</color>";
    public static String FloatValue(float value) => StringValue(value.ToString("N1", CultureInfo.InvariantCulture));
    public static String PercentValue(float value) => StringValue(value.ToString("P0", CultureInfo.InvariantCulture));
    public static String IntValue(float value) => StringValue(value.ToString("N0"));
    public static String ProgressValue(float value, float limit, String header = "Progress") => header + ": " + StringValue(value.ToString("N0") + "/" + limit.ToString("N0") + " seconds") + "(" + PercentValue(value / limit) + ")";
  }
}