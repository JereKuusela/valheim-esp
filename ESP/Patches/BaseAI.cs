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
      var obj = Drawer.DrawSphere(instance.gameObject, range, Color.green, 0.1f);
      Drawer.AddText(obj, text);
    }
    private static void DrawViewRange(BaseAI instance, Character character)
    {
      if (!Settings.showBaseAI || CharacterUtils.IsExcluded(character))
        return;
      var range = instance.m_viewRange;
      var angle = instance.m_viewAngle;
      var text = "\nView range: " + TextUtils.Int(range) + "\nView angle: " + TextUtils.Int(angle);
      if (instance.m_hearRange > 100) text += "\nHear range: " + TextUtils.String("Infinite");
      var obj = Drawer.DrawCone(instance.gameObject, character.m_eye.position - character.transform.position, range, angle, Color.white, 0.1f);
      Drawer.AddText(obj, GetNameText(character), text);
    }
    private static void DrawRay(Character character)
    {
      if (!Settings.showCreatureRays || CharacterUtils.IsExcluded(character))
        return;
      var text = Localization.instance.Localize(character.m_name);
      var obj = Drawer.DrawMarkerLine(character.gameObject, Vector3.zero, Color.magenta, Settings.characterRayWidth);
      Drawer.AddText(obj, text);
      Drawer.AddBoxCollider(obj);
    }
    private static void DrawFireLimit(BaseAI instance, Character character)
    {
      if (!Settings.showCreatureFireLimits || CharacterUtils.IsExcluded(character))
        return;
      if (!instance.m_afraidOfFire && !instance.m_avoidFire) return;
      var text = instance.m_afraidOfFire ? "Fears fire" : "Avoids fire";
      var obj = Drawer.DrawSphere(instance.gameObject, 3f, Color.magenta, 0.1f);
      Drawer.AddText(obj, text);
      Drawer.AddBoxCollider(obj);
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