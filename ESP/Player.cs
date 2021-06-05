using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Player), "StartEmote")]
  public class Player_StartEmote
  {
    public static void Postfix(Player __instance, string emote)
    {
      if (__instance == Player.m_localPlayer && emote == "sit")
        DPSMeter.Reset();
    }
  }
  [HarmonyPatch(typeof(Player), "UseStamina")]
  public class Player_UseStamina
  {
    public static void Prefix(Player __instance, float v)
    {
      if (__instance == Player.m_localPlayer)
        DPSMeter.AddStamina(v);
    }
  }
}