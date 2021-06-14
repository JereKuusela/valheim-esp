using System.Linq;

namespace ESP
{
  public partial class HoverTextUtils
  {
    public static string GetSenseText(MonsterAI obj)
    {
      var range = obj.m_alertRange;
      var angle = obj.m_viewAngle;
      return "Alert range: " + TextUtils.Int(range) + "\nAlert angle: " + TextUtils.Int(angle);
    }

    private static string GetTargetName(ItemDrop.ItemData.AiTarget target)
    {
      if (target == ItemDrop.ItemData.AiTarget.Enemy) return "Damage";
      if (target == ItemDrop.ItemData.AiTarget.Friend) return "Support";
      if (target == ItemDrop.ItemData.AiTarget.FriendHurt) return "Heal";
      return "";
    }
    private static string GetDamages(HitData.DamageTypes target) => target.GetTooltipString().Replace("\n", ", ");
    public static string GetAttackText(Humanoid obj)
    {
      var weapons = obj.GetInventory().GetAllItems().Where(item => item.IsWeapon());
      var texts = weapons.Select(weapon =>
      {
        var data = weapon.m_shared;
        var header = TextUtils.Name(weapon) + " (" + TextUtils.String(GetTargetName(data.m_aiTargetType));
        var text = TextUtils.ProgressPercent(header, weapon.m_lastAttackTime, data.m_aiAttackInterval);
        text += "Range: " + TextUtils.Range(data.m_aiAttackRangeMin, data.m_aiAttackRange) + "(" + TextUtils.Int(data.m_aiAttackMaxAngle) + " degrees)";
        if (data.m_aiPrioritized)
          text += ", " + TextUtils.String("priority");
        text += GetDamages(weapon.GetDamage());
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
        text += Texts.GetHitboxText(data.m_attack);
        return text;
      });
      return string.Join("\n", weapons);
    }
  }
}

