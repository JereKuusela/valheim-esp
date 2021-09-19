using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace ESP {
  public partial class Settings {
    public static void Init(ConfigFile config) {
      InitDev(config);
      InitHUD(config);
      InitTooltips(config);
      InitVisuals(config);
      InitExcluded(config);
      InitColors(config);
    }
  }

  [HarmonyPatch(typeof(Player), "Update")]
  public class Player_Update {
    public static void Postfix(Player __instance) {
      if (Patch.Player_TakeInput(__instance)) {
        if (Input.GetKeyDown(KeyCode.Y)) {
          Drawer.ToggleZoneVisibility();
        }
        if (Input.GetKeyDown(KeyCode.U)) {
          Drawer.ToggleCreatureVisibility();
        }
        if (Input.GetKeyDown(KeyCode.I)) {
          Drawer.ToggleOtherVisibility();
          SupportUtils.SetVisibility(Drawer.showOthers);
        }
        if (Input.GetKeyDown(KeyCode.O)) {
          Hoverables.extraInfo = !Hoverables.extraInfo;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
          Settings.configShowDPS.Value = !Settings.configShowDPS.Value;
          if (!Settings.ShowDPS) DPSMeter.Reset();
        }
        if (Input.GetKeyDown(KeyCode.L)) {
          Settings.configShowExperienceMeter.Value = !Settings.configShowExperienceMeter.Value;
          if (!Settings.ShowExperienceMeter) ExperienceMeter.Reset();
        }
        if (Input.GetKeyDown(KeyCode.J)) {
          Ruler.Toggle(Player.m_localPlayer.transform.position);
        }
      }
    }
  }
}
