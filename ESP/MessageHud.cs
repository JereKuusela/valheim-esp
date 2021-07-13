using HarmonyLib;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ESP
{

  [HarmonyPatch(typeof(MessageHud), "UpdateMessage")]
  public class MessageHud_UpdateMessage : MonoBehaviour
  {
    private static string GetShowHide(bool value) => value ? "Hide" : "Show";
    private static List<string> GetInfo()
    {
      if (!Settings.showHud) return null;
      // Wait for the game to load.
      if (Player.m_localPlayer == null) return null;
      var lines = new List<string>();
      lines.Add(GetEnvironment());
      lines.Add(GetLocation(Player.m_localPlayer.transform.position));
      lines.Add(GetSpeed() + ", " + GetNoise());
      lines.Add(GetTrackedCreatures());
      lines.Add(GetVisualSettings());
      lines.Add(GetOtherSettings());
      return lines.Where(item => item != "").ToList();
    }
    private static List<string> GetFixedMessage()
    {
      // Wait for the game to load.
      if (Player.m_localPlayer == null) return null;
      var dps = DPSMeter.Get();
      if (dps != null) return dps;
      var eps = ExperienceMeter.Get();
      if (eps != null) return eps;
      if (FixedMessage != null) return new List<string>(FixedMessage);
      return null;
    }
    private static string GetVisualSettings()
    {
      var text = Format.String("Y") + ": " + GetShowHide(Drawer.showZones) + " zones, ";
      text += Format.String("U") + ": " + GetShowHide(Drawer.showCreatures) + " creatures, ";
      text += Format.String("I") + ": " + GetShowHide(Drawer.showOthers) + " other";
      return text;
    }
    private static string GetOtherSettings()
    {
      var text = Format.String("O") + ": " + GetShowHide(Hoverables.extraInfo) + " extra info, ";
      text += Format.String("P") + ": " + GetShowHide(Settings.showDPS) + " DPS, ";
      text += Format.String("L") + ": " + GetShowHide(Settings.showExperienceMeter) + " experience";
      return text;
    }
    private static string GetSpeed() => "Speed: " + Format.Float(Patch.m_currentVel(Player.m_localPlayer).magnitude, "0.#") + " m/s";
    private static string GetNoise() => "Noise: " + Format.Int(Player.m_localPlayer.GetNoiseRange()) + " meters";
    private static string GetEnvironment()
    {
      if (!Settings.showTimeAndWeather) return "";
      return EnvUtils.GetTime() + ", " + EnvUtils.GetCurrentEnvironment() + " (" + EnvUtils.GetWindHud() + ")";
    }
    private static string GetLocation(Vector3 location)
    {
      return EnvUtils.GetLocation(location) + ", " + EnvUtils.GetForest(location);
    }
    public static string GetTrackedCreatures()
    {
      if (Settings.trackedCreatures == "") return "";
      var tracked = Settings.trackedCreatures.Split(',');
      var tracks = tracked.Select(name =>
      {
        var prefab = ZNetScene.instance.GetPrefab(name);
        var counts = prefab == null ? -1 : SpawnSystem.GetNrOfInstances(prefab);
        return name + ": " + Format.Int(counts);
      });

      return string.Join(",", tracks);
    }

    private static bool baseGameMessage = false;
    public static List<string> FixedMessage = null;

    // Space is limited so skip showing messages when manually showing something.
    public static bool Prefix(out string __state)
    {
      __state = MessageHud.instance.m_messageText.text;
      return baseGameMessage || (GetFixedMessage() == null);
    }
    // Keeps the message always visible and shows any base game messages.
    public static void Postfix(float ___m_msgQueueTimer, string __state)
    {
      var hud = MessageHud.instance;
      var lines = GetFixedMessage();
      if (lines == null)
        lines = GetInfo();
      var customMessage = "";
      if (lines != null)
      {
        var padding = lines.Count - 2;
        for (var i = 0; i < padding; i++) lines.Insert(0, "");
        customMessage = string.Join("\n", lines);
      }
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
        hud.m_messageText.text = customMessage;
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
      var lines = Texts.Get(__instance).Split('\n').Where(line => line != "").ToList();
      lines.Insert(0, MessageHud_UpdateMessage.GetTrackedCreatures());
      MessageHud_UpdateMessage.FixedMessage = lines;
    }
  }
  [HarmonyPatch(typeof(Ship), "OnTriggerExit")]
  public class Ship_OnTriggerExit_Hud
  {
    public static void Postfix()
    {
      MessageHud_UpdateMessage.FixedMessage = null;
    }
  }
}