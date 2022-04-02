using BepInEx;
using HarmonyLib;
using Service;
namespace ESP;
[BepInDependency("valheim.jerekuusela.dps", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin("valheim.jerekuusela.esp", "ESP", "1.8.0.0")]
public class ESP : BaseUnityPlugin {
  public void Awake() {
    Settings.Init(Config);
    Harmony harmony = new("valheim.jerekuusela.esp");
    harmony.PatchAll();
    Admin.Instance = new EspAdmin();
    MessageHud_UpdateMessage.GetMessage = Hud.GetMessage;
  }
  public void LateUpdate() {
    if (Player.m_localPlayer)
      Texts.UpdateAverageSpeed(Ship.GetLocalShip());
  }
}
