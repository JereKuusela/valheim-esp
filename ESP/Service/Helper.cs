using System;
using HarmonyLib;
using UnityEngine;
namespace Service;
public class Helper
{
  private static T Get<T>(object obj, string field) => Traverse.Create(obj).Field<T>(field).Value;
  public static ZNetView Nview(MonoBehaviour obj) => Get<ZNetView>(obj, "m_nview");
  public static double GetElapsed(MonoBehaviour obj, string key, long defaultValue = 0)
  {
    var time = ZNet.instance.GetTime();
    var d = GetDateTime(obj, key, defaultValue);
    return (time - d).TotalSeconds;
  }
  public static double GetElapsed(MonoBehaviour obj, int key, long defaultValue = 0)
  {
    var time = ZNet.instance.GetTime();
    var d = GetDateTime(obj, key, defaultValue);
    return (time - d).TotalSeconds;
  }
  public static DateTime GetDateTime(MonoBehaviour obj, string key, long defaultValue = 0) => new(GetLong(obj, key, defaultValue));
  public static DateTime GetDateTime(MonoBehaviour obj, int key, long defaultValue = 0) => new(GetLong(obj, key, defaultValue));
  public static float GetFloat(MonoBehaviour obj, string key, float defaultValue = 0) => Nview(obj).GetZDO().GetFloat(key, defaultValue);
  public static long GetLong(MonoBehaviour obj, string key, long defaultValue = 0) => Nview(obj).GetZDO().GetLong(key, defaultValue);
  public static long GetLong(MonoBehaviour obj, int key, long defaultValue = 0) => Nview(obj).GetZDO().GetLong(key, defaultValue);
  public static int GetInt(MonoBehaviour obj, string key, int defaultValue = 0) => Nview(obj).GetZDO().GetInt(key, defaultValue);
  public static bool GetBool(MonoBehaviour obj, string key, bool defaultValue = false) => Nview(obj).GetZDO().GetBool(key, defaultValue);
  public static string GetString(MonoBehaviour obj, string key, string defaultValue = "") => Nview(obj).GetZDO().GetString(key, defaultValue);
  public static GameObject GetPrefab(MonoBehaviour obj) => ZNetScene.instance.GetPrefab(Nview(obj).GetZDO().GetPrefab());
  public static bool IsValid(MonoBehaviour obj)
  {
    if (!obj) return false;
    var nView = Nview(obj);
    if (!nView) return false;
    return nView.IsValid();
  }
}
