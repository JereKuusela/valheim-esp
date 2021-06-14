using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESP
{
  public class CharacterUtils
  {
    public static bool IsExcluded(Character instance)
    {
      var name = instance.name.ToLower();
      var m_name = instance.m_name.ToLower();
      var localized = Localization.instance.Localize(instance.m_name).ToLower();
      var excluded = Settings.excludedCreatures.ToLower().Split(',');
      return Array.Exists(excluded, item => item == name || item == m_name || item == localized);
    }

  }
  [HarmonyPatch(typeof(Character), "Awake")]
  public class Character_Awake
  {
    private static void DrawNoise(Character instance)
    {
      if (!Settings.showNoise || CharacterUtils.IsExcluded(instance))
        return;
      var obj = Drawer.DrawSphere(instance.gameObject, Patch.m_noiseRange(instance), Color.cyan, 0.1f);
      obj.AddComponent<NoiseText>().character = instance;
    }
    public static void Postfix(Character __instance)
    {
      DrawNoise(__instance);
    }
  }

  [HarmonyPatch(typeof(Character), "UpdateNoise")]
  public class Character_UpdateNoise : Component
  {
    public static void Postfix(Character __instance, float ___m_noiseRange)
    {
      if (!Settings.showNoise || CharacterUtils.IsExcluded(__instance))
        return;
      Drawer.UpdateSphere(__instance.gameObject, ___m_noiseRange, 0.1f);
    }
  }
  public class NoiseText : MonoBehaviour, Hoverable
  {
    public string GetHoverText() => GetHoverName() + "\n" + Texts.GetNoise(character);
    public string GetHoverName() => Format.Name(character);
    public Character character;
  }
  [HarmonyPatch(typeof(Character), "GetHoverText")]
  public class Character_GetHoverText
  {

    public static void Postfix(Character __instance, ref string __result)
    {
      if (!Settings.showExtraInfo) return;
      Hoverables.AddTexts(__instance.gameObject, ref __result);
    }
  }
}