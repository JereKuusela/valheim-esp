using HarmonyLib;
using UnityEngine;

namespace ESP
{

  [HarmonyPatch(typeof(Player), "Update")]
  public class Player_Update
  {
    public static void Postfix(Player __instance)
    {
      if (Patch.Player_TakeInput(__instance))
      {
        if (Player.m_debugMode && global::Console.instance.IsCheatsEnabled())
        {
          if (Input.GetKeyDown(KeyCode.I))
          {
            Settings.configShowExtraInfo.Value = !Settings.configShowExtraInfo.Value;
          }
          if (Input.GetKeyDown(KeyCode.O))
          {
            Drawer.ToggleVisibility();
          }
          if (Input.GetKeyDown(KeyCode.P))
          {
            Settings.configShowDPS.Value = !Settings.configShowDPS.Value;
            if (!Settings.showDPS) DPSMeter.Reset();
          }
        }
      }
    }
  }
}