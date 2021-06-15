using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(BaseAI), "Awake")]
  public class BaseAI_Awake_Ray
  {
    private static void DrawRay(Character obj)
    {
      if (!Settings.showCreatureRays || !obj) return;
      var line = Drawer.DrawMarkerLine(obj.gameObject, Vector3.zero, Color.magenta, Settings.characterRayWidth, Drawer.CREATURE);
      Drawer.AddText(line);
    }
    public static void Postfix(Character ___m_character)
    {
      if (CharacterUtils.IsExcluded(___m_character)) return;
      DrawRay(___m_character);
    }
  }
  [HarmonyPatch(typeof(Pickable), "Awake")]
  public class Pickable_Awake_Ray
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
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, color, Settings.pickableRayWidth, Drawer.OTHER);
      Drawer.AddText(obj, text);
    }
  }
}