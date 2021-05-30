using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public class CreatureSpawnerUtils
  {
    public static bool IsEnabled(CreatureSpawner instance)
    {
      if (!Settings.showCreatureSpawners) return false;
      var name = instance.name.ToLower();
      var excluded = Settings.excludedCreatureSpawners.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return false;
      return true;
    }
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
      var level = TextUtils.Range(instance.m_minLevel, instance.m_maxLevel);
      var lines = new string[]{
        TextUtils.String(Localization.instance.Localize(instance.m_creaturePrefab.name)),
        "Respawn: " + TextUtils.String(respawn),
        "Level: " + level + " (" + TextUtils.Percent(instance.m_levelupChance / 100f) + " per level)"
      };
      if (!instance.m_spawnAtDay)
      {
        lines.AddItem("Only during " + TextUtils.String("night"));
      }
      if (!instance.m_spawnAtNight)
      {
        lines.AddItem("Only during " + TextUtils.String("day"));
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
      if (!CreatureSpawnerUtils.IsEnabled(__instance))
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
      if (!CreatureSpawnerUtils.IsEnabled(__instance))
        return;
      var text = CreatureSpawnerUtils.GetText(__instance, ___m_nview);
      __instance.GetComponentInChildren<HoverText>().m_text = text;
    }
  }
}