using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace ESP
{
  public partial class Texts
  {
    private static string GetTargetName(ItemDrop.ItemData.AiTarget target)
    {
      if (target == ItemDrop.ItemData.AiTarget.Enemy) return "";
      if (target == ItemDrop.ItemData.AiTarget.Friend) return "Support";
      if (target == ItemDrop.ItemData.AiTarget.FriendHurt) return "Heal";
      return "";
    }
    private static string GetDamages(HitData.DamageTypes target) => Localization.instance.Localize(target.GetTooltipString().Replace("\n", ", ")).Substring(2);
    public static string GetAttack(Humanoid obj)
    {
      if (!obj) return "";
      var weapons = obj.GetInventory().GetAllItems().Where(item => item.IsWeapon());
      var time = Time.time;
      var texts = weapons.Select(weapon =>
      {
        var data = weapon.m_shared;
        var header = Format.Name(weapon);
        var target = GetTargetName(data.m_aiTargetType);
        if (target != "")
          header += " (" + target + ")";
        var timer = Mathf.Min(time - weapon.m_lastAttackTime, data.m_aiAttackInterval);
        var text = Format.ProgressPercent(header, timer, data.m_aiAttackInterval);
        text += "\nRange: " + Format.Range(data.m_aiAttackRangeMin, data.m_aiAttackRange) + " meters (" + Format.Int(data.m_aiAttackMaxAngle) + " degrees)";
        if (data.m_aiPrioritized)
          text += ", " + Format.String("priority");
        text += "\n" + GetDamages(weapon.GetDamage());
        if (!data.m_blockable || !data.m_dodgeable)
        {
          text += "\n";
          if (!data.m_blockable)
            text += "Can't be blocked";
          if (!data.m_blockable && !data.m_dodgeable)
            text += ", ";
          if (!data.m_dodgeable)
            text += "Can't be dodged";
        }
        var hitbox = Texts.GetHitboxText(data.m_attack);
        if (hitbox != "")
          text += "\n" + hitbox;
        var projectile = Texts.GetProjectileText(data.m_attack);
        if (projectile != "")
          text += "\n" + projectile;
        return text;
      });
      return "\n" + string.Join("\n", texts);
    }

    public static string GetNoise(Character obj) => "Noise: " + Format.Int(Patch.m_noiseRange(obj));

    public static string GetStaggerText(float health, float staggerDamageFactor, float staggerDamage)
    {
      var staggerLimit = staggerDamageFactor * health;
      if (staggerLimit > 0)
        return "\n" + "Stagger: " + Format.Progress(staggerDamage, staggerLimit);
      else
        return "\n" + "Stagger: " + Format.String("Immune");
    }
    public static string Get(Character obj, BaseAI baseAI, MonsterAI monsterAI)
    {
      if (!Settings.showCreatureStats || !obj || !baseAI || !monsterAI)
        return "";
      var staggerDamage = Patch.m_staggerDamage(obj);
      var body = Patch.m_body(obj);
      var stats = "";
      if (monsterAI && (monsterAI.IsAlerted() || monsterAI.HuntPlayer()))
      {
        var mode = "";
        if (monsterAI.IsAlerted())
          mode += "Alerted";
        if (monsterAI.IsAlerted() && monsterAI.HuntPlayer())
          mode += ", ";
        if (monsterAI.HuntPlayer())
          mode += "Hunt mode";
        stats += "\n<color=red>" + mode + "</color>";
      }
      var health = obj.GetMaxHealth();
      stats += "\n" + Format.GetHealth(obj.GetHealth(), health);
      stats += GetStaggerText(health, obj.m_staggerDamageFactor, staggerDamage);
      stats += "\n" + "Mass: " + Format.Int(body.mass) + " (" + Format.Percent(1f - 5f / body.mass) + " knockback resistance)";
      var damageModifiers = Patch.Character_GetDamageModifiers(obj);
      stats += DamageModifierUtils.Get(damageModifiers);
      if (baseAI)
      {
        Vector3 patrolPoint;
        var patrol = baseAI.GetPatrolPoint(out patrolPoint);
        var patrolText = patrol ? patrolPoint.ToString("F0") : "No patrol";
        stats += "\nPatrol: " + Format.String(patrolText);
      }
      if (monsterAI.m_consumeItems.Count > 0)
      {
        var heal = " (" + Format.Int(monsterAI.m_consumeHeal) + " health)";
        var items = monsterAI.m_consumeItems.Select(item => Format.Name(item.gameObject));
        stats += "\n" + string.Join(", ", items) + heal;
      }
      return stats;
    }

    public static string GetStatusStats(Character character)
    {
      if (!Settings.showStatusEffects || !character)
        return "";
      var statusEffects = character.GetSEMan().GetStatusEffects();
      var text = "";
      foreach (var status in statusEffects)
      {
        text += "\n" + Localization.instance.Localize(status.m_name) + ": " + Format.Progress(status.GetRemaningTime(), status.m_ttl) + " seconds";
      }
      return text;
    }
    public static string Get(BaseAI baseAI, Growup growup)
    {
      if (!Settings.showBreedingStats || !baseAI || !growup)
        return "";
      var value = baseAI.GetTimeSinceSpawned().TotalSeconds;
      var limit = growup.m_growTime;
      return "\n" + Format.ProgressPercent("Progress", value, limit);
    }

    private static List<string> GetDropTexts(CharacterDrop characterDrop, Character character)
    {
      var list = new List<string>();
      int num = character ? Mathf.Max(1, (int)Mathf.Pow(2f, (float)(character.GetLevel() - 1))) : 1;
      foreach (CharacterDrop.Drop drop in characterDrop.m_drops)
      {
        if (!(drop.m_prefab == null))
        {
          float chance = drop.m_chance;
          if (drop.m_levelMultiplier)
          {
            chance *= (float)num;
          }
          int min = drop.m_amountMin;
          int max = drop.m_amountMax - 1;  // -1 because exclusive on the random range.
          if (drop.m_levelMultiplier)
          {
            min *= num;
            max *= num;
          }
          var text = "";
          if (max > 1 || (max == 1 && chance >= 1.0)) text += Format.Range(min, max) + " ";
          text += drop.m_prefab.name;
          if (drop.m_onePerPlayer) text += " (per player)";
          if (chance < 1.0) text += " (" + Format.Percent(chance) + ")";
          list.Add(text);
        }
      }
      return list;
    }
    public static string Get(CharacterDrop characterDrop, Character character)
    {
      if (!Settings.showDropStats || !characterDrop || !character)
        return "";
      var dropTexts = string.Join(", ", GetDropTexts(characterDrop, character));
      return "\nDrops: " + dropTexts;
    }
  }
}

