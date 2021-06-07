using HarmonyLib;

namespace ESP
{

  [HarmonyPatch(typeof(Console), "Awake")]
  public class Console_Awake
  {
    public static void Prefix(Console __instance, ref bool ___m_cheat)
    {
      if (!Settings.useDegugMode) return;
      ___m_cheat = true;
    }
  }

  [HarmonyPatch(typeof(Console), "IsConsoleEnabled")]
  public class Console_IsConsoleEnabled
  {
    public static bool Prefix(ref bool __result)
    {
      __result = true;
      return false;
    }
  }

  [HarmonyPatch(typeof(Player), "Awake")]
  public class Player_Awake
  {
    public static void Prefix(Player __instance, ref bool ___m_noPlacementCost, ref bool ___m_debugFly)
    {
      if (!Settings.useDegugMode) return;
      Player.m_debugMode = true;
      ___m_noPlacementCost = Settings.useFreeBuild;
      ___m_debugFly = Settings.useFreeFly;
      __instance.SetGodMode(Settings.useGodMode);
    }
  }
}