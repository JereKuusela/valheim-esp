using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Service;
using UnityEngine;

namespace Data;

// Parameters are technically just a key-value mapping.
// Proper class allows properly adding caching and other features.
// While also ensuring that all code is in one place.
public class Parameters(string prefab, string arg, Vector3 pos)
{
  public static Func<string, string?> ExecuteCode = (string key) => null!;
  public static Func<string, string, string?> ExecuteCodeWithValue = (string key, string value) => null!;

  protected string[]? args;
  private readonly double time = ZNet.instance.GetTimeSeconds();

  public string Replace(string str)
  {
    StringBuilder parts = new();
    int nesting = 0;
    var start = 0;
    for (int i = 0; i < str.Length; i++)
    {
      if (str[i] == '<')
      {
        if (nesting == 0)
        {
          parts.Append(str.Substring(start, i - start));
          start = i;
        }
        nesting++;

      }
      if (str[i] == '>')
      {
        if (nesting == 1)
        {
          var key = str.Substring(start, i - start + 1);
          parts.Append(ResolveParameters(key));
          start = i + 1;
        }
        if (nesting > 0)
          nesting--;
      }
    }
    if (start < str.Length)
      parts.Append(str.Substring(start));

    return parts.ToString();
  }
  private string ResolveParameters(string str)
  {
    for (int i = 0; i < str.Length; i++)
    {
      var end = str.IndexOf(">", i);
      if (end == -1) break;
      i = end;
      var start = str.LastIndexOf("<", end);
      if (start == -1) continue;
      var length = end - start + 1;
      if (TryReplaceParameter(str.Substring(start, length), out var resolved))
      {
        str = str.Remove(start, length);
        str = str.Insert(start, resolved);
        // Resolved could contain parameters, so need to recheck the same position.
        i = start - 1;
      }
      else
      {
        i = end;
      }
    }
    return str;
  }
  private bool TryReplaceParameter(string key, out string resolved)
  {
    resolved = GetParameter(key) ?? key;
    return resolved != key;
  }

  protected virtual string? GetParameter(string key)
  {
    var value = ExecuteCode(key.Substring(1, key.Length - 2));
    if (value != null) return value;
    value = GetGeneralParameter(key);
    if (value != null) return value;
    var kvp = Parse.Kvp(key, '_');
    if (kvp.Value == "") return null;
    key = kvp.Key.Substring(1);
    var keyValue = kvp.Value.Substring(0, kvp.Value.Length - 1);
    var kvp2 = Parse.Kvp(keyValue, '=');
    keyValue = kvp2.Key;
    var defaultValue = kvp2.Value;

    value = ExecuteCodeWithValue(key, keyValue);
    if (value != null) return value;
    return GetValueParameter(key, keyValue, defaultValue);
  }

  private string? GetGeneralParameter(string key) =>
    key switch
    {
      "<prefab>" => prefab,
      "<par>" => arg,
      "<par0>" => GetArg(0),
      "<par1>" => GetArg(1),
      "<par2>" => GetArg(2),
      "<par3>" => GetArg(3),
      "<par4>" => GetArg(4),
      "<par5>" => GetArg(5),
      "<par6>" => GetArg(6),
      "<par7>" => GetArg(7),
      "<par8>" => GetArg(8),
      "<par9>" => GetArg(9),
      "<time>" => Format(time),
      "<day>" => EnvMan.instance.GetDay(time).ToString(),
      "<ticks>" => ((long)(time * 10000000.0)).ToString(),
      "<x>" => Format(pos.x),
      "<y>" => Format(pos.y),
      "<z>" => Format(pos.z),
      "<snap>" => Format(WorldGenerator.instance.GetHeight(pos.x, pos.z)),
      _ => null,
    };

  protected virtual string? GetValueParameter(string key, string value, string defaultValue) =>
   key switch
   {
     "sqrt" => Parse.TryFloat(value, out var f) ? Mathf.Sqrt(f).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "round" => Parse.TryFloat(value, out var f) ? Mathf.Round(f).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "ceil" => Parse.TryFloat(value, out var f) ? Mathf.Ceil(f).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "floor" => Parse.TryFloat(value, out var f) ? Mathf.Floor(f).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "abs" => Parse.TryFloat(value, out var f) ? Mathf.Abs(f).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "sin" => Parse.TryFloat(value, out var f) ? Mathf.Sin(f).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "cos" => Parse.TryFloat(value, out var f) ? Mathf.Cos(f).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "tan" => Parse.TryFloat(value, out var f) ? Mathf.Tan(f).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "asin" => Parse.TryFloat(value, out var f) ? Mathf.Asin(f).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "acos" => Parse.TryFloat(value, out var f) ? Mathf.Acos(f).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "atan" => Atan(value, defaultValue),
     "pow" => Parse.TryKvp(value, out var kvp, '_') && Parse.TryFloat(kvp.Key, out var f1) && Parse.TryFloat(kvp.Value, out var f2) ? Mathf.Pow(f1, f2).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "log" => Log(value, defaultValue),
     "exp" => Parse.TryFloat(value, out var f) ? Mathf.Exp(f).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "min" => Parse.TryKvp(value, out var kvp, '_') && Parse.TryFloat(kvp.Key, out var f1) && Parse.TryFloat(kvp.Value, out var f2) ? Mathf.Min(f1, f2).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "max" => Parse.TryKvp(value, out var kvp, '_') && Parse.TryFloat(kvp.Key, out var f1) && Parse.TryFloat(kvp.Value, out var f2) ? Mathf.Max(f1, f2).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "add" => Parse.TryKvp(value, out var kvp, '_') && Parse.TryFloat(kvp.Key, out var f1) && Parse.TryFloat(kvp.Value, out var f2) ? (f1 + f2).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "sub" => Parse.TryKvp(value, out var kvp, '_') && Parse.TryFloat(kvp.Key, out var f1) && Parse.TryFloat(kvp.Value, out var f2) ? (f1 - f2).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "mul" => Parse.TryKvp(value, out var kvp, '_') && Parse.TryFloat(kvp.Key, out var f1) && Parse.TryFloat(kvp.Value, out var f2) ? (f1 * f2).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "div" => Parse.TryKvp(value, out var kvp, '_') && Parse.TryFloat(kvp.Key, out var f1) && Parse.TryFloat(kvp.Value, out var f2) ? (f1 / f2).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "mod" => Parse.TryKvp(value, out var kvp, '_') && Parse.TryFloat(kvp.Key, out var f1) && Parse.TryFloat(kvp.Value, out var f2) ? (f1 % f2).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "randf" => Parse.TryKvp(value, out var kvp, '_') && Parse.TryFloat(kvp.Key, out var f1) && Parse.TryFloat(kvp.Value, out var f2) ? UnityEngine.Random.Range(f1, f2).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "randi" => Parse.TryKvp(value, out var kvp, '_') && Parse.TryInt(kvp.Key, out var i1) && Parse.TryInt(kvp.Value, out var i2) ? UnityEngine.Random.Range(i1, i2).ToString(CultureInfo.InvariantCulture) : defaultValue,
     "hash" => Hash(value).ToString(),
     "len" => value.Length.ToString(CultureInfo.InvariantCulture),
     "lower" => value.ToLowerInvariant(),
     "upper" => value.ToUpperInvariant(),
     "trim" => value.Trim(),
     "calcf" => Calculator.EvaluateFloat(value)?.ToString(CultureInfo.InvariantCulture) ?? defaultValue,
     "calci" => Calculator.EvaluateInt(value)?.ToString(CultureInfo.InvariantCulture) ?? defaultValue,
     "par" => Parse.TryInt(value, out var i) ? GetArg(i, defaultValue) : defaultValue,
     "rest" => Parse.TryInt(value, out var i) ? GetRest(i, defaultValue) : defaultValue,
     _ => null,
   };


  private string GetRest(int index, string defaultValue = "")
  {
    args ??= arg.Split(' ');
    if (index < 0 || index >= args.Length) return defaultValue;
    return string.Join(" ", args, index, args.Length - index);
  }

  private string Atan(string value, string defaultValue)
  {
    var kvp = Parse.Kvp(value, '_');
    if (!Parse.TryFloat(kvp.Key, out var f1)) return defaultValue;
    if (kvp.Value == "") return Mathf.Atan(f1).ToString(CultureInfo.InvariantCulture);
    if (!Parse.TryFloat(kvp.Value, out var f2)) return defaultValue;
    return Mathf.Atan2(f1, f2).ToString(CultureInfo.InvariantCulture);
  }

  private string Log(string value, string defaultValue)
  {
    var kvp = Parse.Kvp(value, '_');
    if (!Parse.TryFloat(kvp.Key, out var f1)) return defaultValue;
    if (kvp.Value == "") return Mathf.Log(f1).ToString(CultureInfo.InvariantCulture);
    if (!Parse.TryFloat(kvp.Value, out var f2)) return defaultValue;
    return Mathf.Log(f1, f2).ToString(CultureInfo.InvariantCulture);
  }

  private string GetArg(int index, string defaultValue = "")
  {
    args ??= arg.Split(' ');
    return args.Length <= index ? defaultValue : args[index];
  }
  protected static string Format(float value) => value.ToString("0.#####", NumberFormatInfo.InvariantInfo);
  protected static string Format(double value) => value.ToString("0.#####", NumberFormatInfo.InvariantInfo);


  public static string PrintVectorXZY(Vector3 vector)
  {
    return vector.x.ToString("0.##", CultureInfo.InvariantCulture) + " " + vector.z.ToString("0.##", CultureInfo.InvariantCulture) + " " + vector.y.ToString("0.##", CultureInfo.InvariantCulture);
  }
  public static string PrintAngleYXZ(Quaternion quaternion)
  {
    return PrintVectorYXZ(quaternion.eulerAngles);
  }
  private static string PrintVectorYXZ(Vector3 vector)
  {
    return vector.y.ToString("0.##", CultureInfo.InvariantCulture) + " " + vector.x.ToString("0.##", CultureInfo.InvariantCulture) + " " + vector.z.ToString("0.##", CultureInfo.InvariantCulture);
  }
  public static int Hash(string key)
  {
    if (Parse.TryInt(key, out var result)) return result;
    if (key.StartsWith("$", StringComparison.InvariantCultureIgnoreCase))
    {
      var hash = ZSyncAnimation.GetHash(key.Substring(1));
      if (key == "$anim_speed") return hash;
      return 438569 + hash;
    }
    return key.GetStableHashCode();
  }
  public static string GetGlobalKey(string key)
  {
    var lower = key.ToLowerInvariant();
    return ZoneSystem.instance.m_globalKeysValues.FirstOrDefault(kvp => kvp.Key.ToLowerInvariant() == lower).Value ?? "0";
  }
}
public class ObjectParameters(string prefab, string arg, ZDO zdo) : Parameters(prefab, arg, zdo.m_position)
{
  private Inventory? inventory;


  protected override string? GetParameter(string key)
  {
    var value = base.GetParameter(key);
    if (value != null) return value;
    value = GetGeneralParameter(key);
    if (value != null) return value;
    var kvp = Parse.Kvp(key, '_');
    if (kvp.Value == "") return null;
    key = kvp.Key.Substring(1);
    var keyValue = kvp.Value.Substring(0, kvp.Value.Length - 1);
    var kvp2 = Parse.Kvp(keyValue, '=');
    keyValue = kvp2.Key;
    var defaultValue = kvp2.Value;

    value = ExecuteCodeWithValue(key, keyValue);
    if (value != null) return value;
    value = base.GetValueParameter(key, keyValue, defaultValue);
    if (value != null) return value;
    return GetValueParameter(key, keyValue, defaultValue);
  }

  private string? GetGeneralParameter(string key) =>
    key switch
    {
      "<zdo>" => zdo.m_uid.ToString(),
      "<pos>" => $"{Format(zdo.m_position.x)},{Format(zdo.m_position.z)},{Format(zdo.m_position.y)}",
      "<i>" => ZoneSystem.GetZone(zdo.m_position).x.ToString(),
      "<j>" => ZoneSystem.GetZone(zdo.m_position).y.ToString(),
      "<a>" => Format(zdo.m_rotation.y),
      "<rot>" => $"{Format(zdo.m_rotation.y)},{Format(zdo.m_rotation.x)},{Format(zdo.m_rotation.z)}",
      "<pid>" => GetPid(zdo),
      "<pname>" => GetPname(zdo),
      "<pchar>" => GetPchar(zdo),
      "<owner>" => zdo.GetOwner().ToString(),
      _ => null,
    };

  private static string GetPid(ZDO zdo)
  {
    var peer = GetPeer(zdo);
    if (peer != null)
      return peer.m_rpc.GetSocket().GetHostName();
    else if (Player.m_localPlayer)
      return "Server";
    return "";
  }
  private static string GetPname(ZDO zdo)
  {
    var peer = GetPeer(zdo);
    if (peer != null)
      return peer.m_playerName;
    else if (Player.m_localPlayer)
      return Player.m_localPlayer.GetPlayerName();
    return "";
  }
  private static string GetPchar(ZDO zdo)
  {
    var peer = GetPeer(zdo);
    if (peer != null)
      return peer.m_characterID.ToString();
    else if (Player.m_localPlayer)
      return Player.m_localPlayer.GetPlayerID().ToString();
    return "";
  }
  private static ZNetPeer? GetPeer(ZDO zdo) => zdo.GetOwner() != 0 ? ZNet.instance.GetPeer(zdo.GetOwner()) : null;


  protected override string? GetValueParameter(string key, string value, string defaultValue) =>
   key switch
   {
     "key" => GetGlobalKey(value),
     "string" => GetString(value, defaultValue),
     "float" => GetFloat(value, defaultValue).ToString(CultureInfo.InvariantCulture),
     "int" => GetInt(value, defaultValue).ToString(CultureInfo.InvariantCulture),
     "long" => GetLong(value, defaultValue).ToString(CultureInfo.InvariantCulture),
     "bool" => GetBool(value, defaultValue) ? "true" : "false",
     "hash" => ZNetScene.instance.GetPrefab(zdo.GetInt(value))?.name ?? "",
     "vec" => PrintVectorXZY(GetVec3(value, defaultValue)),
     "quat" => PrintAngleYXZ(GetQuaternion(value, defaultValue)),
     "byte" => Convert.ToBase64String(zdo.GetByteArray(value)),
     "zdo" => zdo.GetZDOID(value).ToString(),
     "item" => GetAmountOfItems(value).ToString(),
     "pos" => PrintVectorXZY(GetPos(value)),
     "pdata" => GetPlayerData(zdo, value),
     _ => null,
   };

  private string GetString(string value, string defaultValue) => ZDOExtraData.s_strings.TryGetValue(zdo.m_uid, out var data) && data.TryGetValue(Hash(value), out var str) ? str : GetStringField(value, defaultValue);
  private float GetFloat(string value, string defaultValue) => ZDOExtraData.s_floats.TryGetValue(zdo.m_uid, out var data) && data.TryGetValue(Hash(value), out var f) ? f : GetFloatField(value, defaultValue);
  private int GetInt(string value, string defaultValue) => ZDOExtraData.s_ints.TryGetValue(zdo.m_uid, out var data) && data.TryGetValue(Hash(value), out var i) ? i : GetIntField(value, defaultValue);
  private long GetLong(string value, string defaultValue) => ZDOExtraData.s_longs.TryGetValue(zdo.m_uid, out var data) && data.TryGetValue(Hash(value), out var l) ? l : GetLongField(value, defaultValue);
  private bool GetBool(string value, string defaultValue) => ZDOExtraData.s_ints.TryGetValue(zdo.m_uid, out var data) && data.TryGetValue(Hash(value), out var b) ? b > 0 : GetBoolField(value, defaultValue);
  private Vector3 GetVec3(string value, string defaultValue) => ZDOExtraData.s_vec3.TryGetValue(zdo.m_uid, out var data) && data.TryGetValue(Hash(value), out var v) ? v : GetVecField(value, defaultValue);
  private Quaternion GetQuaternion(string value, string defaultValue) => ZDOExtraData.s_quats.TryGetValue(zdo.m_uid, out var data) && data.TryGetValue(Hash(value), out var q) ? q : GetQuatField(value, defaultValue);



  private string GetStringField(string value, string defaultValue) => GetField(value) is string s ? s : defaultValue;
  private float GetFloatField(string value, string defaultValue) => GetField(value) is float f ? f : Parse.Float(defaultValue);
  private int GetIntField(string value, string defaultValue) => GetField(value) is int i ? i : Parse.Int(defaultValue);
  private bool GetBoolField(string value, string defaultValue) => GetField(value) is bool b ? b : Parse.Boolean(defaultValue);
  private long GetLongField(string value, string defaultValue) => GetField(value) is long l ? l : Parse.Long(defaultValue);
  private Vector3 GetVecField(string value, string defaultValue) => GetField(value) is Vector3 v ? v : Parse.VectorXZY(defaultValue);
  private Quaternion GetQuatField(string value, string defaultValue) => GetField(value) is Quaternion q ? q : Parse.AngleYXZ(defaultValue);

  private object? GetField(string value)
  {
    var kvp = Parse.Kvp(value, '.');
    if (kvp.Value == "") return null;
    var prefab = ZNetScene.instance.GetPrefab(zdo.m_prefab);
    if (prefab == null) return null;
    // Reflection to get the component and field.
    var component = prefab.GetComponent(kvp.Key);
    if (component == null) return null;
    var fields = kvp.Value.Split('.');
    object result = component;
    foreach (var field in fields)
    {
      var fieldInfo = result.GetType().GetField(field);
      if (fieldInfo == null) return null;
      result = fieldInfo.GetValue(result);
      if (result == null) return null;
    }
    return result;
  }

  private int GetAmountOfItems(string prefab)
  {
    LoadInventory();
    if (inventory == null) return 0;
    int count = 0;
    foreach (var item in inventory.m_inventory)
    {
      if ((item.m_dropPrefab?.name ?? item.m_shared.m_name) == prefab) count += item.m_stack;
    }
    return count;
  }

  private void LoadInventory()
  {
    if (inventory != null) return;
    var currentItems = zdo.GetString(ZDOVars.s_items);
    if (currentItems == "") return;
    inventory = new("", null, 4, 2);
    inventory.Load(new ZPackage(currentItems));
  }

  private Vector3 GetPos(string value)
  {
    var offset = Parse.VectorXZY(value);
    return zdo.GetPosition() + zdo.GetRotation() * offset;
  }

  public static string GetPlayerData(ZDO zdo, string key)
  {
    var peer = GetPeer(zdo);
    if (peer != null)
      return peer.m_serverSyncedPlayerData.TryGetValue(key, out var data) ? data : "";
    else if (Player.m_localPlayer)
      return ZNet.instance.m_serverSyncedPlayerData.TryGetValue(key, out var data) ? data : "";
    return "";
  }
}
