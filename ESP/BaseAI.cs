using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(BaseAI), "Awake")]
  public class BaseAI_Awake
  {
    private static String GetNameText(Character character) => TextUtils.String(character.m_name);
    private static void DrawHearRange(BaseAI instance, Character character)
    {
      if (!Settings.showBaseAI || CharacterUtils.IsExcluded(character))
        return;
      var range = instance.m_hearRange;
      if (range > 100) return;
      var text = GetNameText(character) + "\nHear range: " + TextUtils.Int(range);
      Drawer.DrawSphere(instance.gameObject, Vector3.zero, range, Color.green, 0.1f, text);
    }
    private static void DrawViewRange(BaseAI instance, Character character)
    {
      if (!Settings.showBaseAI || CharacterUtils.IsExcluded(character))
        return;
      var range = instance.m_viewRange;
      var angle = instance.m_viewAngle;
      var text = GetNameText(character) + "\nView range: " + TextUtils.Int(range) + "\nView angle: " + TextUtils.Int(angle);
      if (instance.m_hearRange > 100) text += "\n Hear range: " + TextUtils.String("Infinite");
      Drawer.DrawConeY(instance.gameObject, character.m_eye.position - character.transform.position, range, angle, Color.white, 0.1f, text);
      Drawer.DrawConeX(instance.gameObject, character.m_eye.position - character.transform.position, range, angle, Color.white, 0.1f, text);
    }
    private static void DrawRay(Character character)
    {
      if (!Settings.showCreatureRays || CharacterUtils.IsExcluded(character))
        return;
      var text = Localization.instance.Localize(character.m_name);
      Drawer.DrawMarkerLine(character.gameObject, Vector3.zero, Color.magenta, Settings.characterRayWidth, text);
    }
    private static void DrawFireLimit(BaseAI instance, Character character)
    {
      if (!Settings.showCreatureFireLimits || CharacterUtils.IsExcluded(character))
        return;
      if (!instance.m_afraidOfFire && !instance.m_avoidFire) return;
      var text = instance.m_afraidOfFire ? "Fears fire" : "Avoids fire";
      Drawer.DrawSphere(instance.gameObject, Vector3.zero, 3f, Color.magenta, 0.1f, text);
    }
    public static void Postfix(BaseAI __instance, Character ___m_character)
    {
      DrawHearRange(__instance, ___m_character);
      DrawViewRange(__instance, ___m_character);
      DrawRay(___m_character);
      DrawFireLimit(__instance, ___m_character);
    }
  }
}