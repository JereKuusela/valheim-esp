using HarmonyLib;
using UnityEngine;

namespace ESP
{

  [HarmonyPatch(typeof(MessageHud), "UpdateMessage")]
  public class MessageHud_UpdateMessage : MonoBehaviour
  {
    private static string GetShowHide(bool value) => value ? "Hide" : "Show";
    private static string GetInfo()
    {
      if (!Settings.showHud) return "";
      // Wait for the game to load.
      if (Player.m_localPlayer == null) return "";
      var status = "\n" + GetStatusText();
      status += "\n";
      status += Format.String("Y") + ": " + GetShowHide(Drawer.showZones) + " zones, ";
      status += Format.String("U") + ": " + GetShowHide(Drawer.showCreatures) + " creatures, ";
      status += Format.String("I") + ": " + GetShowHide(Drawer.showOthers) + " other visuals";
      status += "\n";
      status += Format.String("O") + ": " + GetShowHide(Hoverables.extraInfo) + " extra info on tooltips, ";
      status += Format.String("P") + ": " + GetShowHide(Settings.showDPS) + " DPS meter";
      return status;
    }
    private static string GetFixedMessage()
    {
      // Wait for the game to load.
      if (Player.m_localPlayer == null) return "";
      var dps = DPSMeter.Get();
      if (dps != "") return dps;
      if (FixedMessage != "") return FixedMessage;
      return "";
    }
    private static string GetSpeed() => "Speed: " + Format.Float(Patch.m_currentVel(Player.m_localPlayer).magnitude, "0.#") + " m/s";
    private static string GetNoise() => "Noise: " + Format.Int(Player.m_localPlayer.GetNoiseRange()) + " meters";
    private static string GetStatusText()
    {
      if (!Settings.showTimeAndWeather) return "";

      var time = EnvUtils.GetTime() + ", " + EnvUtils.GetCurrentEnvironment();
      return time + "\n" + GetSpeed() + "\n" + GetNoise();
    }

    private static bool baseGameMessage = false;
    public static string FixedMessage = "";

    // Space is limited so skip showing messages when manually showing something.
    public static bool Prefix(out string __state)
    {
      __state = MessageHud.instance.m_messageText.text;
      return baseGameMessage || GetFixedMessage() == "";
    }
    // Keeps the message always visible and shows any base game messages.
    public static void Postfix(float ___m_msgQueueTimer, string __state)
    {
      var hud = MessageHud.instance;
      var customMessage = GetFixedMessage();
      if (customMessage == "")
        customMessage = GetInfo();
      // New base game message.
      if (hud.m_messageText.text != __state)
        baseGameMessage = true;
      if (baseGameMessage)
      {
        // No more base game messages.
        if (___m_msgQueueTimer >= 4f)
          baseGameMessage = false;
        if (customMessage != "" && ___m_msgQueueTimer >= 1f)
          baseGameMessage = false;
      }
      else if (customMessage != "")
      {
        hud.m_messageText.CrossFadeAlpha(1f, 0f, true);
        hud.m_messageText.text = "\n" + customMessage;

      }
    }
  }

  [HarmonyPatch(typeof(Ship), "FixedUpdate")]
  public class Ship_FixedUpdate_Hud
  {
    public static void Postfix(Ship __instance)
    {
      if (!Settings.showShipStatsOnHud || !Player.m_localPlayer) return;
      if (!__instance.IsPlayerInBoat(Player.m_localPlayer.GetZDOID())) return;
      MessageHud_UpdateMessage.FixedMessage = "\n" + Texts.Get(__instance);
    }
  }
  [HarmonyPatch(typeof(Ship), "OnTriggerExit")]
  public class Ship_OnTriggerExit_Hud
  {
    public static void Postfix()
    {
      MessageHud_UpdateMessage.FixedMessage = "";
    }
  }
}