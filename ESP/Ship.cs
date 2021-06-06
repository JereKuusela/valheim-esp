using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public static class ShipUtils
  {
    public static string text = "";
  }
  [HarmonyPatch(typeof(Ship), "FixedUpdate")]
  public class Ship_FixedUpdate
  {

    private static float GetRelativeAngle(Ship ship)
    {
      var forward = ship.transform.forward * 1f;
      forward.y = 0;
      var forwardAngle = 90f - Mathf.Atan2(ship.transform.forward.z, ship.transform.forward.x) / Math.PI * 180f;
      var windDir = EnvMan.instance.GetWindDir() * 1f;
      windDir.y = 0;
      return Vector3.Angle(forward, windDir);
    }
    private static double GetWindPower(Ship ship)
    {
      var windIntensity = EnvMan.instance.GetWindIntensity();
      var windPower = Mathf.Lerp(0.25f, 1f, windIntensity);
      return windPower * ship.GetWindAngleFactor();
    }
    private static string GetShipText(Ship ship, Rigidbody body)
    {
      var text = "";
      var forwardSpeed = ship.GetSpeed();
      var forwardAngle = 90f - Mathf.Atan2(ship.transform.forward.z, ship.transform.forward.x) / Math.PI * 180f;
      if (forwardSpeed < 0)
        text += "\nSpeed: " + TextUtils.Fixed(-forwardSpeed) + " m/s away from " + TextUtils.Int(forwardAngle) + " degrees (backward)";
      else
        text += "\nSpeed: " + TextUtils.Fixed(forwardSpeed) + " m/s towards " + TextUtils.Int(forwardAngle) + " degrees (forward)";
      Vector3 velocity = body.velocity * 1f;
      velocity.y = 0f;
      var angle = 90f - Mathf.Atan2(velocity.z, velocity.x) / Math.PI * 180f;
      var speed = velocity.magnitude;
      text += "\nSpeed: " + TextUtils.Fixed(speed) + " m/s towards " + TextUtils.Int(angle) + " degrees";
      text += "\n" + EnvUtils.GetWind();
      text += "\nWind power: " + TextUtils.Percent(GetWindPower(ship)) + " from " + TextUtils.Int(GetRelativeAngle(ship)) + " degrees";
      //text += "\nSail force: " + TextUtils.Percent(ship.m_sailForceFactor);
      //text += "\nRudderSpeed: " + TextUtils.Float(ship.m_rudderSpeed) + " m/s";
      return text;
    }
    public static void Postfix(Ship __instance, Rigidbody ___m_body)
    {
      if (!Settings.showShipStats && !Settings.showShipStatsOnHud) return;
      if (!__instance || !___m_body || !Player.m_localPlayer) return;
      if (!__instance.IsPlayerInBoat(Player.m_localPlayer.GetZDOID())) return;
      ShipUtils.text = GetShipText(__instance, ___m_body);
      if (Settings.showShipStatsOnHud)
        MessageHud_UpdateMessage.CustomMessage = "\n" + ShipUtils.text;
    }
  }
  [HarmonyPatch(typeof(Ship), "OnTriggerExit")]
  public class Ship_OnTriggerExit
  {
    public static void Postfix()
    {
      ShipUtils.text = "";
      MessageHud_UpdateMessage.CustomMessage = "";
    }
  }
}