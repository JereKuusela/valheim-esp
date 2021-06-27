using HarmonyLib;
using System.Linq;

namespace ESP
{
  public static class SupportUtils
  {
    public static bool Shown = Drawer.showOthers;
    public static void SetVisibility(bool shown)
    {
      if (!Settings.showSupport) return;
      Shown = shown;
      WearNTear.GetAllInstaces().Where(item => item.m_supports).ToList().ForEach(item =>
      {
        if (shown)
          item.Highlight();
        else
          Patch.WearNTear_ResetHighlight(item);
      });
    }
    public static bool Enabled(WearNTear obj)
    {
      var piece = obj.GetComponent<Piece>();
      return Settings.showSupport && obj.m_supports && (!piece || !piece.m_waterPiece);
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
      if (!SupportUtils.Shown || !SupportUtils.Enabled(__instance)) return;
      __instance.CancelInvoke("ResetHighlight");
    }
  }

}