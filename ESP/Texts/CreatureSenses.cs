using System.Collections.Generic;
using Text;

namespace ESP {
  public partial class Texts {
    private static string FireRange(BaseAI obj) {
      if (Settings.CreatureFireLineWidth == 0 || !IsValid(obj)) return "";
      if (!obj.m_afraidOfFire && !obj.m_avoidFire) return "";
      return (obj.m_afraidOfFire ? "Fears fire" : "Avoids fire") + ": " + Format.Meters(3);
    }
    private static string ViewRange(BaseAI obj) {
      if (Settings.SenseLineWidth == 0 || !IsValid(obj)) return "";
      return "View range: " + Format.Meters(obj.m_viewRange)
         + "\nView angle: " + Format.Degrees(obj.m_viewAngle);
    }
    private static string Hearing(BaseAI obj) {
      if (Settings.SenseLineWidth == 0 || !IsValid(obj)) return "";
      var range = obj.m_hearRange;
      return "Hear range: " + (range > 100 ? Format.String("Infinite") : Format.Meters(range));
    }
    private static string AlertRange(MonsterAI obj) {
      if (Settings.SenseLineWidth == 0 || !IsValid(obj)) return "";
      return "Alert range: " + Format.Meters(obj.m_alertRange)
         + "\nAlert angle: " + Format.Degrees(obj.m_viewAngle);
    }
    private static string BreedingLimit(Procreation obj) {
      if (Settings.BreedingLineWidth == 0 || !IsValid(obj)) return "";
      return "Breeding limit: " + Format.Meters(obj.m_totalCheckRange);
    }
    private static string PartnerSearch(Procreation obj) {
      if (Settings.BreedingLineWidth == 0 || !IsValid(obj)) return "";
      return "Partner search: " + Format.Meters(obj.m_partnerCheckRange);
    }
    private static string FoodLimit(MonsterAI obj) {
      if (Settings.BreedingLineWidth == 0 || !IsValid(obj) || obj.m_consumeItems.Count == 0) return "";
      return "Food search: " + Format.Meters(obj.m_consumeSearchRange);
    }
    private static string GetEatRange(MonsterAI obj) {
      if (Settings.BreedingLineWidth == 0 || !IsValid(obj) || obj.m_consumeItems.Count == 0) return "";
      return "Eat range: " + Format.Meters(obj.m_consumeRange);
    }
    public static string GetSenses(BaseAI obj, MonsterAI monsterAI, Procreation procreation) {
      var lines = new List<string>();
      lines.Add(FireRange(obj));
      lines.Add(ViewRange(obj));
      lines.Add(AlertRange(monsterAI));
      lines.Add(Hearing(obj));
      lines.Add(BreedingLimit(procreation));
      lines.Add(PartnerSearch(procreation));
      lines.Add(FoodLimit(monsterAI));
      lines.Add(GetEatRange(monsterAI));
      return Format.JoinLines(lines);
    }
  }
}

