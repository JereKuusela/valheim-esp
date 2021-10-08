using System.Linq;
using HarmonyLib;
using Visualization;

namespace ESP {
  ///<summary>Utility code for support related things.</summary>
  public static class SupportUtils {
    ///<summary>Sets visibility of the support visual for all structures.</summary>
    public static void UpdateVisibility() {
      WearNTear.GetAllInstaces().Where(item => item.m_supports).ToList().ForEach(item => {
        var shown = VisualEnabled(item);
        if (shown)
          item.Highlight();
        else
          Patch.WearNTear_ResetHighlight(item);
      });
    }
    ///<summary>Returns whether support information is available for a given structure.</summary>
    public static bool Enabled(WearNTear obj) {
      if (!obj) return false;
      var piece = obj.GetComponent<Piece>();
      return obj.m_supports && (!piece || !piece.m_waterPiece);
    }
    ///<summary>Returns whether visual should be drawn for a given structure.</summary>
    public static bool VisualEnabled(WearNTear obj) => Visibility.IsTag(Tag.StructureSupport) && Enabled(obj);
    ///<summary>Updates visual of a given structure.</summary>
    public static void Update(WearNTear obj) {
      if (!VisualEnabled(obj)) return;
      obj.Highlight();
    }
  }

  ///<summary>Adds automatic highlight for structures.</summary>
  [HarmonyPatch(typeof(WearNTear), "Awake")]
  public class WearNTear_AutoHighlight {
    public static void Postfix(WearNTear __instance) {
      if (!SupportUtils.VisualEnabled(__instance)) return;
      __instance.Highlight();
    }
  }
  ///<summary>Hightlighting automatically calls reset which must be patched out.</summary>
  [HarmonyPatch(typeof(WearNTear), "Highlight")]
  public class WearNTear_Highlight {
    public static void Postfix(WearNTear __instance) {
      if (!SupportUtils.VisualEnabled(__instance)) return;
      __instance.CancelInvoke("ResetHighlight");
    }
  }
  ///<summary>Support updates gradually which requires updating the visual.</summary>
  [HarmonyPatch(typeof(WearNTear), "UpdateSupport")]
  public class WearNTear_Update {
    public static void Postfix(WearNTear __instance) => SupportUtils.Update(__instance);
  }
}