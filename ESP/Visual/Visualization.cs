using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(Location), "Awake")]
  public class Location_Awake
  {
    public static void Postfix(Location __instance)
    {
      if (!Settings.showLocations)
        return;
      var text = Format.Name(__instance.gameObject);
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, Color.black, Settings.locationRayWidth, Drawer.OTHER);
      Drawer.AddText(obj, text);
    }
  }
  [HarmonyPatch(typeof(Container), "Awake")]
  public class Container_Awake
  {
    public static void Postfix(Container __instance, Piece ___m_piece)
    {
      if (!Settings.showChests || !___m_piece || ___m_piece.IsPlacedByPlayer())
        return;
      var text = Format.String(__instance.GetHoverName());
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, Color.white, Settings.chestRayWidth, Drawer.OTHER);
      Drawer.AddText(obj, text);
    }
  }
  [HarmonyPatch(typeof(CreatureSpawner), "Awake")]
  public class CreatureSpawner_Awake
  {
    private static bool IsEnabled(CreatureSpawner obj)
    {
      if (!Settings.showCreatureSpawners) return false;
      var name = obj.name.ToLower();
      var excluded = Settings.excludedCreatureSpawners.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return false;
      return true;
    }
    private static Color GetColor(CreatureSpawner obj)
    {
      return obj.m_respawnTimeMinuts > 0f ? Color.yellow : Color.red;
    }
    public static void Postfix(CreatureSpawner __instance)
    {
      var obj = __instance;
      if (!IsEnabled(obj))
        return;
      var color = GetColor(obj);
      var text = Format.Name(obj.m_creaturePrefab);
      var line = Drawer.DrawMarkerLine(obj.gameObject, Vector3.zero, color, 0.5f, Drawer.OTHER);
      Drawer.AddText(line, text);
    }
  }
  [HarmonyPatch(typeof(EffectArea), "Awake")]
  public class EffectArea_Awake
  {
    public static void Postfix(EffectArea __instance)
    {
      if (!Settings.showEffectAreas)
        return;
      var color = EffectAreaUtils.GetEffectColor(__instance.m_type);
      var radius = Math.Max(0.5f, __instance.GetRadius());

      var obj = Drawer.DrawSphere(__instance.gameObject, radius, color, 0.1f, Drawer.OTHER);
      Drawer.AddText(obj, EffectAreaUtils.GetTypeText(__instance.m_type), Format.Radius(__instance.GetRadius()));
    }
  }
  [HarmonyPatch(typeof(PrivateArea), "Awake")]
  public class PrivateArea_Awake
  {
    public static void Postfix(PrivateArea __instance)
    {
      if (!Settings.showEffectAreas)
        return;

      var obj = Drawer.DrawSphere(__instance.gameObject, __instance.m_radius, Color.gray, 0.1f, Drawer.OTHER);
      Drawer.AddText(obj, "Protection", Format.Radius(__instance.m_radius));
    }
  }
}