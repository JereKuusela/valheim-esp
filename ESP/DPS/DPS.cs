using HarmonyLib;

namespace ESP
{
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
}