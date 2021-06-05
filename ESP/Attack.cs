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
}