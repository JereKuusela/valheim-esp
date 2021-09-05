using BepInEx;
using HarmonyLib;

namespace ESP
{
  [BepInPlugin("valheim.jerekuusela.esp", "ESP", "1.5.0.0")]
  public class ESP : BaseUnityPlugin
  {
    void Awake()
    {
      Settings.Init(Config);
      var harmony = new Harmony("valheim.jerekuusela.esp");
      harmony.PatchAll();
    }

    void Update()
    {
      if (Player.m_localPlayer)
        Texts.UpdateAverageSpeed(Ship.GetLocalShip());
    }
  }
}
