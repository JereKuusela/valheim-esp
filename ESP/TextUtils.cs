using System;
using System.Globalization;

namespace ESP
{
  public class TextUtils
  {
    public static String StringValue(String value) => "<color=yellow>" + value + "</color>";
    public static String FloatValue(float value) => StringValue(value.ToString("N1", CultureInfo.InvariantCulture));
    public static String IntValue(float value) => StringValue(value.ToString("N0"));
  }
}