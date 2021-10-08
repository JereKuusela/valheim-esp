using HarmonyLib;
using Text;
using UnityEngine;
using Visualization;

namespace ESP {
  [HarmonyPatch(typeof(BaseAI), "Awake")]
  public class BaseAI_Awake {
    private static void DrawHearRange(BaseAI obj, string name, string text) {
      if (Settings.SenseLineWidth == 0 || !obj) return;
      if (obj.m_hearRange > 100) return;
      var line = Draw.DrawSphere(Tag.CreatureHearRange, obj, obj.m_hearRange, Settings.CreatureHearColor, Settings.SenseLineWidth);
      Draw.AddText(line, name, text);
    }
    private static void DrawViewRange(BaseAI obj, Vector3 eye, string name, string text) {
      if (Settings.SenseLineWidth == 0 || !obj) return;
      var line = Draw.DrawCone(Tag.CreatureViewRange, obj, eye, obj.m_viewRange, obj.m_viewAngle, Settings.CreatureViewColor, Settings.SenseLineWidth);
      Draw.AddText(line, name, text);
    }
    private static void DrawAlertRange(MonsterAI obj, Vector3 eye, string name, string text) {
      if (Settings.SenseLineWidth == 0 || !obj) return;
      var line = Draw.DrawArc(Tag.CreatureAlertRange, obj, eye, obj.m_alertRange, obj.m_viewAngle, Settings.CreatureAlertViewColor, Settings.SenseLineWidth);
      Draw.AddText(line, name, text);
    }
    private static void DrawFireLimit(BaseAI obj, string name, string text) {
      if (Settings.CreatureFireLineWidth == 0 || !obj) return;
      if (!obj.m_afraidOfFire && !obj.m_avoidFire) return;
      var line = Draw.DrawSphere(Tag.CreatureFireRange, obj, 3f, Settings.CreatureFireLimitColor, Settings.CreatureFireLineWidth);
      Draw.AddText(line, name, text);
    }
    private static void DrawTotalLimit(Procreation obj, string name, string text) {
      if (Settings.BreedingLineWidth == 0 || !obj) return;
      var line = Draw.DrawSphere(Tag.CreatureBreedingTotalRange, obj, obj.m_totalCheckRange, Settings.CreatureTotalLimitColor, Settings.BreedingLineWidth);
      Draw.AddText(line, name, text);
    }
    private static void DrawPartnerCheck(Procreation obj, string name, string text) {
      if (Settings.BreedingLineWidth == 0 || !obj) return;
      var line = Draw.DrawSphere(Tag.CreatureBreedingPartnerRange, obj, obj.m_partnerCheckRange, Settings.CreaturePartnerCheckColor, Settings.BreedingLineWidth);
      Draw.AddText(line, name, text);
    }
    private static void DrawFoodCheck(MonsterAI obj, string name, string text) {
      if (Settings.BreedingLineWidth == 0 || !obj || obj.m_consumeItems.Count == 0) return;
      var line = Draw.DrawSphere(Tag.CreatureFoodSearchRange, obj, obj.m_consumeSearchRange, Settings.CreatureFoodCheckColor, Settings.BreedingLineWidth);
      Draw.AddText(line, name, text);
    }
    private static void DrawEatRange(MonsterAI obj, string name, string text) {
      if (Settings.BreedingLineWidth == 0 || !obj || obj.m_consumeItems.Count == 0) return;
      var line = Draw.DrawSphere(Tag.CreatureEatingRange, obj, obj.m_consumeRange, Settings.CreatureEatRangeColor, Settings.BreedingLineWidth);
      Draw.AddText(line, name, text);
    }
    public static void Postfix(BaseAI __instance, Character ___m_character) {
      if (CharacterUtils.IsExcluded(___m_character)) return;
      var name = Format.Name(___m_character);
      var monsterAI = __instance.GetComponent<MonsterAI>();
      var procreation = __instance.GetComponent<Procreation>();
      var text = Texts.GetSenses(__instance, monsterAI, procreation);
      var eye = ___m_character.m_eye.localPosition;
      DrawHearRange(__instance, name, text);
      DrawViewRange(__instance, eye, name, text);
      DrawAlertRange(monsterAI, eye, name, text);
      DrawFireLimit(__instance, name, text);
      DrawTotalLimit(procreation, name, text);
      DrawPartnerCheck(procreation, name, text);
      DrawFoodCheck(monsterAI, name, text);
      DrawEatRange(monsterAI, name, text);
    }
  }
}