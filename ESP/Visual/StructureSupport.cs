using HarmonyLib;

namespace ESP
{
  public static class SupportUtils
  {
    public static bool Shown = Settings.showVisualization;
    public static void ToggleVisibility()
    {
      if (!Settings.showSupport) return;
      Shown = !Shown;
      // Automatically calls reset after a delay.
      WearNTear.GetAllInstaces().ForEach(item => item.Highlight());
    }
  }

  [HarmonyPatch(typeof(WearNTear), "Awake")]
  public class WearNTear_Awake
  {
    public static void Postfix(WearNTear __instance)
    {
      if (!SupportUtils.Shown || !Settings.showSupport)
        return;
      __instance.Highlight();
    }
  }
  [HarmonyPatch(typeof(WearNTear), "Highlight")]
  public class WearNTear_Highlight
  {
    public static void Postfix(WearNTear __instance)
    {
      if (!SupportUtils.Shown || !Settings.showSupport)
        return;
      __instance.CancelInvoke("ResetHighlight");
    }
  }

}