using System.Linq;

namespace ESP
{
  public class DamageModifierUtils
  {
    private static HitData.DamageType[] DAMAGE_TYPES = new HitData.DamageType[]{
      HitData.DamageType.Blunt,
      HitData.DamageType.Chop,
      HitData.DamageType.Elemental,
      HitData.DamageType.Fire,
      HitData.DamageType.Frost,
      HitData.DamageType.Lightning,
      HitData.DamageType.Physical,
      HitData.DamageType.Pickaxe,
      HitData.DamageType.Pierce,
      HitData.DamageType.Poison,
      HitData.DamageType.Slash,
      HitData.DamageType.Spirit
    };
    public static string DamageTypeToString(HitData.DamageType damageType)
    {
      if (damageType == HitData.DamageType.Blunt) return "Blunt";
      if (damageType == HitData.DamageType.Chop) return "Chop";
      if (damageType == HitData.DamageType.Elemental) return "Elemental";
      if (damageType == HitData.DamageType.Fire) return "Fire";
      if (damageType == HitData.DamageType.Frost) return "Frost";
      if (damageType == HitData.DamageType.Lightning) return "Lightning";
      if (damageType == HitData.DamageType.Physical) return "Physical";
      if (damageType == HitData.DamageType.Pickaxe) return "Pickaxe";
      if (damageType == HitData.DamageType.Pierce) return "Pierce";
      if (damageType == HitData.DamageType.Poison) return "Poison";
      if (damageType == HitData.DamageType.Slash) return "Slash";
      if (damageType == HitData.DamageType.Spirit) return "Spirit";
      return "";
    }
    private static string GetModifierText(HitData.DamageModifiers modifiers, HitData.DamageType damageType)
    {
      var name = DamageTypeToString(damageType);
      var modifier = modifiers.GetModifier(damageType);
      if (modifier == HitData.DamageModifier.Immune) return name + ": " + TextUtils.String("x0");
      if (modifier == HitData.DamageModifier.Resistant) return name + ": " + TextUtils.String("x0.5");
      if (modifier == HitData.DamageModifier.VeryResistant) return name + ": " + TextUtils.String("x0.25");
      if (modifier == HitData.DamageModifier.Weak) return name + ": " + TextUtils.String("x1.5");
      if (modifier == HitData.DamageModifier.VeryWeak) return name + ": " + TextUtils.String("x2");
      return "";
    }
    public static string GetText(HitData.DamageModifiers modifiers)
    {
      var texts = DAMAGE_TYPES.Select(type => GetModifierText(modifiers, type)).Where(text => text.Length > 0);
      if (texts.Count() > 0)
        return "\n" + string.Join("\n", texts);
      return "";
    }
  }
}

