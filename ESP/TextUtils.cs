using System;
using System.Globalization;

namespace ESP
{
  public class TextUtils
  {
    public static String StringValue(string value) => "<color=yellow>" + value + "</color>";
    public static String FloatValue(float value) => StringValue(value.ToString("N1", CultureInfo.InvariantCulture));
    public static String PercentValue(float value) => StringValue(value.ToString("P0", CultureInfo.InvariantCulture));
    public static String IntValue(float value) => StringValue(value.ToString("N0"));
    public static String ProgressValue(string header, float value, float limit) => header + ": " + StringValue(value.ToString("N0") + "/" + limit.ToString("N0")) + " seconds (" + PercentValue(value / limit) + ")";
  }
}