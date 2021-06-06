using HarmonyLib;
using System;
using UnityEngine;

namespace ESP
{

  [HarmonyPatch(typeof(MessageHud), "UpdateMessage")]
  public class MessageHud_UpdateMessage : MonoBehaviour
  {
    private static string GetCustomMessage()
    {
      var dps = DPSMeter.GetText();
      if (dps != "") return dps;
      if (CustomMessage != "") return CustomMessage;
      return "";
    }
    private static string GetStatusText()
    {
      // Local player exists when the game is loaded (during loading shows arbitrary time).
      if (!Settings.showTimeAndWeather || Player.m_localPlayer == null) return "";
      return EnvUtils.GetTime() + ", " + EnvUtils.GetCurrentEnvironment();
    }

    private static bool baseGameMessage = false;
    public static string CustomMessage = "";

    // Space is limited so skip showing messages when manually showing something.
    public static bool Prefix(out string __state)
    {
      __state = MessageHud.instance.m_messageText.text;
      return GetCustomMessage() == "";
    }
    // Keeps the message always visible and shows any base game messages.
    public static void Postfix(float ___m_msgQueueTimer, string __state)
    {
      var hud = MessageHud.instance;
      var customMessage = GetCustomMessage();
      if (customMessage == "")
        customMessage = GetStatusText();
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
}