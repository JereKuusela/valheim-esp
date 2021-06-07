using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
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
}