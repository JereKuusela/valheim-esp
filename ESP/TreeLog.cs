using HarmonyLib;

namespace ESP
{
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