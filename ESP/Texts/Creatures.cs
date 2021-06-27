using System.Linq;
using System;
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
    private static string GetDamages(HitData.DamageTypes target, int tier = 0)
    {
      var tooltip = target.GetTooltipString();
      tooltip = tooltip.Replace("#CHOP_TIER", Texts.GetChopTier(tier));
      tooltip = tooltip.Replace("#PICKAXE_TIER", Texts.GetPickaxeTier(tier));
      var text = Localization.instance.Localize(tooltip.Replace("\n", ", "));
      if (text.Length > 0) return text.Substring(2);
      return text;
    }
    public static string GetAttack(Humanoid obj)
    {
      if (!Settings.creatures || !Settings.attacks || !obj) return "";
      var weapons = obj.GetInventory().GetAllItems().Where(item => item.IsWeapon());
      var time = Time.time;
      // Some attacks have multiple instances so group them to reduce clutter.
      var groups = weapons.GroupBy(weapon => GetDamages(weapon.GetDamage()) + weapon.m_shared.m_aiAttackInterval + weapon.m_shared.m_aiAttackRange);
      var texts = groups.Select(group =>
      {
        var weapon = group.First();
        var data = weapon.m_shared;
        var attack = data.m_attack;
        var isNonAttack = attack.m_attackType == Attack.AttackType.None;
        var text = Format.Name(weapon, "orange");
        var target = GetTargetName(data.m_aiTargetType);
        if (target != "")
          text += " (" + target + ")";
        var timers = group.Select(item =>
        {
          var timer = Mathf.Min(time - item.m_lastAttackTime, data.m_aiAttackInterval);
          return Format.Progress(timer, data.m_aiAttackInterval) + " s";
        });
        text += ": " + string.Join(", ", timers);
        var damage = weapon.GetDamage();
        var damages = GetDamages(damage);
        if (!isNonAttack && damages != "")
          text += "\n" + damages;
        text += "\nRange: " + Format.Range(data.m_aiAttackRangeMin, data.m_aiAttackRange) + " meters (" + Format.Int(data.m_aiAttackMaxAngle) + " degrees)";
        if (data.m_aiPrioritized)
          text += ", " + Format.String("priority");

        if (!isNonAttack)
          text += ", " + Texts.GetAttackType(attack);
        var hitbox = Texts.GetHitboxText(attack);
        if (!isNonAttack && hitbox != "")
          text += ", " + hitbox;
        var projectile = Texts.GetProjectileText(attack);
        if (!isNonAttack && projectile != "")
          text += ", " + projectile;
        if (!attack.m_lowerDamagePerHit)
          text += ", " + Format.String("No multitarget penalty");
        if (!isNonAttack && (!data.m_blockable || !data.m_dodgeable))
        {
          text += "\n";
          if (!data.m_blockable)
            text += "Can't be blocked";
          if (!data.m_blockable && !data.m_dodgeable)
            text += ", ";
          if (!data.m_dodgeable)
            text += "Can't be dodged";
        }
        return text;
      });
      return "\n\n" + string.Join("\n", texts);
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
      if (!Settings.resistances || !obj || !baseAI || !monsterAI)
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
      stats += DamageModifierUtils.Get(damageModifiers, true, true);
      if (baseAI)
      {
        Vector3 patrolPoint;
        var patrol = baseAI.GetPatrolPoint(out patrolPoint);
        if (patrol)
          stats += "\nPatrol: " + Format.String(patrolPoint.ToString("F0"));
      }
      if (monsterAI.m_consumeItems.Count > 0)
      {
        var heal = " (" + Format.Int(monsterAI.m_consumeHeal) + " health)";
        var items = Format.Name(monsterAI.m_consumeItems);
        stats += "\n" + items + heal;
      }
      return stats;
    }

    public static string GetStatusStats(Character character)
    {
      if (!Settings.status || !character)
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
      if (!Settings.breeding || !baseAI || !growup)
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
          int max = Math.Max(min, drop.m_amountMax - 1);  // -1 because exclusive on the random range.
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
      if (!Settings.drops || !characterDrop || !character)
        return "";
      var dropTexts = string.Join(", ", GetDropTexts(characterDrop, character));
      return "\nDrops: " + dropTexts;
    }
  }
}

