using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public class PickableUtils
  {
    private static String GetRespawnTime(Pickable instance, ZNetView nview, bool picked)
    {
      if (!instance.m_hideWhenPicked) return "Never";
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
        "<color=yellow><b>" + instance.m_itemPrefab.name + "</b></color>",
        "Amount: <color=yellow><b>" + instance.m_amount + "</b></color>",
        "Respawn: <color=yellow>" + respawn + "</color>"
      };
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
      if (!Settings.showPickables)
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
      if (!Settings.showPickables)
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
      if (!Settings.showPickables)
        return;
      var text = PickableUtils.GetText(__instance, ___m_nview, ___m_picked);
      __instance.GetComponentInChildren<HoverText>().m_text = text;
    }
  }
}