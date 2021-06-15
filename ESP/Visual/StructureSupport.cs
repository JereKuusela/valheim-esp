using HarmonyLib;
using System.Linq;

namespace ESP
{
  public static class SupportUtils
  {
    public static bool Shown = Settings.showOthers;
    public static void SetVisibility(bool shown)
    {
      if (!Settings.showSupport) return;
      Shown = shown;
      // Automatically calls reset after a delay.
      WearNTear.GetAllInstaces().Where(item => item.m_supports).ToList().ForEach(item => item.Highlight());
    }
    public static bool Enabled(WearNTear obj)
    {
      var piece = obj.GetComponent<Piece>();
      return SupportUtils.Shown && Settings.showSupport && obj.m_supports && (!piece || !piece.m_waterPiece);
    }
  }

  [HarmonyPatch(typeof(WearNTear), "Awake")]
  public class WearNTear_Awake
  {
    public static void Postfix(WearNTear __instance)
    {
      if (!SupportUtils.Enabled(__instance)) return;
      __instance.Highlight();
    }
  }
  [HarmonyPatch(typeof(WearNTear), "Highlight")]
  public class WearNTear_Highlight
  {
    public static void Postfix(WearNTear __instance)
    {
      if (!SupportUtils.Enabled(__instance)) return;
      __instance.CancelInvoke("ResetHighlight");
    }
  }

}