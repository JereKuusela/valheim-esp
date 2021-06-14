using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public static class SupportUtils
  {
    public static bool Shown = Settings.showVisualization;
    public static void ToggleVisibility()
    {
      if (!Settings.showSupport) return;
      Shown = !Shown;
      foreach (var obj in Resources.FindObjectsOfTypeAll(typeof(WearNTear)) as WearNTear[])
        obj.Highlight(); // Automatically calls reset after a delay.
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