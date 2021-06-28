using System.Collections.Generic;

namespace ESP
{
  public partial class Texts
  {
    public static string GetChopTier(int tier)
    {
      if (tier > 4) return "Blackmetal+";
      if (tier == 4) return "Blackmetal";
      if (tier == 3) return "Iron";
      if (tier == 2) return "Bronze";
      if (tier == 1) return "Flint";
      return "Stone";
    }
    public static string GetPickaxeTier(int tier)
    {
      if (tier > 2) return "Iron+";
      if (tier == 2) return "Iron";
      if (tier == 1) return "Bronze";
      return "Antler";
    }
    public static string GetToolTier(int tier, bool chop, bool pickaxe)
    {
      var text = "";
      if (chop)
        text += "\nAxe: " + Format.String(Texts.GetChopTier(tier));
      if (pickaxe)
        text += "\nPickaxe: " + Format.String(Texts.GetPickaxeTier(tier));
      return text;
    }
    private static string GetAttackTypeName(Attack.AttackType type)
    {
      if (type == Attack.AttackType.Area) return "Horizontal";
      if (type == Attack.AttackType.Horizontal) return "Horizontal";
      if (type == Attack.AttackType.Projectile) return "Projectile";
      if (type == Attack.AttackType.TriggerProjectile) return "TriggerProjectile";
      if (type == Attack.AttackType.Vertical) return "Vertical";
      return "";
    }
    public static string GetAttackType(Attack attack, string color = "yellow") => Format.String(GetAttackTypeName(attack.m_attackType), color);
    public static string GetStaminaText(Attack attack, Skills.SkillType skillType)
    {
      if (attack == null) return "";
      var maxStamina = attack.m_attackStamina;
      var skillFactor = Player.m_localPlayer.GetSkillFactor(skillType);
      var stamina = maxStamina * (1 - 0.33f * skillFactor);
      return "Stamina: " + Format.Int(maxStamina, "orange") + " " + Format.String("(" + Format.Float(stamina) + ")");
    }
    public static string GetHitboxText(Attack attack, string color = "yellow")
    {
      if (attack == null || attack.m_attackType == Attack.AttackType.Projectile) return "";
      var texts = new List<string>();
      if (attack.m_attackRange > 0)
        texts.Add(Format.Float(attack.m_attackRange, Format.FORMAT, color) + " m");
      if (attack.m_attackAngle > 0)
        texts.Add(Format.Float(attack.m_attackAngle, Format.FORMAT, color) + " deg");
      if (attack.m_attackHeight > 0)
        texts.Add(Format.Float(attack.m_attackHeight, Format.FORMAT, color) + " h");
      if (attack.m_attackRayWidth > 0)
        texts.Add(Format.Float(attack.m_attackRayWidth, Format.FORMAT, color) + " ray");
      if (attack.m_attackOffset > 0)
        texts.Add(Format.Float(attack.m_attackOffset, Format.FORMAT, color) + " offset");
      return "Hit: " + string.Join(", ", texts);
    }
    public static string GetProjectileText(Attack attack, float holdDuration = 0, string color = "yellow")
    {
      if (attack == null || attack.m_attackType != Attack.AttackType.Projectile) return "";
      var texts = new List<string>();
      if (holdDuration > 0)
      {
        texts.Add("Spread: " + Format.Range(attack.m_projectileAccuracy, attack.m_projectileAccuracyMin, color));
        texts.Add("Velocity: " + Format.Range(attack.m_projectileVelMin, attack.m_projectileVel, color));
      }
      else
      {
        texts.Add("Spread: " + Format.Int(attack.m_projectileAccuracy, color));
        texts.Add("Velocity: " + Format.Int(attack.m_projectileVel, color));
      }
      if (attack.m_projectileBursts > 1)
        texts.Add("Burst: " + Format.Multiplier(attack.m_projectileBursts, color));
      if (attack.m_projectiles > 1)
        texts.Add("Projectiles: " + Format.Multiplier(attack.m_projectiles, color));
      return string.Join(", ", texts);
    }
    public static string GetAttackSpeed(string animation, float holdDuration)
    {
      if (animation == "atgeir_attack") return "2.98 s (0.84 + 0.86 + 1.28)";
      if (animation == "atgeir_secondary") return "1.74 s";
      if (animation == "battleaxe_attack") return "3.44 s (1.82 + 0.92 + 0.7)";
      if (animation == "battleaxe_secondary") return "0.94 s";
      if (animation == "bow_fire") return (0.8 + holdDuration).ToString(Format.FORMAT) + " s";
      if (animation == "emote_drink") return "";
      if (animation == "knife_stab") return "2.04 s (0.88 + 0.54 + 0.62)";
      if (animation == "knife_secondary") return "1.72 s";
      if (animation == "mace_secondary") return "1.72 s";
      if (animation == "spear_poke") return "0.68 s";
      if (animation == "spear_throw") return "1.57 s (includes picking up)";
      if (animation == "swing_axe") return "2.74 s (0.93 + 0.69 + 1.12)";
      if (animation == "swing_hammer") return "";
      if (animation == "swing_hoe") return "";
      // Also used for the mace.
      if (animation == "swing_longsword") return "2.46 s (1.02 + 0.68 + 0.76)";
      if (animation == "swing_pickaxe") return "1.4 s";
      if (animation == "swing_sledge") return "2.23 s";
      if (animation == "sword_secondary") return "1.84 s";
      return "";
    }
    public static string GetAttackSpeed(Attack attack, float holdDuration = 0, string color = "yellow")
    {
      if (attack == null) return "";
      var animation = attack.m_attackAnimation;
      var text = GetAttackSpeed(animation, holdDuration);
      if (text == "") return "";
      text = text.Replace("(", "<color=yellow>(");
      text = text.Replace(")", ")</color>");
      return "Speed: " + Format.String(text, color);
    }
  }
}