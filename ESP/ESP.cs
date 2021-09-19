using BepInEx;
using HarmonyLib;

namespace ESP {
  [BepInPlugin("valheim.jerekuusela.esp", "ESP", "1.5.0.0")]
  public class ESP : BaseUnityPlugin {
    public void Awake() {
      Settings.Init(Config);
      var harmony = new Harmony("valheim.jerekuusela.esp");
      harmony.PatchAll();
      Admin.Instance = new EspAdmin();
    }

    public void Update() {
      if (Player.m_localPlayer)
        Texts.UpdateAverageSpeed(Ship.GetLocalShip());
    }
  }
}
