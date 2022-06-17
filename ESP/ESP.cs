using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using Service;
namespace ESP;
[BepInDependency("valheim.jerekuusela.dps", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("org.bepinex.plugins.jewelcrafting", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin("valheim.jerekuusela.esp", "ESP", "1.11.0.0")]
public class ESP : BaseUnityPlugin {
  private static ManualLogSource? Logs;
  public static ManualLogSource Log => Logs!;
  public void Awake() {
    Logs = Logger;
    Settings.Init(Config);
    Harmony harmony = new("valheim.jerekuusela.esp");
    harmony.PatchAll();
    Admin.Instance = new EspAdmin();
    MessageHud_UpdateMessage.GetMessage = Hud.GetMessage;
  }
  public void Start() {
    if (Chainloader.PluginInfos.TryGetValue("org.bepinex.plugins.jewelcrafting", out var info))
      JewelcraftingPatcher.DoPatching(info.Instance.GetType().Assembly);
  }
  public void LateUpdate() {
    if (Player.m_localPlayer)
      Texts.UpdateAverageSpeed(Ship.GetLocalShip());
  }
}
