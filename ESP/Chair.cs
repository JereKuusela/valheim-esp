using HarmonyLib;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch(typeof(Chair), "GetHoverText")]
  public class Chair_GetHoverText
  {
    private static string GetShipHoverText(Ship ship)
    {
      if (!Settings.showShipStats) return "";
      if (!ship) return "";
      if (!ship.IsPlayerInBoat(Player.m_localPlayer.GetZDOID())) return "";
      return ShipUtils.text;
    }
    public static void Postfix(Chair __instance, ref string __result)
    {
      var ship = __instance.gameObject.GetComponentInParent<Ship>();
      __result += GetShipHoverText(ship);
    }
  }
}