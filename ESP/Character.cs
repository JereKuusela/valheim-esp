using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public class CharacterUtils
  {
    public static String GetNameText(float range) => "Noise: " + TextUtils.IntValue(range);
    public static String GetNameText(Character character) => TextUtils.StringValue(character.m_name);

  }
  [HarmonyPatch(typeof(Character), "Awake")]
  public class Character_Awake
  {
    public static void Postfix(Character __instance, float ___m_noiseRange)
    {
      if (!Settings.showNoise)
        return;
      var text = CharacterUtils.GetNameText(__instance) + "\n" + CharacterUtils.GetNameText(___m_noiseRange);
      Drawer.DrawSphere(__instance.gameObject, __instance.transform.position, ___m_noiseRange, Color.cyan, 0.1f, text);
    }
  }

  [HarmonyPatch(typeof(Character), "UpdateNoise")]
  public class Character_UpdateNoise : Component
  {
    public static void Postfix(Character __instance, float ___m_noiseRange)
    {
      if (!Settings.showNoise)
        return;
      var text = CharacterUtils.GetNameText(__instance) + "\n" + CharacterUtils.GetNameText(___m_noiseRange);
      Drawer.UpdateSphere(__instance.gameObject, Vector3.zero, ___m_noiseRange, text);
    }
  }
}