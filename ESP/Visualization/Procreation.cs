using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(Procreation), "Awake")]
  public class Procreation_Awake
  {
    private static void DrawTotalLimit(Procreation instance, string name)
    {
      var text = "Limit range: " + TextUtils.Int(instance.m_totalCheckRange) + " meters";
      var obj = Drawer.DrawSphere(instance.gameObject, instance.m_totalCheckRange, Color.cyan, 0.1f);
      Drawer.AddText(obj, name, text);
    }
    private static void DrawPartnerCheck(Procreation instance, string name)
    {
      var text = "Partner range: " + TextUtils.Int(instance.m_partnerCheckRange) + " meters";
      var obj = Drawer.DrawSphere(instance.gameObject, instance.m_partnerCheckRange, Color.magenta, 0.1f);
      Drawer.AddText(obj, name, text);
    }
    public static void Postfix(Procreation __instance, Character ___m_character)
    {
      if (!Settings.showBreedingLimits || CharacterUtils.IsExcluded(___m_character))
        return;
      var name = TextUtils.Name(___m_character.gameObject);
      DrawTotalLimit(__instance, name);
      DrawPartnerCheck(__instance, name);
    }
  }
}