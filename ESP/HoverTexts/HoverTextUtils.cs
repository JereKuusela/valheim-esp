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
      var text = "";
      var maxHealth = obj.m_health;
      var health = Patch.GetFloat(obj, "health", maxHealth);

      text += "\n" + TextUtils.GetHealth(health, maxHealth);
      text += "\nHit noise: " + TextUtils.Int(obj.m_hitNoise);
      text += DamageModifierUtils.GetText(obj.m_damages, false);
      return text;
    }
    public static string GetText(TreeBase obj)
    {
      if (!obj || !Settings.showStructureHealth) return "";
      var text = "";
      var maxHealth = obj.m_health;
      var health = Patch.GetFloat(obj, "health", maxHealth);

      text += "\n" + TextUtils.GetHealth(health, maxHealth);
      text += "\nHit noise: " + TextUtils.Int(100);
      text += DamageModifierUtils.GetText(obj.m_damageModifiers, false);
      return text;
    }
    public static string GetText(Destructible obj)
    {
      if (!obj || !Settings.showStructureHealth) return "";
      var text = "";
      var health = Patch.GetFloat(obj, "health", obj.m_health);
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
    public static string GetText(WearNTear obj)
    {
      if (!obj || !Settings.showStructureHealth) return "";
      var text = "";
      var health = obj.GetHealthPercentage();

      text += "\n" + TextUtils.GetHealth(health * obj.m_health, obj.m_health);
      text += DamageModifierUtils.GetText(obj.m_damages);
      return text;
    }
    public static string GetText(Beehive obj)
    {
      if (!obj || !Settings.showProgress) return "";
      var limit = obj.m_secPerUnit;
      if (limit == 0) return "";
      var value = Patch.GetFloat(obj, "product");
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static string GetText(Ship obj)
    {
      if (!obj || !Settings.showShipStats) return "";
      if (!obj.IsPlayerInBoat(Player.m_localPlayer.GetZDOID())) return "";
      return ShipUtils.text;
    }
    private static string GetItem(CookingStation obj, int slot) => Patch.GetString(obj, "slot" + slot);
    private static float GetTime(CookingStation obj, int slot) => Patch.GetFloat(obj, "slot" + slot);
    private static string GetSlotText(CookingStation obj, int slot)
    {
      if (!Settings.showProgress) return "";
      var itemName = GetItem(obj, slot);
      var cookedTime = GetTime(obj, slot);
      if (itemName == "") return "";
      var item = Patch.CookingStation_GetItemConversion(obj, itemName);
      if (item == null) return "";
      var limit = item.m_cookTime;
      if (limit == 0) return "";
      var value = cookedTime;
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static string GetText(CookingStation obj)
    {
      if (!obj || !Settings.showProgress) return "";
      var text = "";
      for (var slot = 0; slot < obj.m_slots.Length; slot++)
        text += GetSlotText(obj, slot);
      return text;
    }
    public static string GetText(Fermenter obj)
    {
      if (!obj || !Settings.showProgress) return "";
      var limit = obj.m_fermentationDuration;
      if (limit == 0) return "";
      var value = Patch.Fermenter_GetFermentationTime(obj);
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static string GetText(Fireplace obj)
    {
      if (!obj || !Settings.showProgress) return "";
      var limit = obj.m_secPerFuel;
      if (limit == 0) return "";
      var value = Patch.GetFloat(obj, "fuel") * limit;
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static string GetText(MineRock obj)
    {
      if (!obj || !Settings.showStructureHealth) return "";
      var text = "";
      var maxHealth = obj.m_health;

      text += "\nHealth per area: " + TextUtils.Int(maxHealth);
      text += DamageModifierUtils.GetText(obj.m_damageModifiers, false);
      text += "\nHit noise: " + TextUtils.Int(100);
      return text;
    }
    public static string GetText(MineRock5 obj)
    {
      if (!obj || !Settings.showStructureHealth) return "";
      var text = "";
      var maxHealth = obj.m_health;

      text += "\nHealth per area: " + TextUtils.Int(maxHealth);
      text += DamageModifierUtils.GetText(obj.m_damageModifiers, false);
      text += "\nHit noise: " + TextUtils.Int(100);
      return text;
    }
    public static string GetText(Plant obj)
    {
      if (!obj || !Settings.showProgress) return "";
      var limit = Patch.Plant_GetGrowTime(obj);
      if (limit == 0) return "";
      var value = Patch.Plant_TimeSincePlanted(obj);
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static string GetText(EffectArea obj)
    {
      if (!obj || !Settings.showEffectAreas) return "";
      return "\n" + EffectAreaUtils.GetTypeText(obj.m_type) + " " + TextUtils.Radius(obj.GetRadius());
    }
    public static string GetText(PrivateArea obj)
    {
      if (!obj || !Settings.showEffectAreas) return "";
      return "\nProtection " + TextUtils.Radius(obj.m_radius);
    }
  }
}

