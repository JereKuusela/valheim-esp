using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{

  [HarmonyPatch(typeof(Pickable), "Awake")]
  public class Pickable_Awake
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
      var text = Format.Name(__instance.m_itemPrefab);
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, color, Settings.pickableRayWidth);
      Drawer.AddText(obj, text);
      Drawer.AddBoxCollider(obj);
    }
  }

}