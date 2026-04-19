using System;
using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;
using Service;
using Visualization;
namespace ESP;

[BepInPlugin(GUID, NAME, VERSION)]
public class ESP : BaseUnityPlugin
{
  const string GUID = "esp";
  const string NAME = "ESP";
  const string VERSION = "1.32";
  public void Awake()
  {
    Log.Init(Logger);
    Settings.Init(Config);
    MessageHud_UpdateMessage.GetMessage = Hud.GetMessage;
    new Harmony(GUID).PatchAll();
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
    if (Chainloader.PluginInfos.TryGetValue("server_devcommands", out info))
      PermissionManager.SetupSDC(info.Instance.GetType().Assembly);
  }
  public void LateUpdate()
  {
    if (Player.m_localPlayer)
      Texts.UpdateAverageSpeed(Ship.GetLocalShip());
    Visualization.Visualization.SharedUpdate();
  }
}

[HarmonyPatch(typeof(Player), nameof(Player.SetupPlacementGhost)), HarmonyPriority(Priority.Last)]
public class PlayerCleanGhost
{
  static void Postfix(Player __instance)
  {
    if (Settings.IsDisabled(Tag.EffectAreaPlayerBase)) return;
    var ghost = __instance.m_placementGhost;
    if (!ghost) return;
    var oldRuler = ghost.GetComponentInChildren<CircleRuler>(true);
    UnityEngine.Object.DestroyImmediate(oldRuler);
  }
}