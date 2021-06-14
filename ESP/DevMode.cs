using HarmonyLib;

namespace ESP
{
  public class Cheats
  {
    public static bool Enabled
    {
      get => ZNet.instance && ZNet.instance.IsServer();
    }
  }
  [HarmonyPatch(typeof(Console), "Awake")]
  public class Console_Awake
  {
    public static void Postfix(Console __instance, ref bool ___m_cheat)
    {
      if (!Settings.useDegugMode) return;
      ___m_cheat = Cheats.Enabled;
    }
  }

  [HarmonyPatch(typeof(Console), "IsConsoleEnabled")]
  public class Console_IsConsoleEnabled
  {
    public static void Postfix(ref bool __result)
    {
      __result = true;
    }
  }
  [HarmonyPatch(typeof(Console), "IsCheatsEnabled")]
  public class Console_IsCheatsEnabled
  {
    public static void Postfix(ref bool __result)
    {
      __result = __result || Cheats.Enabled;
    }
  }


  [HarmonyPatch(typeof(Player), "Awake")]
  public class Player_Awake
  {
    public static void Postfix(Player __instance, ref bool ___m_noPlacementCost, ref bool ___m_debugFly)
    {
      if (!Cheats.Enabled || !Settings.useDegugMode) return;
      Player.m_debugMode = true;
      ___m_noPlacementCost = Settings.useFreeBuild;
      ___m_debugFly = Settings.useFreeFly;
      __instance.SetGodMode(Settings.useGodMode);
    }
  }
}