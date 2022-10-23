using HarmonyLib;
using Service;
using UnityEngine;
using Visualization;
namespace ESP;
[HarmonyPatch(typeof(BaseAI), nameof(BaseAI.Awake)), HarmonyPriority(Priority.Last)]
public class BaseAI_Awake {
  private static void DrawHearRange(BaseAI obj, string name, string text) {
    if (Settings.IsDisabled(Tag.CreatureHearRange) | !obj) return;
    if (obj.m_hearRange > 100) return;
    var line = Draw.DrawSphere(Tag.CreatureHearRange, obj, obj.m_hearRange);
    Draw.AddText(line, name, text);
  }
  private static void DrawViewRange(BaseAI obj, Vector3 eye, string name, string text) {
    if (Settings.IsDisabled(Tag.CreatureViewRange) || !obj) return;
    var line = Draw.DrawCone(Tag.CreatureViewRange, obj, eye, obj.m_viewRange, obj.m_viewAngle);
    Draw.AddText(line, name, text);
  }
  private static void DrawAlertRange(MonsterAI obj, Vector3 eye, string name, string text) {
    if (Settings.IsDisabled(Tag.CreatureAlertRange) || !obj) return;
    var line = Draw.DrawArc(Tag.CreatureAlertRange, obj, eye, obj.m_alertRange, obj.m_viewAngle);
    Draw.AddText(line, name, text);
  }
  private static void DrawFireLimit(BaseAI obj, string name, string text) {
    if (Settings.IsDisabled(Tag.CreatureFireRange) || !obj) return;
    if (!obj.m_afraidOfFire && !obj.m_avoidFire) return;
    var line = Draw.DrawSphere(Tag.CreatureFireRange, obj, Constants.CreatureFireLimitRadius);
    Draw.AddText(line, name, text);
  }
  private static void DrawTotalLimit(Procreation obj, string name, string text) {
    if (Settings.IsDisabled(Tag.CreatureBreedingTotalRange) || !obj) return;
    var line = Draw.DrawSphere(Tag.CreatureBreedingTotalRange, obj, obj.m_totalCheckRange);
    Draw.AddText(line, name, text);
  }
  private static void DrawPartnerCheck(Procreation obj, string name, string text) {
    if (Settings.IsDisabled(Tag.CreatureBreedingPartnerRange) || !obj) return;
    var line = Draw.DrawSphere(Tag.CreatureBreedingPartnerRange, obj, obj.m_partnerCheckRange);
    Draw.AddText(line, name, text);
  }
  private static void DrawFoodCheck(MonsterAI obj, string name, string text) {
    if (Settings.IsDisabled(Tag.CreatureFoodSearchRange) || !obj || obj.m_consumeItems.Count == 0) return;
    var line = Draw.DrawSphere(Tag.CreatureFoodSearchRange, obj, obj.m_consumeSearchRange);
    Draw.AddText(line, name, text);
  }
  private static void DrawEatRange(MonsterAI obj, string name, string text) {
    if (Settings.IsDisabled(Tag.CreatureEatingRange) || !obj || obj.m_consumeItems.Count == 0) return;
    var line = Draw.DrawSphere(Tag.CreatureEatingRange, obj, obj.m_consumeRange);
    Draw.AddText(line, name, text);
  }
  static void Postfix(BaseAI __instance, Character ___m_character) {
    if (CharacterUtils.IsExcluded(___m_character)) return;
    var name = Translate.Name(___m_character);
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
