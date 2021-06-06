using System;

namespace ESP
{
  public class DPSMeter
  {
    private static DateTime? startTime = null;
    private static DateTime? endTime = null;
    private static float damageTaken = 0;
    private static float damage = 0;
    private static float structureDamage = 0;
    private static float baseDamage = 0;
    private static float baseStructureDamage = 0;
    // Pending damages are used to update DPS at end of the attack to make it more stable.
    private static float pendingDamage = 0;
    private static float pendingStructureDamage = 0;
    private static float pendingBaseDamage = 0;
    private static float pendingBaseStructureDamage = 0;
    private static HitData.DamageTypes? pendingBaseDamageTypes = null;
    private static float stagger = 0;
    private static float stamina = 0;
    private static float hits = 0;
    public static void Start()
    {
      if (!Settings.showDPS) return;
      if (startTime.HasValue) return;
      startTime = DateTime.Now;
      endTime = null;
      damageTaken = 0;
      damage = 0;
      structureDamage = 0;
      baseDamage = 0;
      baseStructureDamage = 0;
      pendingDamage = 0;
      pendingStructureDamage = 0;
      pendingBaseDamage = 0;
      pendingBaseStructureDamage = 0;
      pendingBaseDamageTypes = null;
      stagger = 0;
      stamina = 0;
      hits = 0;
    }
    public static void Reset()
    {
      startTime = null;
      endTime = null;
    }
    public static void AddBaseDamage(HitData.DamageTypes hit)
    {
      // Base damage is only available at start of the attack so it must be stored when the actual hits are resolved.
      pendingBaseDamageTypes = hit;
    }
    public static void AddRawHit(HitData hit)
    {
    }
    public static void AddStructureDamage(HitData hit, TreeLog treeLog)
    {
      if (hit.m_toolTier < treeLog.m_minToolTier) return;
      pendingStructureDamage += hit.GetTotalDamage();
      AddPendingBaseStructureDamage(treeLog.m_damages);
    }
    public static void AddStructureDamage(HitData hit, TreeBase treeBase)
    {
      if (hit.m_toolTier < treeBase.m_minToolTier) return;
      pendingStructureDamage += hit.GetTotalDamage();
      AddPendingBaseStructureDamage(treeBase.m_damageModifiers);
    }
    public static void AddStructureDamage(HitData hit, MineRock5 mineRock5)
    {
      if (hit.m_toolTier < mineRock5.m_minToolTier) return;
      pendingStructureDamage += hit.GetTotalDamage();
      AddPendingBaseStructureDamage(mineRock5.m_damageModifiers);
    }
    public static void AddStructureDamage(HitData hit, MineRock mineRock)
    {
      if (hit.m_toolTier < mineRock.m_minToolTier) return;
      pendingStructureDamage += hit.GetTotalDamage();
      AddPendingBaseStructureDamage(mineRock.m_damageModifiers);
    }
    public static void AddStructureDamage(HitData hit, Destructible destructible)
    {
      if (hit.m_toolTier < destructible.m_minToolTier) return;
      pendingStructureDamage += hit.GetTotalDamage();
      AddPendingBaseStructureDamage(destructible.m_damages);
    }
    public static void AddDamageTaken(HitData hit)
    {
      damageTaken += hit.GetTotalDamage();
      SetTime();
    }
    private static void AddPendingBaseDamage(Character target)
    {
      var hit = new HitData()
      {
        m_damage = pendingBaseDamageTypes.Value.Clone()
      };
      var damageModifiers = Patch.Character_GetDamageModifiers(target);
      hit.ApplyResistance(damageModifiers, out var mod);
      if (target.IsPlayer())
      {
        float bodyArmor = target.GetBodyArmor();
        hit.ApplyArmor(bodyArmor);
      }
      pendingBaseDamage += hit.GetTotalDamage();
    }
    private static void AddPendingBaseStructureDamage(HitData.DamageModifiers modifiers)
    {
      var hit = new HitData()
      {
        m_damage = pendingBaseDamageTypes.Value.Clone()
      };
      hit.ApplyResistance(modifiers, out var mod);
      pendingBaseStructureDamage += hit.GetTotalDamage();
    }
    public static void AddDamage(HitData hit, Character target)
    {
      AddPendingBaseDamage(target);
      pendingDamage += hit.GetTotalDamage();
      stagger += hit.m_damage.GetTotalPhysicalDamage() * hit.m_staggerMultiplier;
      hits++;
    }
    public static void AddDot(HitData hit)
    {
      pendingDamage += hit.GetTotalDamage();
    }
    public static void AddStamina(float stamina)
    {
      DPSMeter.stamina += stamina;
    }
    public static void SetTime()
    {
      endTime = DateTime.Now;
      damage += pendingDamage;
      pendingDamage = 0;
      baseDamage += pendingBaseDamage;
      pendingBaseDamage = 0;
      structureDamage += pendingStructureDamage;
      pendingStructureDamage = 0;
      baseStructureDamage += pendingBaseStructureDamage;
      pendingBaseStructureDamage = 0;

    }
    public static string GetText()
    {
      if (!startTime.HasValue || !endTime.HasValue) return "";
      var time = endTime.Value.Subtract(startTime.Value).TotalMilliseconds;
      var damagePerSecond = damage * 1000.0 / time;
      var baseDamagePerSecond = baseDamage * 1000.0 / time;
      var staminaPerSecond = stamina * 1000.0 / time;
      var damagePerStamina = (damage + pendingDamage) / stamina;
      var baseDamagePerStamina = (baseDamage + pendingBaseDamage) / stamina;
      var staggerPerSecond = stagger * 1000.0 / time;
      var hitsPerSecond = hits * 1000.0 / time;
      var attackSpeed = hits > 0 ? time / hits / 1000.0 : 0;
      var damageTakenPerSecond = damageTaken * 1000.0 / time;
      var text = "\n\n\n";
      if (structureDamage > 0)
        text += "\n\n";
      text += "\nTime: " + TextUtils.Float(time / 1000.0) + " seconds with " + TextUtils.Float(hits) + " hits";
      text += "\nDPS: " + TextUtils.Float(damagePerSecond) + " (total " + TextUtils.Float(damage + pendingDamage) + ")";
      text += ", per stamina: " + TextUtils.Float(damagePerStamina);
      text += "\nBase DPS: " + TextUtils.Float(baseDamagePerSecond) + " (total " + TextUtils.Float(baseDamage + pendingBaseDamage) + ")";
      text += ", per stamina: " + TextUtils.Float(baseDamagePerStamina);
      text += "\nSPS: " + TextUtils.Float(staminaPerSecond) + " (total " + TextUtils.Float(stamina) + ")";
      text += "\nStagger per second: " + TextUtils.Float(staggerPerSecond) + " (" + TextUtils.Float(stagger) + ")";
      text += "\nAttack speed: " + TextUtils.Float(attackSpeed) + " s (" + TextUtils.Float(hitsPerSecond) + " per second)";
      text += "\nDamage taken: " + TextUtils.Float(damageTakenPerSecond) + " (total " + TextUtils.Float(damageTaken) + ")";
      if (structureDamage > 0)
      {
        var structureDamagePerSecond = structureDamage * 1000.0 / time;
        var structureDamagePerStamina = (structureDamage + pendingStructureDamage) / stamina;
        var baseStructureDamagePerSecond = baseStructureDamage * 1000.0 / time;
        var baseStructureDamagePerStamina = (baseStructureDamage + pendingBaseStructureDamage) / stamina;
        text += "\nStructures DPS: " + TextUtils.Float(structureDamagePerSecond) + " (total " + TextUtils.Float(structureDamage + pendingStructureDamage) + ")";
        text += ", per stamina: " + TextUtils.Float(structureDamagePerStamina);
        text += "\nBase DPS: " + TextUtils.Float(baseStructureDamagePerSecond) + " (total " + TextUtils.Float(baseStructureDamage + pendingBaseStructureDamage) + ")";
        text += ", per stamina: " + TextUtils.Float(baseStructureDamagePerStamina);

      }
      return text;
    }
  }
}