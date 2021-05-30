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
      var text = name + "\nLimit range: " + TextUtils.IntValue(instance.m_totalCheckRange) + " meters";
      Drawer.DrawSphere(instance.gameObject, Vector3.zero, instance.m_totalCheckRange, Color.cyan, 0.1f, text);
    }
    private static void DrawPartnerCheck(Procreation instance, string name)
    {
      var text = name + "\nPartner range: " + TextUtils.IntValue(instance.m_partnerCheckRange) + " meters";
      Drawer.DrawSphere(instance.gameObject, Vector3.zero, instance.m_partnerCheckRange, Color.magenta, 0.1f, text);
    }
    public static void Postfix(Procreation __instance, Character ___m_character)
    {
      if (!Settings.showBreedingLimits || CharacterUtils.IsExcluded(___m_character))
        return;
      var name = CharacterUtils.GetNameText(___m_character);
      DrawTotalLimit(__instance, name);
      DrawPartnerCheck(__instance, name);
    }
  }
}