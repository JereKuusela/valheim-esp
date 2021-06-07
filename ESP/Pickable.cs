using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{

  [HarmonyPatch(typeof(Pickable), "Awake")]
  public class Pickabler_Awake
  {
    private static bool IsEnabled(Pickable instance)
    {
      if (!Settings.showPickables) return false;
      var name = instance.m_itemPrefab.name.ToLower();
      var excluded = Settings.excludedPickables.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return false;
      return true;
    }
    private static Color GetColor(Pickable instance)
    {
      return instance.m_hideWhenPicked && instance.m_respawnTimeMinutes > 0 ? Color.green : Color.blue;
    }
    public static void Postfix(Pickable __instance, ZNetView ___m_nview)
    {
      if (!IsEnabled(__instance))
        return;
      var color = GetColor(__instance);
      Action<GameObject> action = (GameObject obj) =>
        {
          var text = obj.AddComponent<PickableText>();
          text.pickable = __instance;
        };
      Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, color, Settings.pickableRayWidth, action);
    }
  }

  [HarmonyPatch(typeof(Pickable), "GetHoverText")]
  public class Pickable_GetHoverText
  {
    public static void Postfix(Pickable __instance, ref string __result)
    {
      __result += HoverTextUtils.GetText(__instance);
    }
  }
  public class PickableText : MonoBehaviour, Hoverable
  {
    public string GetHoverText() => GetHoverName() + HoverTextUtils.GetText(pickable);
    public string GetHoverName() => TextUtils.Name(pickable.m_itemPrefab);
    public Pickable pickable;
  }
}