using BepInEx;
using HarmonyLib;
using Service;

namespace ESP {
  [BepInDependency("valheim.jerekuusela.dps", BepInDependency.DependencyFlags.SoftDependency)]
  [BepInPlugin("valheim.jerekuusela.esp", "ESP", "1.5.0.0")]
  public class ESP : BaseUnityPlugin {
    public void Awake() {
      Settings.Init(Config);
      var harmony = new Harmony("valheim.jerekuusela.esp");
      harmony.PatchAll();
      Admin.Instance = new EspAdmin();
      MessageHud_UpdateMessage.GetMessage = Hud.GetMessage;
    }
    public void LateUpdate() {
      if (Player.m_localPlayer)
        Texts.UpdateAverageSpeed(Ship.GetLocalShip());
    }
  }

  [HarmonyPatch(typeof(Chat), "Awake")]
  public class ChatBind {
    public static void Postfix(Console __instance) {
      if (!Settings.configFirstRun.Value) return;
      Settings.configFirstRun.Value = false;
      var binds = Terminal.m_bindList;
      if (binds == null) UnityEngine.Debug.LogError("No binds!");
      while (true) {
        var index = binds.FindIndex(item => item.Contains("esp_"));
        if (index == -1) break;
        binds.RemoveAt(index);
      }
      __instance.TryRunCommand("bind o esp_toggle " + Tool.ExtraInfo);
      __instance.TryRunCommand("bind j esp_toggle " + Tag.Ruler);
    }
  }
}
