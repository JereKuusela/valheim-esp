using System;
using System.Collections.Generic;
using System.Linq;
using Service;
using UnityEngine;

namespace ESP {
  public partial class Texts {
    private static string GetTargetName(ItemDrop.ItemData.AiTarget target) {
      if (target == ItemDrop.ItemData.AiTarget.Enemy) return "";
      if (target == ItemDrop.ItemData.AiTarget.Friend) return "Support";
      if (target == ItemDrop.ItemData.AiTarget.FriendHurt) return "Heal";
      return "";
    }
    private static string GetDamages(HitData.DamageTypes target, int tier) {
      var tooltip = target.GetTooltipString();
      tooltip = tooltip.Replace("#CHOP_TIER", Texts.GetChopTier(tier));
      tooltip = tooltip.Replace("#PICKAXE_TIER", Texts.GetPickaxeTier(tier));
      var text = Localization.instance.Localize(tooltip.Replace("\n", ", "));
      if (text.Length > 0) return text.Substring(2);
      return text;
    }
    public static string GetAttack(Humanoid obj) {
      if (!Settings.Creatures || !Settings.Attacks || !Helper.IsValid(obj)) return "";
      var weapons = obj.GetInventory().GetAllItems().Where(item => item.IsWeapon());
      var time = Time.time;
      // Some attacks have multiple instances so group them to reduce clutter.
      var groups = weapons.GroupBy(weapon => GetDamages(weapon.GetDamage(), weapon.m_shared.m_toolTier) + weapon.m_shared.m_aiAttackInterval + weapon.m_shared.m_aiAttackRange);
      var texts = groups.Select(group => {
        var weapon = group.First();
        var data = weapon.m_shared;
        var attack = data.m_attack;
        var isNonAttack = attack.m_attackType == Attack.AttackType.None;
        var text = Translate.Name(weapon, "orange");
        var target = GetTargetName(data.m_aiTargetType);
        if (target != "")
          text += " (" + target + ")";
        var timers = group.Select(item => {
          var timer = Mathf.Min(time - item.m_lastAttackTime, data.m_aiAttackInterval);
          return Format.Progress(timer, data.m_aiAttackInterval) + " s";
        });
        text += ": " + Format.JoinRow(timers);
        var damage = weapon.GetDamage();
        var damages = GetDamages(damage, weapon.m_shared.m_toolTier);
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
        if (!isNonAttack && (!data.m_blockable || !data.m_dodgeable)) {
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
      return "\n" + Format.JoinLines(texts);
    }

    public static string GetNoise(Character obj) => "Noise: " + Format.Int(obj.m_noiseRange);

    public static string GetStaggerText(float health, float staggerDamageFactor, float staggerDamage) {
      var staggerLimit = staggerDamageFactor * health;
      if (staggerLimit > 0)
        return "Stagger: " + Format.Progress(staggerDamage, staggerLimit);
      else
        return "Stagger: " + Format.String("Immune");
    }
    private static string GetState(Character character, BaseAI baseAI, MonsterAI monsterAI) {
      var state = new List<string>();
      if (monsterAI && monsterAI.IsAlerted())
        state.Add(Format.String("Alerted", "red"));
      if (monsterAI && monsterAI.HuntPlayer())
        state.Add(Format.String("Hunt mode", "red"));
      if (character.IsStaggering())
        state.Add(Format.String("Staggering", "red"));
      if (baseAI && baseAI.IsSleeping())
        state.Add(Format.String("Sleeping", "yellow"));
      var stateText = Format.JoinRow(state);
      if (stateText != "")
        return ", " + stateText;
      return "";
    }
    public static string Get(Character obj, BaseAI baseAI, MonsterAI monsterAI) {
      if (!Settings.Resistances || !Helper.IsValid(obj) || !baseAI || !monsterAI)
        return "";
      var lines = new List<string>();
      var mass = obj.m_body.mass;
      lines.Add(GetState(obj, baseAI, monsterAI));
      lines.Add(monsterAI.m_aiStatus);
      var health = obj.GetMaxHealth();
      lines.Add(Text.GetHealth(obj.GetHealth(), health));
      var factor = obj.m_staggerDamageFactor;
      // Doesn't have stagger animation so hardcoded to be immune.
      if ((obj.m_name == "Deathsquito"))
        factor = 0f;
      lines.Add(GetStaggerText(health, factor, obj.m_staggerDamage));
      lines.Add("Mass: " + Format.Int(mass) + " (" + Format.Percent(1f - 5f / mass) + " knockback resistance)");
      var damageModifiers = obj.GetDamageModifiers();
      lines.Add(DamageModifierUtils.Get(damageModifiers, true, true));
      if (monsterAI && monsterAI.IsSleeping()) {
        var wakeUp = new List<string>();
        if (monsterAI.m_wakeupRange > 0)
          wakeUp.Add("Wake up range: " + Format.Float(monsterAI.m_wakeupRange) + " m");
        if (monsterAI.m_noiseWakeup)
          wakeUp.Add("Wake up noise: " + Format.Float(monsterAI.m_noiseRangeScale) + "x");
        lines.Add(Format.JoinRow(wakeUp));
      }

      if (baseAI) {
        Vector3 patrolPoint;
        var patrol = baseAI.GetPatrolPoint(out patrolPoint);
        if (patrol)
          lines.Add("Patrol: " + Format.String(patrolPoint.ToString("F0")));
      }
      if (monsterAI.m_consumeItems.Count > 0) {
        var items = Translate.Name(monsterAI.m_consumeItems);
        lines.Add(items);
      }
      return Format.JoinLines(lines);
    }
    private static string Get(StatusEffect statusEffect)
      => Localization.instance.Localize(statusEffect.m_name) + ": " + Format.Progress(statusEffect.GetRemaningTime(), statusEffect.m_ttl) + " seconds";
    public static string GetStatusStats(Character character) {
      if (!Settings.Status || !character)
        return "";
      var lines = character.GetSEMan().GetStatusEffects().Select(Get);
      return Format.JoinLines(lines);
    }
    public static string Get(BaseAI baseAI, Growup growup) {
      if (!Settings.Breeding || !baseAI || !growup)
        return "";
      var value = baseAI.GetTimeSinceSpawned().TotalSeconds;
      var limit = growup.m_growTime;
      return Format.ProgressPercent("Progress", value, limit);
    }

    private static List<string> GetDropTexts(CharacterDrop characterDrop, Character character) {
      var list = new List<string>();
      int num = character ? Mathf.Max(1, (int)Mathf.Pow(2f, (float)(character.GetLevel() - 1))) : 1;
      foreach (CharacterDrop.Drop drop in characterDrop.m_drops) {
        if (!(drop.m_prefab == null)) {
          float chance = drop.m_chance;
          if (drop.m_levelMultiplier) {
            chance *= (float)num;
          }
          int min = drop.m_amountMin;
          int max = Math.Max(min, drop.m_amountMax - 1);  // -1 because exclusive on the random range.
          if (drop.m_levelMultiplier) {
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
    public static string Get(CharacterDrop characterDrop, Character character) {
      if (!Settings.Drops || !characterDrop || !character)
        return "";
      var dropTexts = Format.JoinRow(GetDropTexts(characterDrop, character));
      return "Drops: " + dropTexts;
    }
  }
}

