using BepInEx;
using HarmonyLib;

namespace ESP
{

  [BepInPlugin("valheim.jerekuusela.esp", "ESP", "0.1.0.0")]
  public class ESP : BaseUnityPlugin
  {
    void Awake()
    {
      Settings.Init(Config);
      var harmony = new Harmony("valheim.jerekuusela.esp");
      harmony.PatchAll();
    }
  }
}
