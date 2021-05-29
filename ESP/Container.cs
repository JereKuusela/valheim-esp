using HarmonyLib;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch(typeof(Container), "Awake")]
  public class Container_Awake
  {
    public static void Postfix(Container __instance, Piece ___m_piece)
    {
      if (!Settings.showChests || ___m_piece.IsPlacedByPlayer())
        return;
      var text = TextUtils.StringValue(__instance.GetHoverName());
      Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, Color.white, 0.5f, text);
    }
  }
}