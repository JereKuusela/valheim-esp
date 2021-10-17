using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Service {
  // Prepends a custom message to the hud.
  [HarmonyPatch(typeof(MessageHud), "Update")]
  public class MessageHud_UpdateMessage : MonoBehaviour {
    public static Func<List<string>> GetMessage = () => new List<string>() { "CRAP" };
    public static void Postfix(MessageHud __instance) {
      // Wait for the game to load.
      if (Player.m_localPlayer == null) return;
      if (Hud.IsUserHidden()) return;
      var hud = __instance;
      var previousText = hud.m_messageText.text;
      var lines = GetMessage();
      if (lines.Count == 0) return;
      while (previousText.StartsWith(" \n"))
        previousText = previousText.Substring(2);
      var previousLines = previousText != "" ? previousText.Split('\n').Length : 0;
      var padding = previousLines + lines.Count - 2;
      for (var i = 0; i < padding; i++) lines.Insert(0, " ");
      if (previousText != "") {
        lines.Insert(0, " ");
        lines.Insert(0, " ");
        lines.Add(" ");
        lines.Add(previousText);
      }
      hud.m_messageText.text = Format.JoinLines(lines);
      hud.m_messageText.CrossFadeAlpha(1f, 0f, true);
      // Icon is not very relevant information and will pop up over the text.
      hud.m_messageIcon.canvasRenderer.SetAlpha(0f);
      hud.m_messageIcon.CrossFadeAlpha(0f, 0f, true);
    }
  }
  // Track message change to ensure a clean slate in every update.
  // This allows multiple mods to work with the hud.
  [HarmonyPatch(typeof(MessageHud), "UpdateMessage")]
  public class MessageHud_GetBaseMessage : MonoBehaviour {
    private static string BaseMessage = "";
    public static void Prefix(out string __state) {
      __state = MessageHud.instance.m_messageText.text;
    }
    public static void Postfix(MessageHud __instance, float ___m_msgQueueTimer, string __state) {
      if (__instance.m_messageText.text != __state)
        BaseMessage = __instance.m_messageText.text;
      // No more base game messages.
      if (___m_msgQueueTimer >= 4f) BaseMessage = "";
      __instance.m_messageText.text = BaseMessage;
    }
  }
}