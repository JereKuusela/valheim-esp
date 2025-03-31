using HarmonyLib;
using Service;
using UnityEngine;
using Visualization;
namespace ESP;
public class CharacterUtils
{
  public static bool IsExcluded(Character instance)
  {
    if (!instance) return true;
    var name = instance.name;
    var m_name = instance.m_name;
    var localized = Localization.instance.Localize(instance.m_name);
    var excluded = Settings.ExcludedCreatures;
    return LocationUtils.IsIn(excluded, name) || LocationUtils.IsIn(excluded, m_name) || LocationUtils.IsIn(excluded, localized);
  }
  public static bool IsTracked(ZNetView instance)
  {
    if (!instance) return false;
    var name = instance.name;
    var m_name = name;
    if (instance.TryGetComponent(out Character character))
      m_name = character.m_name;
    var localized = Localization.instance.Localize(m_name);
    var tracked = Settings.TrackedObjects;
    return LocationUtils.IsIn(tracked, name) || LocationUtils.IsIn(tracked, m_name) || LocationUtils.IsIn(tracked, localized);
  }

}
[HarmonyPatch(typeof(Character), nameof(Character.Awake)), HarmonyPriority(Priority.Last)]
public class Character_Awake
{
  private static void DrawNoise(Character instance)
  {
    if (Settings.IsDisabled(Tag.CreatureNoise) || CharacterUtils.IsExcluded(instance))
      return;
    var obj = Draw.DrawSphere(Tag.CreatureNoise, instance, instance.m_noiseRange);
    obj.AddComponent<NoiseText>().character = instance;
  }
  static void Postfix(Character __instance)
  {
    DrawNoise(__instance);
  }
}

[HarmonyPatch(typeof(Character), nameof(Character.UpdateNoise))]
public class Character_UpdateNoise : Component
{
  static void Postfix(Character __instance)
  {
    if (Settings.IsHidden(Tag.CreatureNoise) || CharacterUtils.IsExcluded(__instance))
      return;
    Draw.UpdateSphere(__instance, __instance.m_noiseRange);
  }
}
public class NoiseText : MonoBehaviour, Hoverable
{
  public string GetHoverText() => character == null ? "" : GetHoverName() + "\n" + Texts.GetNoise(character);
  public string GetHoverName() => character == null ? "" : Translate.Name(character);
  public Character? character;
}
[HarmonyPatch(typeof(Character), nameof(Character.GetHoverText))]
public class Character_GetHoverText
{
  static void Postfix(Character __instance, ref string __result)
  {
    if (!Settings.ExtraInfo) return;
    Text.AddTexts(__instance.gameObject, ref __result);
  }
}
