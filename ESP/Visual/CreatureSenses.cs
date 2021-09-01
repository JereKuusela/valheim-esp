using HarmonyLib;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch(typeof(BaseAI), "Awake")]
  public class BaseAI_Awake
  {
    private static void DrawHearRange(BaseAI obj, string name, string text)
    {
      if (Settings.senseLineWidth == 0 || !obj) return;
      if (obj.m_hearRange > 100) return;
      var line = Drawer.DrawSphere(obj, obj.m_hearRange, Settings.creatureHearColor, Settings.senseLineWidth, Drawer.CREATURE);
      Drawer.AddText(line, name, text);
    }
    private static void DrawViewRange(BaseAI obj, Vector3 eye, string name, string text)
    {
      if (Settings.senseLineWidth == 0 || !obj) return;
      var line = Drawer.DrawCone(obj, eye, obj.m_viewRange, obj.m_viewAngle, Settings.creatureViewColor, Settings.senseLineWidth, Drawer.CREATURE);
      Drawer.AddText(line, name, text);
    }
    private static void DrawAlertRange(MonsterAI obj, Vector3 eye, string name, string text)
    {
      if (Settings.senseLineWidth == 0 || !obj) return;
      var line = Drawer.DrawArc(obj, eye, obj.m_alertRange, obj.m_viewAngle, Settings.creatureAlertViewColor, Settings.senseLineWidth, Drawer.CREATURE);
      Drawer.AddText(line, name, text);
    }
    private static void DrawFireLimit(BaseAI obj, string name, string text)
    {
      if (Settings.creatureFireLineWidth == 0 || !obj) return;
      if (!obj.m_afraidOfFire && !obj.m_avoidFire) return;
      var line = Drawer.DrawSphere(obj, 3f, Settings.creatureFireLimitColor, Settings.creatureFireLineWidth, Drawer.CREATURE);
      Drawer.AddText(line, name, text);
    }
    private static void DrawTotalLimit(Procreation obj, string name, string text)
    {
      if (Settings.breedingLineWidth == 0 || !obj) return;
      var line = Drawer.DrawSphere(obj, obj.m_totalCheckRange, Settings.creatureTotalLimitColor, Settings.breedingLineWidth, Drawer.CREATURE);
      Drawer.AddText(line, name, text);
    }
    private static void DrawPartnerCheck(Procreation obj, string name, string text)
    {
      if (Settings.breedingLineWidth == 0 || !obj) return;
      var line = Drawer.DrawSphere(obj, obj.m_partnerCheckRange, Settings.creaturePartnerCheckColor, Settings.breedingLineWidth, Drawer.CREATURE);
      Drawer.AddText(line, name, text);
    }
    private static void DrawFoodCheck(MonsterAI obj, string name, string text)
    {
      if (Settings.breedingLineWidth == 0 || !obj || obj.m_consumeItems.Count == 0) return;
      var line = Drawer.DrawSphere(obj, obj.m_consumeSearchRange, Settings.creatureFoodCheckColor, Settings.breedingLineWidth, Drawer.CREATURE);
      Drawer.AddText(line, name, text);
    }
    private static void DrawEatRange(MonsterAI obj, string name, string text)
    {
      if (Settings.breedingLineWidth == 0 || !obj || obj.m_consumeItems.Count == 0) return;
      var line = Drawer.DrawSphere(obj, obj.m_consumeRange, Settings.creatureEatRangeColor, Settings.breedingLineWidth, Drawer.CREATURE);
      Drawer.AddText(line, name, text);
    }
    public static void Postfix(BaseAI __instance, Character ___m_character)
    {
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