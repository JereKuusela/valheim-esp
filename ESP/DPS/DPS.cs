using HarmonyLib;

namespace ESP
{
  #region Tracking attacks for DPS meter.
  [HarmonyPatch(typeof(Attack), "Start")]
  public class Attack_Start
  {
    public static void Postfix(Attack __instance, Humanoid character, bool __result)
    {

      if (__result && character == Player.m_localPlayer)
      {
        DPSMeter.Start();
        DPSMeter.AddBaseDamage(__instance.GetWeapon().GetDamage());

      }
    }
  }
  [HarmonyPatch(typeof(Attack), "OnAttackDone")]
  public class Attack_OnAttackDone
  {
    public static void Postfix(Humanoid ___m_character)
    {
      if (___m_character == Player.m_localPlayer)
        DPSMeter.SetTime();
    }
  }
  #endregion
  #region Tracking structure damage for DPS meter.
  [HarmonyPatch(typeof(Destructible), "RPC_Damage")]
  public class Destructible_RPC_Damage
  {
    public static void Postfix(Destructible __instance, HitData hit)
    {
      if (hit.GetAttacker() == Player.m_localPlayer)
        DPSMeter.AddStructureDamage(hit, __instance);
    }
  }
  [HarmonyPatch(typeof(MineRock), "RPC_Hit")]
  public class MineRock_RPC_Hit
  {
    public static void Postfix(MineRock __instance, HitData hit)
    {
      if (hit.GetAttacker() == Player.m_localPlayer)
        DPSMeter.AddStructureDamage(hit, __instance);
    }
  }
  [HarmonyPatch(typeof(WearNTear), "RPC_Damage")]
  public class WearNTear_RPC_Damage
  {
    public static void Postfix(WearNTear __instance, HitData hit)
    {
      if (hit.GetAttacker() == Player.m_localPlayer)
        DPSMeter.AddStructureDamage(hit, __instance);
    }
  }
  [HarmonyPatch(typeof(MineRock5), "DamageArea")]
  public class MineRock5_DamageArea
  {
    public static void Postfix(MineRock5 __instance, HitData hit)
    {
      if (hit.GetAttacker() == Player.m_localPlayer)
        DPSMeter.AddStructureDamage(hit, __instance);
    }
  }
  [HarmonyPatch(typeof(TreeBase), "RPC_Damage")]
  public class TreeBase_RPC_Damage
  {
    public static void Postfix(TreeBase __instance, HitData hit)
    {
      if (hit.GetAttacker() == Player.m_localPlayer)
        DPSMeter.AddStructureDamage(hit, __instance);
    }
  }
  public class TreeLogUtils
  {
    [HarmonyPatch(typeof(TreeLog), "RPC_Damage")]
    public class TreeLog_RPC_Damage
    {
      public static void Postfix(TreeLog __instance, HitData hit)
      {
        if (hit.GetAttacker() == Player.m_localPlayer)
          DPSMeter.AddStructureDamage(hit, __instance);
      }
    }
  }
  #endregion
  #region Tracking creature damage for DPS meter
  [HarmonyPatch(typeof(Character), "RPC_Damage")]
  public class Character_RPC_Damage
  {
    public static void Prefix(HitData hit)
    {
      if (hit.GetAttacker() == Player.m_localPlayer)
        DPSMeter.AddRawHit(hit);
    }
  }
  [HarmonyPatch(typeof(Character), "ApplyDamage")]
  public class Character_ApplyDamage
  {
    public static void Prefix(Character __instance, HitData hit)
    {
      if (hit.GetAttacker() == Player.m_localPlayer)
        DPSMeter.AddDamage(hit, __instance);
      if (hit.GetAttacker() == null)
        DPSMeter.AddDot(hit);
      if (__instance == Player.m_localPlayer)
        DPSMeter.AddDamageTaken(hit);
    }
  }
  #endregion
  #region Tracking other things for DPS meter.
  [HarmonyPatch(typeof(Player), "UseStamina")]
  public class Player_UseStamina
  {
    public static void Prefix(Player __instance, float v)
    {
      if (__instance == Player.m_localPlayer)
        DPSMeter.AddStamina(v);
    }
  }
  #endregion
}