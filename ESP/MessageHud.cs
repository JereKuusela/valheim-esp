using HarmonyLib;
using UnityEngine;
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
      var lines = new List<string>();
      // Wait for the game to load.
      if (Player.m_localPlayer == null) return lines;
      lines.AddRange(GetInfo());
      var dps = DPSMeter.Get();
      if (dps != null)
      {
        lines.Add("");
        lines.AddRange(dps);
      }
      var eps = ExperienceMeter.Get();
      if (eps != null)
      {
        lines.Add("");
        lines.AddRange(eps);
      }
      if (PlayerShip != null)
      {
        lines.Add("");
        lines.AddRange(Texts.Get(PlayerShip).Split('\n').Where(line => line != ""));
      }
      return lines;
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
      return EnvUtils.GetLocation(location) + ", " + EnvUtils.GetAltitude(location) + ", " + EnvUtils.GetForest(location);
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

    private static string baseGameMessage = "";
    public static Ship PlayerShip = null;

    // Use state to track when a default in game message arrives.
    public static void Prefix(out string __state)
    {
      __state = MessageHud.instance.m_messageText.text;
    }
    // Keeps the message always visible and shows any base game messages.
    public static void Postfix(float ___m_msgQueueTimer, string __state)
    {
      if (!Settings.showHud) return;
      var hud = MessageHud.instance;
      var lines = GetFixedMessage();
      var isCustomMessage = lines.Count > 0;
      // New base game message.
      if (hud.m_messageText.text != __state)
        baseGameMessage = hud.m_messageText.text;
      if (baseGameMessage != "")
      {
        // No more base game messages.
        if (___m_msgQueueTimer >= 4f)
          baseGameMessage = "";
      }
      if (baseGameMessage != "")
      {
        lines.Add("");
        lines.Add(baseGameMessage);
      }
      if (lines.Count == 0) return;
      var padding = lines.Count - 2;
      for (var i = 0; i < padding; i++) lines.Insert(0, "");
      hud.m_messageText.CrossFadeAlpha(1f, 0f, true);
      hud.m_messageText.text = string.Join("\n", lines);
      // Icon is not very relevant information and will pop up over the text.
      if (isCustomMessage)
        hud.m_messageIcon.canvasRenderer.SetAlpha(0f);
    }
  }

  // No easy way to find the player's ship so must be done indirectly.
  [HarmonyPatch(typeof(Ship), "FixedUpdate")]
  public class Ship_FixedUpdate_Hud
  {
    public static void Postfix(Ship __instance)
    {
      if (!Settings.showShipStatsOnHud || !Player.m_localPlayer) return;
      if (!__instance.IsPlayerInBoat(Player.m_localPlayer.GetZDOID())) return;
      MessageHud_UpdateMessage.PlayerShip = __instance;
    }
  }
  [HarmonyPatch(typeof(Ship), "OnTriggerExit")]
  public class Ship_OnTriggerExit_Hud
  {
    public static void Postfix()
    {
      MessageHud_UpdateMessage.PlayerShip = null;
    }
  }
}