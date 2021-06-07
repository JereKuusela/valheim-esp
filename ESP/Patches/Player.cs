using HarmonyLib;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch(typeof(Player), "UseStamina")]
  public class Player_UseStamina
  {
    public static void Prefix(Player __instance, float v)
    {
      if (__instance == Player.m_localPlayer)
        DPSMeter.AddStamina(v);
    }
  }
  [HarmonyPatch(typeof(Player), "Update")]
  public class Player_Update
  {
    public static void Postfix(Player __instance)
    {
      if (Patch.Player_TakeInput(__instance))
      {
        if (Player.m_debugMode && global::Console.instance.IsCheatsEnabled())
        {
          if (Input.GetKeyDown(KeyCode.J))
          {
            Settings.configShowDPS.Value = !Settings.configShowDPS.Value;
            if (!Settings.showDPS) DPSMeter.Reset();
          }
          if (Input.GetKeyDown(KeyCode.H))
          {
            Drawer.ToggleVisibility();
          }
        }
      }
    }
  }
}