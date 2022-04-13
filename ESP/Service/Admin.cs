using HarmonyLib;
namespace Service;
///<summary>Static accessors for easier usage.</summary>
public static class Admin {
  public static IAdmin Instance = new DefaultAdmin();
  ///<summary>Admin status.</summary>
  public static bool Enabled {
    get => Instance.Enabled;
    set => Instance.Enabled = value;
  }
  ///<summary>Is admin status currently being checked.</summary>
  public static bool Checking {
    get => Instance.Checking;
    set => Instance.Checking = value;
  }
  ///<summary>Checks for admin status. Terminal is used for the output.</summary>
  public static void Check() => Instance.Check();
  ///<summary>Verifies the admin status with a given text. Shouldn't be called directly.</summary>
  public static void Verify(string text) => Instance.Verify(text);
}

public interface IAdmin {
  bool Enabled { get; set; }
  bool Checking { get; set; }
  void Check();
  void Verify(string text);
}

///<summary>Admin checker. Can be extended by overloading OnSuccess and OnFail.</summary>
public class DefaultAdmin : IAdmin {
  public virtual bool Enabled { get; set; }
  ///<summary>Admin status is checked by issuing a dummy unban command.</summary>
  public void Check() {
    if (!ZNet.instance) return;
    Checking = true;
    if (ZNet.instance.IsServer())
      OnSuccess();
    else
      ZNet.instance.Unban("admintest");
  }
  public void Verify(string text) {
    if (text == "Unbanning user admintest")
      OnSuccess();
    else
      OnFail();
  }
  public virtual bool Checking { get; set; }
  protected virtual void OnSuccess() {
    Checking = false;
    Enabled = true;
  }
  protected virtual void OnFail() {
    Checking = false;
    Enabled = false;
  }
}

[HarmonyPatch(typeof(ZNet), nameof(ZNet.RPC_RemotePrint))]
public class ZNet_RPC_RemotePrint {
  static bool Prefix(string text) {
    if (!Admin.Checking) return true;
    Admin.Verify(text);
    return false;
  }
}

///<summary>Check admin status on connect to ensure features are enabled/disabled when changing servers.</summary>
[HarmonyPatch(typeof(Game), nameof(Game.UpdateRespawn))]
public class CheckAdmin {
  static void Prefix(bool ___m_firstSpawn, ref bool __state) {
    if (___m_firstSpawn) Admin.Enabled = false;
    __state = ___m_firstSpawn;
  }
  static void Postfix(bool ___m_firstSpawn, bool __state) {
    if (__state && !___m_firstSpawn && !Admin.Checking) Admin.Check();
  }
}
