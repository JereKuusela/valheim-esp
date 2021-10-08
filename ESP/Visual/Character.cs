using HarmonyLib;
using Text;
using UnityEngine;
using Visualization;

namespace ESP {
  public partial class Visual {

  }
  public class CharacterUtils {
    public static bool IsExcluded(Character instance) {
      var name = instance.name;
      var m_name = instance.m_name;
      var localized = Localization.instance.Localize(instance.m_name);
      var excluded = Settings.ExcludedCreatures;
      return LocationUtils.IsIn(excluded, name) || LocationUtils.IsIn(excluded, m_name) || LocationUtils.IsIn(excluded, localized);
    }
    public static bool IsTracked(Character instance) {
      var name = instance.name;
      var m_name = instance.m_name;
      var localized = Localization.instance.Localize(instance.m_name);
      var tracked = Settings.TrackedObjects;
      return LocationUtils.IsIn(tracked, name) || LocationUtils.IsIn(tracked, m_name) || LocationUtils.IsIn(tracked, localized);
    }

  }
  [HarmonyPatch(typeof(Character), "Awake")]
  public class Character_Awake {
    private static void DrawNoise(Character instance) {
      if (Settings.NoiseLineWidth == 0 || CharacterUtils.IsExcluded(instance))
        return;
      var obj = Draw.DrawSphere(Tag.CreatureNoise, instance, Patch.NoiseRange(instance), Settings.NoiseColor, Settings.NoiseLineWidth);
      obj.AddComponent<NoiseText>().character = instance;
    }
    public static void Postfix(Character __instance) {
      DrawNoise(__instance);
    }
  }

  [HarmonyPatch(typeof(Character), "UpdateNoise")]
  public class Character_UpdateNoise : Component {
    public static void Postfix(Character __instance, float ___m_noiseRange) {
      if (Settings.NoiseLineWidth == 0 || CharacterUtils.IsExcluded(__instance))
        return;
      Draw.UpdateSphere(__instance, ___m_noiseRange, Settings.NoiseLineWidth);
    }
  }
  public class NoiseText : MonoBehaviour, Hoverable {
    public string GetHoverText() => GetHoverName() + "\n" + Texts.GetNoise(character);
    public string GetHoverName() => Format.Name(character);
    public Character character;
  }
  [HarmonyPatch(typeof(Character), "GetHoverText")]
  public class Character_GetHoverText {

    public static void Postfix(Character __instance, ref string __result) {
      if (!Settings.ExtraInfo) return;
      Text.AddTexts(__instance.gameObject, ref __result);
    }
  }

  [HarmonyPatch(typeof(BaseAI), "FindPath")]
  public class BaseAI_Pathfinding {
    public static void Prefix(out float __state, float ___m_lastFindPathTime) {
      __state = ___m_lastFindPathTime;
    }
    public static void Postfix(BaseAI __instance, float __state, float ___m_lastFindPathTime, bool __result) {
      if (__state == ___m_lastFindPathTime) return;

    }
  }
}