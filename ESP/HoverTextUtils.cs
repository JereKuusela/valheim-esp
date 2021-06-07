using System;
using System.Collections.Generic;

namespace ESP
{
  public class HoverTextUtils
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
      var nView = Patch.m_nview(obj);
      DateTime time = ZNet.instance.GetTime();
      DateTime d = new DateTime(nView.GetZDO().GetLong("alive_time", 0L));
      var timer = (time - d).TotalSeconds;
      var timerString = timer == 0 ? "Alive" : timer.ToString("N0");
      return timerString + " / " + (60 * obj.m_respawnTimeMinuts).ToString("N0") + " seconds";
    }
    private static string GetRespawnTime(Pickable obj)
    {
      if (!obj.m_hideWhenPicked || obj.m_respawnTimeMinutes == 0) return "Never";
      var nView = Patch.m_nview(obj);
      var time = ZNet.instance.GetTime();
      var d = new DateTime(nView.GetZDO().GetLong("picked_time", 0L));
      var timer = (time - d).TotalMinutes;
      var picked = nView.GetZDO().GetBool("picked", false); ;
      var timerString = picked ? timer.ToString("N0") : "Not picked";
      return timerString + " / " + obj.m_respawnTimeMinutes.ToString("N0") + " minutes";
    }
    public static string GetText(Pickable obj)
    {
      if (!obj || !Settings.showStructureHealth) return "";
      var respawn = GetRespawnTime(obj);
      var text = "\nRespawn: " + TextUtils.String(respawn);
      if (obj.m_amount > 0)
        text += "\nAmount: " + TextUtils.String(obj.m_amount.ToString());
      return text;
    }
    public static string GetText(TreeLog obj)
    {
      if (!obj || !Settings.showStructureHealth) return "";
      var nView = Patch.m_nview(obj);
      var text = "";
      var maxHealth = obj.m_health;
      var health = nView.GetZDO().GetFloat("health", maxHealth);

      text += "\n" + TextUtils.GetHealth(health, maxHealth);
      text += "\nHit noise: " + TextUtils.Int(obj.m_hitNoise);
      text += DamageModifierUtils.GetText(obj.m_damages, false);
      return text;
    }
    public static string GetText(TreeBase obj)
    {
      if (!obj || !Settings.showStructureHealth) return "";
      var nView = Patch.m_nview(obj);
      var text = "";
      var maxHealth = obj.m_health;
      var health = nView.GetZDO().GetFloat("health", maxHealth);

      text += "\n" + TextUtils.GetHealth(health, maxHealth);
      text += "\nHit noise: " + TextUtils.Int(100);
      text += DamageModifierUtils.GetText(obj.m_damageModifiers, false);
      return text;
    }
    public static string GetText(Destructible obj)
    {
      if (!obj || !Settings.showStructureHealth) return "";
      var nView = Patch.m_nview(obj);
      var text = "";
      var health = nView.GetZDO().GetFloat("health", obj.m_health);
      var maxHealth = obj.m_health;

      text += "\n" + TextUtils.GetHealth(health, maxHealth);
      text += "\nHit noise: " + TextUtils.Int(obj.m_hitNoise);
      text += DamageModifierUtils.GetText(obj.m_damages, false);
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

