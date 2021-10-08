using HarmonyLib;

namespace Authorization {

  ///<summary>Static accessors for easier usage.</summary>
  public static class Admin {
    public static IAdmin Instance = new DefaultAdmin();
    public static bool Enabled {
      get => Instance.Enabled;
      set => Instance.Enabled = value;
    }
    public static bool Checking {
      get => Instance.Checking;
      set => Instance.Checking = value;
    }
    public static bool Check() => Instance.Check();

  }

  public interface IAdmin {
    bool Enabled { get; set; }
    bool Checking { get; set; }
    bool Check();
  }

  ///<summary>Default implementation. Can be extended and overwritten.</summary>
  public class DefaultAdmin : IAdmin {
    public virtual bool Check() {
      if (Enabled) return true;
      // Automatically pass locally.
      if (ZNet.instance && ZNet.instance.IsServer()) return true;
      if (ZNet.instance) {
        Checking = true;
        ZNet.instance.Unban("admintest");
      }
      return false;
    }
    public virtual bool Enabled { get; set; }
    public virtual bool Checking { get; set; }
  }

  [HarmonyPatch(typeof(ZNet), "RPC_RemotePrint")]
  public class ZNet_RPC_RemotePrint {
    public static bool Prefix(string text) {
      if (Admin.Checking) {
        if (text == "Unbanning user admintest")
          Console.instance.TryRunCommand("devcommands");
        else
          Console.instance.Print("Unauthorized to use devcommands.");

        Admin.Checking = false;
        return false;
      }
      return true;
    }
  }

  // Replace devcommands check with a custom one.
  [HarmonyPatch(typeof(Terminal), "TryRunCommand")]
  public class TryRunCommand {
    public static bool Prefix(string text) {
      string[] array = text.Split(' ');
      // Let other commands pass normally.
      if (array[0] != "devcommands") {
        return true;
      }
      // Devcommands during admin check means that the check passed.
      if (Admin.Checking) {
        Admin.Enabled = true;
        return true;
      }
      // Disabling doesn't require admin check.
      if (Admin.Enabled) {
        Admin.Enabled = false;
        return true;
      }
      return Admin.Check();
    }
  }

  // Cheats must be disabled when joining servers (so that locally enabling doesn't work).
  [HarmonyPatch(typeof(ZNet), "Start")]
  public class ZNet_Start {
    public static void Postfix() {
      Admin.Enabled = (ZNet.instance && ZNet.instance.IsServer());
    }
  }

  [HarmonyPatch(typeof(Console), "IsConsoleEnabled")]
  public class IsConsoleEnabled {
    public static void Postfix(ref bool __result) {
      __result = true;
    }
  }
  // Must be patched because contains a "is server check".
  [HarmonyPatch(typeof(Terminal), "IsCheatsEnabled")]
  public class IsCheatsEnabled {
    public static void Postfix(ref bool __result) {
      __result = __result || Admin.Enabled;
    }
  }
  // Must be patched because contains a "is server check".
  [HarmonyPatch(typeof(Terminal.ConsoleCommand), "IsValid")]
  public class IsValid {
    public static void Postfix(ref bool __result) {
      __result = __result || Admin.Enabled;
    }
  }
}
