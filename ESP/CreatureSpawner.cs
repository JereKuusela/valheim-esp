using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public class CreatureSpawnerUtils
  {
    private static String GetRespawnTime(CreatureSpawner instance, ZNetView nview)
    {
      if (instance.m_respawnTimeMinuts == 0) return "Never";
      DateTime time = ZNet.instance.GetTime();
      DateTime d = new DateTime(nview.GetZDO().GetLong("alive_time", 0L));
      var timer = (time - d).TotalSeconds;
      var timerString = timer == 0 ? "Alive" : timer.ToString("N0");
      return timerString + " / " + (60 * instance.m_respawnTimeMinuts).ToString("N0") + " seconds";
    }
    public static String GetText(CreatureSpawner instance, ZNetView nview)
    {
      var respawn = GetRespawnTime(instance, nview);
      var level = instance.m_maxLevel > instance.m_minLevel ? instance.m_minLevel + "-" + instance.m_maxLevel : instance.m_maxLevel.ToString();
      var lines = new string[]{
        TextUtils.StringValue(instance.m_creaturePrefab.name),
        "Respawn: " + TextUtils.StringValue(respawn),
        "Level: " + TextUtils.StringValue(level) + " (" + TextUtils.PercentValue(instance.m_levelupChance / 100f) + " per level)"
      };
      if (!instance.m_spawnAtDay)
      {
        lines.AddItem("Only during " + TextUtils.StringValue("night"));
      }
      if (!instance.m_spawnAtNight)
      {
        lines.AddItem("Only during " + TextUtils.StringValue("day"));
      }
      return lines.Join(null, "\n");
    }
    public static Color GetColor(CreatureSpawner __instance)
    {
      return __instance.m_respawnTimeMinuts > 0f ? Color.yellow : Color.red;
    }
  }

  [HarmonyPatch(typeof(CreatureSpawner), "Awake")]
  public class CreatureSpawner_Awake
  {
    public static void Postfix(CreatureSpawner __instance, ZNetView ___m_nview)
    {
      if (!Settings.showCreatureSpawners)
        return;
      var color = CreatureSpawnerUtils.GetColor(__instance);
      var text = CreatureSpawnerUtils.GetText(__instance, ___m_nview);
      Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, color, 0.5f, text);
    }
  }

  [HarmonyPatch(typeof(CreatureSpawner), "UpdateSpawner")]
  public class CreatureSpawner_UpdateSpawner
  {
    public static void Postfix(CreatureSpawner __instance, ZNetView ___m_nview)
    {
      if (!Settings.showCreatureSpawners)
        return;
      var text = CreatureSpawnerUtils.GetText(__instance, ___m_nview);
      __instance.GetComponentInChildren<HoverText>().m_text = text;
    }
  }
}