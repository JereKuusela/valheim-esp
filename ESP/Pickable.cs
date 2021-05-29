using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public class PickableUtils
  {
    public static bool IsEnabled(Pickable instance)
    {
      var name = instance.m_itemPrefab.name.ToLower();
      var excluded = Settings.excludedPickables.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return false;
      return Settings.showPickables;
    }
    private static String GetRespawnTime(Pickable instance, ZNetView nview, bool picked)
    {
      if (!instance.m_hideWhenPicked || instance.m_respawnTimeMinutes == 0) return "Never";
      DateTime time = ZNet.instance.GetTime();
      DateTime d = new DateTime(nview.GetZDO().GetLong("picked_time", 0L));
      var timer = (time - d).TotalMinutes;
      var timerString = picked ? timer.ToString("N0") : "Not picked";
      return timerString + " / " + instance.m_respawnTimeMinutes.ToString("N0") + " minutes";
    }
    public static String GetText(Pickable instance, ZNetView nview, bool picked)
    {
      var respawn = GetRespawnTime(instance, nview, picked);
      var lines = new string[]{
        TextUtils.StringValue(instance.m_itemPrefab.name),
        "Respawn: " + TextUtils.StringValue(respawn)
      };
      if (instance.m_amount > 0)
      {
        lines.AddItem("Amount: " + TextUtils.StringValue(instance.m_amount.ToString()));
      }
      return lines.Join(null, "\n");
    }
    public static Color GetColor(Pickable __instance)
    {
      return __instance.m_hideWhenPicked ? Color.green : Color.blue;
    }
  }

  [HarmonyPatch(typeof(Pickable), "Awake")]
  public class Pickabler_Awake
  {
    public static void Postfix(Pickable __instance, ZNetView ___m_nview, bool ___m_picked)
    {
      if (!PickableUtils.IsEnabled(__instance))
        return;
      var color = PickableUtils.GetColor(__instance);
      var text = PickableUtils.GetText(__instance, ___m_nview, ___m_picked);
      Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, color, 0.5f, text);
    }
  }

  [HarmonyPatch(typeof(Pickable), "UpdateRespawn")]
  public class Pickable_UpdateRespawn
  {
    public static void Postfix(Pickable __instance, ZNetView ___m_nview, bool ___m_picked)
    {
      if (!PickableUtils.IsEnabled(__instance))
        return;
      var text = PickableUtils.GetText(__instance, ___m_nview, ___m_picked);
      __instance.GetComponentInChildren<HoverText>().m_text = text;
    }
  }


  [HarmonyPatch(typeof(Pickable), "RPC_SetPicked")]
  public class Pickable_RPC_SetPicked
  {
    public static void Postfix(Pickable __instance, ZNetView ___m_nview, bool ___m_picked)
    {
      if (!PickableUtils.IsEnabled(__instance))
        return;
      var text = PickableUtils.GetText(__instance, ___m_nview, ___m_picked);
      __instance.GetComponentInChildren<HoverText>().m_text = text;
    }
  }
}