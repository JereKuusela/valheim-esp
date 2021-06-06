using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(TreeBase), "RPC_Damage")]
  public class TreeBase_RPC_Damage
  {
    public static void Postfix(TreeBase __instance, HitData hit)
    {
      if (hit.GetAttacker() == Player.m_localPlayer)
        DPSMeter.AddStructureDamage(hit, __instance);
    }
  }
}