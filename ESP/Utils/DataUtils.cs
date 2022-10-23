using System;
using UnityEngine;

namespace ESP;
public class DataUtils {

  public static GameObject? GetPrefab(string hashStr) {
    if (int.TryParse(hashStr, out var hash)) return GetPrefab(hash);
    return null;
  }
  public static GameObject? GetPrefab(int hash) {
    if (hash == 0) return null;
    var prefab = ZNetScene.instance.GetPrefab(hash);
    if (!prefab) return null;
    return prefab;
  }

  public static void Float(ZNetView view, int hash, Action<float> action) {
    if (view == null || !view.IsValid()) return;
    var value = view.GetZDO().GetFloat(hash, -1f);
    if (value < 0f) return;
    action(value);
  }
  public static void Long(ZNetView view, int hash, Action<long> action) {
    if (view == null || !view.IsValid()) return;
    var value = view.GetZDO().GetLong(hash, -1L);
    if (value < 0L) return;
    action(value);
  }
  public static void Int(ZNetView view, int hash, Action<int> action) {
    if (view == null || !view.IsValid()) return;
    var value = view.GetZDO().GetInt(hash, -1);
    if (value < 0) return;
    action(value);
  }
  public static void Bool(ZNetView view, int hash, Action<bool> action) {
    if (view == null || !view.IsValid()) return;
    var value = view.GetZDO().GetBool(hash, false);
    action(value);
  }
  public static void String(ZNetView view, int hash, Action<string> action) {
    if (view == null || !view.IsValid()) return;
    var value = view.GetZDO().GetString(hash, "");
    if (value == "") return;
    action(value);
  }
  public static void Prefab(ZNetView view, int hash, Action<GameObject> action) {
    if (view == null || !view.IsValid()) return;
    var value = view.GetZDO().GetInt(hash, 0);
    var prefab = DataUtils.GetPrefab(value);
    if (prefab == null) return;
    action(prefab);
  }
}
