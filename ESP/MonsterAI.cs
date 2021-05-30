using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(MonsterAI), "Awake")]
  public class MonsterAI_Awake
  {
    private static String GetNameText(Character character) => TextUtils.String(character.m_name);
    private static void DrawAlertRange(MonsterAI instance, Character character)
    {
      var range = instance.m_alertRange;
      var angle = instance.m_viewAngle;
      var text = GetNameText(character) + "\nAlert range: " + TextUtils.Int(range) + "\nAlert angle: " + TextUtils.Int(angle);
      Drawer.DrawArcY(instance.gameObject, character.m_eye.position - character.transform.position, range, angle, Color.red, 0.1f, text);
      Drawer.DrawArcX(instance.gameObject, character.m_eye.position - character.transform.position, range, angle, Color.red, 0.1f, text);
    }
    public static void Postfix(MonsterAI __instance, Character ___m_character)
    {
      if (!Settings.showBaseAI)
        return;
      DrawAlertRange(__instance, ___m_character);
    }
  }
}