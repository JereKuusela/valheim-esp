using System.Linq;
using Service;
namespace ESP;
public class DamageModifierUtils {
  private static HitData.DamageType[] DAMAGE_TYPES = new[]{
      HitData.DamageType.Blunt,
      HitData.DamageType.Chop,
      HitData.DamageType.Fire,
      HitData.DamageType.Frost,
      HitData.DamageType.Lightning,
      HitData.DamageType.Pickaxe,
      HitData.DamageType.Pierce,
      HitData.DamageType.Poison,
      HitData.DamageType.Slash,
      HitData.DamageType.Spirit
    };
  public static string DamageTypeToString(HitData.DamageType damageType) {
    if (damageType == HitData.DamageType.Blunt) return "Blunt";
    if (damageType == HitData.DamageType.Chop) return "Chop";
    if (damageType == HitData.DamageType.Fire) return "Fire";
    if (damageType == HitData.DamageType.Frost) return "Frost";
    if (damageType == HitData.DamageType.Lightning) return "Lightning";
    if (damageType == HitData.DamageType.Pickaxe) return "Pickaxe";
    if (damageType == HitData.DamageType.Pierce) return "Pierce";
    if (damageType == HitData.DamageType.Poison) return "Poison";
    if (damageType == HitData.DamageType.Slash) return "Slash";
    if (damageType == HitData.DamageType.Spirit) return "Spirit";
    return "";
  }
  private static string GetModifierText(HitData.DamageModifiers modifiers, HitData.DamageType damageType, bool ignoreNeutral, bool ignoreIgnore) {
    var name = DamageTypeToString(damageType);
    var modifier = modifiers.GetModifier(damageType);
    if (ignoreNeutral && modifier == HitData.DamageModifier.Immune) return name + ": " + Format.String("x0");
    if ((ignoreNeutral && !ignoreIgnore) && modifier == HitData.DamageModifier.Ignore) return name + ": " + Format.String("x0");
    if (modifier == HitData.DamageModifier.Resistant) return name + ": " + Format.String("x0.5");
    if (modifier == HitData.DamageModifier.VeryResistant) return name + ": " + Format.String("x0.25");
    if (modifier == HitData.DamageModifier.Weak) return name + ": " + Format.String("x1.5");
    if (modifier == HitData.DamageModifier.VeryWeak) return name + ": " + Format.String("x2");
    if (!ignoreNeutral && modifier == HitData.DamageModifier.Normal) return name + ": " + Format.String("x1");
    return "";
  }
  public static string Get(HitData.DamageModifiers modifiers, bool ignoreNeutral, bool ignoreIgnore) {
    if (!Settings.Resistances) return "";
    var texts = DAMAGE_TYPES.Select(type => GetModifierText(modifiers, type, ignoreNeutral, ignoreIgnore)).Where(text => text.Length > 0);
    return Format.JoinRow(texts);
  }
}
