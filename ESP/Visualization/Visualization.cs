using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ESP
{
  [HarmonyPatch(typeof(Location), "Awake")]
  public class Location_Awake
  {
    public static void Postfix(Location __instance)
    {
      if (!Settings.showLocations)
        return;
      var text = TextUtils.Name(__instance.gameObject);
      Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, Color.black, Settings.locationRayWidth, text);
    }
  }
  [HarmonyPatch(typeof(Container), "Awake")]
  public class Container_Awake
  {
    public static void Postfix(Container __instance, Piece ___m_piece)
    {
      if (!Settings.showChests || !___m_piece || ___m_piece.IsPlacedByPlayer())
        return;
      var text = TextUtils.String(__instance.GetHoverName());
      Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, Color.white, Settings.chestRayWidth, text);
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
      var text = TextUtils.Name(obj.m_creaturePrefab);
      Drawer.DrawMarkerLine(obj.gameObject, Vector3.zero, color, 0.5f, text);
    }
  }
  [HarmonyPatch(typeof(EffectArea), "Awake")]
  public class EffectArea_Awake
  {
    private static Color GetEffectColor(EffectArea.Type type)
    {
      if ((type & EffectArea.Type.Burning) != 0) return Color.yellow;
      if ((type & EffectArea.Type.Heat) != 0) return Color.magenta;
      if ((type & EffectArea.Type.Fire) != 0) return Color.red;
      if ((type & EffectArea.Type.NoMonsters) != 0) return Color.green;
      if ((type & EffectArea.Type.Teleport) != 0) return Color.blue;
      if ((type & EffectArea.Type.PlayerBase) != 0) return Color.white;
      return Color.black;
    }
    private static String GetTypeText(EffectArea.Type type)
    {
      var types = new List<string>();
      if ((type & EffectArea.Type.Burning) != 0) types.Add("Burning");
      if ((type & EffectArea.Type.Heat) != 0) types.Add("Heat");
      if ((type & EffectArea.Type.Fire) != 0) types.Add("Fire");
      if ((type & EffectArea.Type.NoMonsters) != 0) types.Add("No monsters");
      if ((type & EffectArea.Type.Teleport) != 0) types.Add("Teleport");
      if ((type & EffectArea.Type.PlayerBase) != 0) types.Add("Base");
      return types.Join(null, ", ");
    }
    private static String GetRadiusText(float radius)
    {
      return "Radius: " + TextUtils.Float(radius);
    }
    public static void Postfix(EffectArea __instance)
    {
      if (!Settings.showEffectAreas)
        return;
      var color = GetEffectColor(__instance.m_type);
      var radius = Math.Max(0.5f, __instance.GetRadius());
      var text = GetTypeText(__instance.m_type) + "\n" + GetRadiusText(__instance.GetRadius());
      Action<GameObject> action = (GameObject obj) =>
        {
          var effectText = obj.AddComponent<EffectAreaText>();
          effectText.hoverText = text;
          effectText.hoverName = GetTypeText(__instance.m_type);
        };
      Drawer.DrawSphere(__instance.gameObject, Vector3.zero, radius, color, 0.1f, action);
    }
  }

  // Custom hover text to prevent showing base object information.
  public class EffectAreaText : MonoBehaviour, Hoverable
  {

    public string GetHoverText() => hoverText;
    public string GetHoverName() => hoverName;
    public string hoverName;
    public string hoverText;
  }
}