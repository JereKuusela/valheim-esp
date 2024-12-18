using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using Service;
using Visualization;
namespace ESP;
[BepInDependency("org.bepinex.plugins.jewelcrafting", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin(GUID, NAME, VERSION)]
public class ESP : BaseUnityPlugin
{
  const string GUID = "esp";
  const string NAME = "ESP";
  const string VERSION = "1.28";
  private static ManualLogSource? Logs;
  public static ManualLogSource Log => Logs!;
  public void Awake()
  {
    Logs = Logger;
    Settings.Init(Config);
    MessageHud_UpdateMessage.GetMessage = Hud.GetMessage;
    new Harmony(GUID).PatchAll();
    Admin.Instance = new EspAdmin();
    Draw.Init();
  }
  public void Start()
  {
    if (Chainloader.PluginInfos.TryGetValue("org.bepinex.plugins.jewelcrafting", out var info))
      JewelcraftingPatcher.DoPatching(info.Instance.GetType().Assembly);
  }
  public void LateUpdate()
  {
    if (Player.m_localPlayer)
      Texts.UpdateAverageSpeed(Ship.GetLocalShip());
    Visualization.Visualization.SharedUpdate();
  }
}
