using HarmonyLib;

namespace ESP
{
  public class Cheats
  {
    public static void CheckAdmin()
    {
      if (ZNet.instance && !IsAdmin)
      {
        CheckingAdmin = true;
        ZNet.instance.Unban("admintest");
      }
    }
    public static bool CheckingAdmin = false;
    private static bool isAdmin = false;
    public static bool IsAdmin
    {
      get => isAdmin || (ZNet.instance && ZNet.instance.IsServer());
      set
      {
        isAdmin = value;
        Drawer.CheckVisibility();
        SupportUtils.SetVisibility(Drawer.showOthers);
      }
    }
  }
  [HarmonyPatch(typeof(Console), "Awake")]
  public class Console_Awake
  {
    public static void Postfix(Console __instance, ref bool ___m_cheat)
    {
      if (!Settings.useDegugMode) return;
      ___m_cheat = Cheats.IsAdmin;
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
      __result = __result || Cheats.IsAdmin;
    }
  }


  [HarmonyPatch(typeof(Player), "Awake")]
  public class Player_Awake
  {
    public static void Postfix(Player __instance, ref bool ___m_noPlacementCost, ref bool ___m_debugFly)
    {
      if (!Cheats.IsAdmin || !Settings.useDegugMode) return;
      Player.m_debugMode = true;
      ___m_noPlacementCost = Settings.useFreeBuild;
      ___m_debugFly = Settings.useFreeFly;
      __instance.SetGodMode(Settings.useGodMode);
    }
  }

  [HarmonyPatch(typeof(ZNet), "RPC_RemotePrint")]
  public class ZNet_RPC_RemotePrint
  {
    public static bool Prefix(string text)
    {
      if (Cheats.CheckingAdmin)
      {
        Cheats.CheckingAdmin = false;
        if (text == "Unbanning user admintest")
          Cheats.IsAdmin = true;
        return false;
      }
      return true;
    }
  }
  // Cheats must be disabled when joining servers (so that locally enabling doesn't work).
  [HarmonyPatch(typeof(ZNet), "Start")]
  public class ZNet_Start
  {
    public static void Postfix()
    {
      Cheats.IsAdmin = false;
    }
  }
}