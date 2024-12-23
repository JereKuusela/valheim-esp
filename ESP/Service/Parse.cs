using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
namespace Service;

public class Range<T>
{
  public T Min;
  public T Max;
  public Range(T value)
  {
    Min = value;
    Max = value;
  }
  public Range(T min, T max)
  {
    Min = min;
    Max = max;
  }
  public bool Uniform = true;
}

///<summary>Contains functions for parsing arguments, etc.</summary>
public static class Parse
{
  public static List<string> ToList(string str, bool removeEmpty = true) => [.. Split(str, removeEmpty)];
  public static Vector2i Vector2Int(string arg)
  {
    string[] array = SplitWithEmpty(arg);
    return new Vector2i(Int(array[0]), (array.Length > 1) ? Int(array[1]) : 0);
  }
  public static int Int(string arg, int defaultValue = 0)
  {
    if (!TryInt(arg, out var result))
      return defaultValue;
    return result;
  }
  public static uint UInt(string arg, uint defaultValue = 0)
  {
    if (!TryUInt(arg, out var result))
      return defaultValue;
    return result;
  }

  public static long Long(string arg, long defaultValue = 0)
  {
    if (!long.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
      return defaultValue;
    return result;
  }
  public static bool TryLong(string arg, out long result)
  {
    return long.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
  }
  public static int Int(string[] args, int index, int defaultValue = 0)
  {
    if (args.Length <= index) return defaultValue;
    return Int(args[index], defaultValue);
  }
  public static bool TryUInt(string arg, out uint result)
  {
    return uint.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
  }
  public static bool TryInt(string arg, out int result)
  {
    return int.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
  }
  public static float Float(string arg, float defaultValue = 0f)
  {
    if (!TryFloat(arg, out var result))
      return defaultValue;
    return result;
  }
  public static float Float(string[] args, int index, float defaultValue = 0f)
  {
    if (args.Length <= index) return defaultValue;
    return Float(args[index], defaultValue);
  }
  public static bool TryFloat(string arg, out float result)
  {
    return float.TryParse(arg, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
  }
  public static bool TryBoolean(string arg, out bool result)
  {
    result = false;
    if (arg.ToLowerInvariant() == "true")
    {
      result = true;
      return true;
    }
    if (arg.ToLowerInvariant() == "false")
    {
      return true;
    }
    return false;
  }

  public static Quaternion AngleYXZ(string arg) => AngleYXZ(Split(arg), 0, Vector3.zero);
  public static Quaternion AngleYXZ(string[] args, int index) => AngleYXZ(args, index, Vector3.zero);
  public static Quaternion AngleYXZ(string[] args, int index, Vector3 defaultValue)
  {
    var vector = Vector3.zero;
    vector.y = Float(args, index, defaultValue.y);
    vector.x = Float(args, index + 1, defaultValue.x);
    vector.z = Float(args, index + 2, defaultValue.z);
    return Quaternion.Euler(vector);
  }

  public static string[] Split(string arg, bool removeEmpty = true, char split = ',') => arg.Split(split).Select(s => s.Trim()).Where(s => !removeEmpty || s != "").ToArray();
  public static KeyValuePair<string, string> Kvp(string str, char separator = ',')
  {
    var split = str.Split([separator], 2);
    if (split.Length < 2) return new(split[0], "");
    return new(split[0], split[1].Trim());
  }
  public static bool TryKvp(string str, out KeyValuePair<string, string> kvp, char separator = ',')
  {
    kvp = Kvp(str, separator);
    return kvp.Value != "";
  }
  public static string[] SplitWithEmpty(string arg, char split = ',') => arg.Split(split).Select(s => s.Trim()).ToArray();
  public static string[] SplitWithEscape(string arg, char separator = ',')
  {
    var parts = new List<string>();
    var split = arg.Split(separator);
    for (var i = 0; i < split.Length; i++)
    {
      var part = split[i].TrimStart();
      // Escape should only work if at start/end of the string.
      if (part.StartsWith("\""))
      {
        split[i] = part.Substring(1);
        var j = i;
        for (; j < split.Length; j++)
        {
          part = split[j].TrimEnd();
          if (part.EndsWith("\""))
          {
            split[j] = part.Substring(0, part.Length - 1);
            break;
          }
        }
        parts.Add(string.Join(separator.ToString(), split.Skip(i).Take(j - i + 1)));
        i = j;
        continue;
      }
      parts.Add(split[i].Trim());
    }
    return [.. parts];
  }
  public static string Name(string arg) => arg.Split(':')[0];
  public static Vector3 VectorXZY(string arg) => VectorXZY(arg, Vector3.zero);
  public static Vector3 VectorXZY(string arg, Vector3 defaultValue) => VectorXZY(Split(arg), 0, defaultValue);

  ///<summary>Parses YXZ vector starting at given index. Zero is used for missing values.</summary>
  public static Vector3 VectorXZY(string[] args, int index) => VectorXZY(args, index, Vector3.zero);
  ///<summary>Parses YXZ vector starting at given index. Default values is used for missing values.</summary>
  public static Vector3 VectorXZY(string[] args, int index, Vector3 defaultValue)
  {
    var vector = Vector3.zero;
    vector.x = Float(args, index, defaultValue.x);
    vector.z = Float(args, index + 1, defaultValue.z);
    vector.y = Float(args, index + 2, defaultValue.y);
    return vector;
  }
  public static Vector2 VectorXY(string arg)
  {
    var vector = Vector2.zero;
    var args = Split(arg);
    vector.x = Float(args, 0);
    vector.y = Float(args, 1);
    return vector;
  }
  public static Vector3 Scale(string args) => Scale(Split(args), 0);
  ///<summary>Parses scale starting at zero index. Includes a sanity check and giving a single value for all axis.</summary>
  public static Vector3 Scale(string[] args) => Scale(args, 0);
  ///<summary>Parses scale starting at given index. Includes a sanity check and giving a single value for all axis.</summary>
  public static Vector3 Scale(string[] args, int index) => SanityCheck(VectorXZY(args, index));
  private static Vector3 SanityCheck(Vector3 scale)
  {
    // Sanity check and also adds support for setting all values with a single number.
    if (scale.x == 0) scale.x = 1;
    if (scale.y == 0) scale.y = scale.x;
    if (scale.z == 0) scale.z = scale.x;
    return scale;
  }

  public static Range<string> StringRange(string arg)
  {
    var range = arg.Split(['-', ';']).ToList();
    if (range.Count > 1 && range[0] == "")
    {
      range[0] = "-" + range[1];
      range.RemoveAt(1);
    }
    if (range.Count > 2 && range[1] == "")
    {
      range[1] = "-" + range[2];
      range.RemoveAt(2);
    }
    if (range.Count == 1) return new(range[0]);
    else return new(range[0], range[1]);

  }
  public static Range<int> IntRange(string arg)
  {
    var range = StringRange(arg);
    return new(Int(range.Min), Int(range.Max));
  }
  public static Range<float> FloatRange(string arg)
  {
    var range = StringRange(arg);
    return new(Float(range.Min), Float(range.Max));
  }
  public static Range<long> LongRange(string arg)
  {
    var range = StringRange(arg);
    return new(Long(range.Min), Long(range.Max));
  }
  public static int? IntNull(string arg)
  {
    if (int.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
      return result;
    return null;
  }
  public static int? IntNull(string[] args, int index)
  {
    if (args.Length <= index) return null;
    return IntNull(args[index]);
  }
  public static float? FloatNull(string arg)
  {
    if (float.TryParse(arg, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
      return result;
    return null;
  }
  public static float? FloatNull(string[] args, int index)
  {
    if (args.Length <= index) return null;
    return FloatNull(args[index]);
  }
  public static long? LongNull(string[] args, int index)
  {
    if (args.Length <= index) return null;
    return LongNull(args[index]);
  }
  public static long? LongNull(string arg)
  {
    if (long.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
      return result;
    return null;
  }
  public static Vector3? VectorXZYNull(string? arg) => arg == null ? null : VectorXZYNull(Split(arg));
  public static Vector3? VectorXZYNull(string[] args)
  {
    var x = FloatNull(args, 0);
    var y = FloatNull(args, 2);
    var z = FloatNull(args, 1);
    if (x == null && y == null && z == null) return null;
    return new(x ?? 0f, y ?? 0f, z ?? 0f);
  }
  public static Quaternion? AngleYXZNull(string? arg) => arg == null ? null : AngleYXZNull(Split(arg));
  public static Quaternion? AngleYXZNull(string[] values)
  {
    var y = FloatNull(values, 0);
    var x = FloatNull(values, 1);
    var z = FloatNull(values, 2);
    if (y == null && x == null && z == null) return null;
    return Quaternion.Euler(new(x ?? 0f, y ?? 0f, z ?? 0f));
  }
  public static string String(string[] args, int index) => args.Length > index ? args[index] : "";
  public static int Hash(string[] args, int index) => args.Length > index ? args[index].GetStableHashCode() : 0;
  public static bool Boolean(string[] args, int index) => args.Length > index && Boolean(args[index]);
  public static bool Boolean(string arg) => arg.ToLowerInvariant() == "true";
  public static bool BooleanTrue(string arg) => arg.ToLowerInvariant() == "false";
  public static ZDOID ZdoId(string arg)
  {
    var split = Split(arg, true, ':');
    if (split.Length < 2) return ZDOID.None;
    return new ZDOID(Long(split[0]), UInt(split[1]));
  }
}
