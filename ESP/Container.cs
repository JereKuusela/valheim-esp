using HarmonyLib;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch(typeof(Container), "Awake")]
  public class Container_Awake
  {
    public static void Postfix(Container __instance, Piece ___m_piece)
    {
      if (!Settings.showChests || !___m_piece || ___m_piece.IsPlacedByPlayer())
        return;
      var text = TextUtils.String(__instance.GetHoverName());
      Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, Color.white, Settings.chestRayWidth, text);
    }
  }
  [HarmonyPatch(typeof(Container), "GetHoverText")]
  public class Container_GetHoverText
  {
    public static void Postfix(Container __instance, ref string __result)
    {
      var wearNTear = __instance.GetComponent<WearNTear>();
      __result += WearNTearUtils.GetText(wearNTear);
    }
  }
}