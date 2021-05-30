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
    public static void Prefix(Player __instance, ref bool ___m_noPlacementCost)
    {
      if (!Settings.useDegugMode) return;
      Player.m_debugMode = true;
      ___m_noPlacementCost = true;
      Player.m_debugMode = true;
      __instance.SetGodMode(true);
    }
  }
}