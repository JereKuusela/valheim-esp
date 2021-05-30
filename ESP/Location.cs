using HarmonyLib;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch(typeof(Location), "Awake")]
  public class Location_Awake
  {
    public static void Postfix(Location __instance)
    {
      if (!Settings.showLocations)
        return;
      var text = TextUtils.String(__instance.name);
      Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, Color.black, Settings.locationRayWidth, text);
    }
  }
}