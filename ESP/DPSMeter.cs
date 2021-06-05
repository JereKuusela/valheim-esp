using System;

namespace ESP
{
  public class DPSMeter
  {
    private static DateTime? startTime = null;
    private static DateTime? endTime = null;
    private static float totalDamageTaken = 0;
    private static float totalDamage = 0;
    private static float totalBaseDamage = 0;
    // Pending damages are used to update DPS at end of the attack to make it more stable.
    private static float totalPendingDamage = 0;
    private static float totalPendingBaseDamage = 0;
    private static HitData.DamageTypes? pendingBaseDamage = null;
    private static float totalStagger = 0;
    private static float totalStamina = 0;
    private static float totalHits = 0;
    public static void Start()
    {
      if (startTime.HasValue) return;
      startTime = DateTime.Now;
      endTime = null;
      totalDamage = 0;
      totalPendingBaseDamage = 0;
      totalStagger = 0;
      totalStamina = 0;
      totalHits = 0;
      totalPendingDamage = 0;
      totalDamageTaken = 0;
      pendingBaseDamage = null;
    }
    public static void Reset()
    {
      startTime = null;
      endTime = null;
    }
    public static void AddBaseDamage(HitData.DamageTypes hit)
    {
      // Base damage is only available at start of the attack so it must be stored when the actual hits are resolved.
      pendingBaseDamage = hit;
    }
    public static void AddRawHit(HitData hit)
    {
    }
    public static void AddDamageTaken(HitData hit)
    {
      totalDamageTaken += hit.GetTotalDamage();
      SetTime();
    }
    private static void AddPendingBaseDamage(Character target)
    {
      var hit = new HitData()
      {
        m_damage = pendingBaseDamage.Value.Clone()
      };
      var damageModifiers = Patch.Character_GetDamageModifiers(target);
      hit.ApplyResistance(damageModifiers, out var mod);
      if (target.IsPlayer())
      {
        float bodyArmor = target.GetBodyArmor();
        hit.ApplyArmor(bodyArmor);
      }
      totalPendingBaseDamage += hit.GetTotalDamage();
    }
    public static void AddDamage(HitData hit, Character target)
    {
      AddPendingBaseDamage(target);
      totalPendingDamage += hit.GetTotalDamage();
      totalStagger += hit.m_damage.GetTotalPhysicalDamage() * hit.m_staggerMultiplier;
      totalHits++;
    }
    public static void AddDot(HitData hit)
    {
      totalPendingDamage += hit.GetTotalDamage();
    }
    public static void AddStamina(float stamina)
    {
      totalStamina += stamina;
    }
    public static void SetTime()
    {
      endTime = DateTime.Now;
      totalDamage += totalPendingDamage;
      totalPendingDamage = 0;
      totalBaseDamage += totalPendingBaseDamage;
      totalPendingBaseDamage = 0;

    }
    public static string GetText()
    {
      if (!startTime.HasValue || !endTime.HasValue) return "";
      var time = endTime.Value.Subtract(startTime.Value).TotalMilliseconds;
      var damagePerSecond = totalDamage * 1000.0 / time;
      var baseDamagePerSecond = totalBaseDamage * 1000.0 / time;
      var staminaPerSecond = totalStamina * 1000.0 / time;
      var damagePerStamina = (totalDamage + totalPendingDamage) / totalStamina;
      var baseDamagePerStamina = (totalBaseDamage + totalPendingBaseDamage) / totalStamina;
      var staggerPerSecond = totalStagger * 1000.0 / time;
      var hitsPerSecond = totalHits * 1000.0 / time;
      var attackSpeed = totalHits > 0 ? time / totalHits / 1000.0 : 0;
      var damageTakenPerSecond = totalDamageTaken * 1000.0 / time;
      var text = "\n\n\n\n\n\nTime: " + TextUtils.Float(time / 1000.0) + " seconds with " + TextUtils.Float(totalHits) + " hits";
      text += "\nDPS: " + TextUtils.Float(damagePerSecond) + " (total " + TextUtils.Float(totalDamage + totalPendingDamage) + ")";
      text += ", per stamina: " + TextUtils.Float(damagePerStamina);
      text += "\nBase DPS: " + TextUtils.Float(baseDamagePerSecond) + " (total " + TextUtils.Float(totalBaseDamage + totalPendingBaseDamage) + ")";
      text += ", per stamina: " + TextUtils.Float(baseDamagePerStamina);
      text += "\nSPS: " + TextUtils.Float(staminaPerSecond) + " (total " + TextUtils.Float(totalStamina) + ")";
      text += "\nStagger per second: " + TextUtils.Float(staggerPerSecond) + " (" + TextUtils.Float(totalStagger) + ")";
      text += "\nAttack speed: " + TextUtils.Float(attackSpeed) + " s (" + TextUtils.Float(hitsPerSecond) + " per second)";
      text += "\nDamage taken: " + TextUtils.Float(damageTakenPerSecond) + " (total " + TextUtils.Float(totalDamageTaken) + ")";
      return text;
    }
  }
}