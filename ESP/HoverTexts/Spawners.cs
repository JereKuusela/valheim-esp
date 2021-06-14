using System;
using System.Collections.Generic;

namespace ESP
{
  public partial class HoverTextUtils
  {
    private static string GetTimeText(CreatureSpawner obj)
    {
      if (!obj.m_spawnAtDay)
        return "Only during " + TextUtils.String("night");
      if (!obj.m_spawnAtNight)
        return "Only during " + TextUtils.String("day");
      return "";
    }
    private static String GetRespawnTime(CreatureSpawner obj)
    {
      if (obj.m_respawnTimeMinuts == 0) return "Never";
      var elapsed = Patch.GetElapsed(obj, "alive_time");
      var elapsedString = elapsed == 0 ? "Alive" : elapsed.ToString("N0");
      return elapsedString + " / " + (60 * obj.m_respawnTimeMinuts).ToString("N0") + " seconds";
    }
    private static string GetRespawnTime(Pickable obj)
    {
      if (!obj.m_hideWhenPicked || obj.m_respawnTimeMinutes == 0) return "Never";
      var nView = Patch.m_nview(obj);
      var elapsed = Patch.GetElapsed(obj, "picked_time");
      var picked = Patch.GetBool(obj, "picked");
      var elapsedText = picked ? elapsed.ToString("N0") : "Not picked";
      return elapsedText + " / " + obj.m_respawnTimeMinutes.ToString("N0") + " minutes";
    }
    public static string GetText(Pickable obj)
    {
      if (!obj || !Settings.showStructureStats) return "";
      var respawn = GetRespawnTime(obj);
      var text = "\nRespawn: " + TextUtils.String(respawn);
      if (obj.m_amount > 0)
        text += "\nAmount: " + TextUtils.String(obj.m_amount.ToString());
      return text;
    }

    public static string GetText(CreatureSpawner obj)
    {
      if (!obj) return "";
      var respawn = GetRespawnTime(obj);
      var noise = obj.m_triggerNoise > 0 ? " with noise of " + TextUtils.Int(obj.m_triggerNoise) : "";
      var lines = new List<string>();
      lines.Add(TextUtils.Name(obj.m_creaturePrefab));
      lines.Add("Respawn: " + TextUtils.String(respawn));
      lines.Add(TextUtils.GetLevel(obj.m_minLevel, obj.m_maxLevel, obj.m_levelupChance));
      var timeText = GetTimeText(obj);
      if (timeText.Length > 0) lines.Add(timeText);
      if (obj.m_setPatrolSpawnPoint) lines.Add("Patrol point");
      lines.Add("Activates within " + TextUtils.Int(obj.m_triggerDistance) + " meters" + noise);
      return string.Join("\n", lines);
    }
  }
}

