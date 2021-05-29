using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(BaseAI), "Awake")]
  public class BaseAI_Awake
  {
    private static String GetNameText(Character character) => TextUtils.StringValue(character.m_name);
    private static void DrawHearRange(BaseAI instance, Character character)
    {
      if (!Settings.showBaseAI || CharacterUtils.IsExcluded(character))
        return;
      var range = instance.m_hearRange;
      if (range > 100) return;
      var text = GetNameText(character) + "\nHear range: " + TextUtils.IntValue(range);
      Drawer.DrawSphere(instance.gameObject, Vector3.zero, range, Color.green, 0.1f, text);
    }
    private static void DrawViewRange(BaseAI instance, Character character)
    {
      if (!Settings.showBaseAI || CharacterUtils.IsExcluded(character))
        return;
      var range = instance.m_viewRange;
      var angle = instance.m_viewAngle;
      var text = GetNameText(character) + "\nView range: " + TextUtils.IntValue(range) + "\nView angle: " + TextUtils.IntValue(angle);
      if (instance.m_hearRange > 100) text += "\n Hear range: " + TextUtils.StringValue("Infinite");
      Drawer.DrawConeY(instance.gameObject, character.m_eye.position - character.transform.position, range, angle, Color.white, 0.1f, text);
      Drawer.DrawConeX(instance.gameObject, character.m_eye.position - character.transform.position, range, angle, Color.white, 0.1f, text);
    }
    private static void DrawRay(Character instance)
    {
      var text = Localization.instance.Localize(instance.m_name);
      Drawer.DrawMarkerLine(instance.gameObject, Vector3.zero, Color.magenta, Settings.characterRayWidth, text);

    }
    public static void Postfix(BaseAI __instance, Character ___m_character)
    {
      DrawHearRange(__instance, ___m_character);
      DrawViewRange(__instance, ___m_character);
      DrawRay(___m_character);
    }
  }
}