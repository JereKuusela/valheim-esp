using System;
using BepInEx;
using BepInEx.Bootstrap;
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
  const string VERSION = "1.29";
  public void Awake()
  {
    Log.Init(Logger);
    Settings.Init(Config);
    MessageHud_UpdateMessage.GetMessage = Hud.GetMessage;
    new Harmony(GUID).PatchAll();
    Admin.Instance = new EspAdmin();
    Draw.Init();
    try
    {
      FormatLoading.SetupWatcher();
      Watcher.SetupWatcher(Config);
    }
    catch (Exception e)
    {
      Log.Error(e.StackTrace);
    }
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
