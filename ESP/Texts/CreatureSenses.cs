using System.Collections.Generic;
using System.Linq;

namespace ESP
{
  public partial class Texts
  {
    private static string FireRange(BaseAI obj)
    {
      if (!Settings.showCreatureFireLimits || !obj) return "";
      if (!obj.m_afraidOfFire && !obj.m_avoidFire) return "";
      return (obj.m_afraidOfFire ? "Fears fire" : "Avoids fire") + ": " + Format.Meters(3);
    }
    private static string ViewRange(BaseAI obj)
    {
      if (!Settings.showBaseAI || !obj) return "";
      return "View range: " + Format.Meters(obj.m_viewRange)
         + "\nView angle: " + Format.Degrees(obj.m_viewAngle);
    }
    private static string Hearing(BaseAI obj)
    {
      if (!Settings.showBaseAI || !obj) return "";
      var range = obj.m_hearRange;
      return "Hear range: " + (range > 100 ? Format.String("Infinite") : Format.Meters(range));
    }
    private static string AlertRange(MonsterAI obj)
    {
      if (!Settings.showBaseAI || !obj) return "";
      return "Alert range: " + Format.Meters(obj.m_alertRange)
         + "\nAlert angle: " + Format.Degrees(obj.m_viewAngle);
    }
    private static string BreedingLimit(Procreation obj)
    {
      if (!Settings.showBreedingLimits || !obj) return "";
      return "Breeding limit: " + Format.Meters(obj.m_totalCheckRange);
    }
    private static string PartnerSearch(Procreation obj)
    {
      if (!Settings.showBreedingLimits || !obj) return "";
      return "Partner search: " + Format.Meters(obj.m_partnerCheckRange);
    }
    private static string FoodLimit(MonsterAI obj)
    {
      if (!Settings.showBreedingLimits || !obj || obj.m_consumeItems.Count == 0) return "";
      return "Food search: " + Format.Meters(obj.m_consumeSearchRange);
    }
    private static string GetEatRange(MonsterAI obj)
    {
      if (!Settings.showBreedingLimits || !obj || obj.m_consumeItems.Count == 0) return "";
      return "Eat range: " + Format.Meters(obj.m_consumeRange);
    }
    public static string GetSenses(BaseAI obj, MonsterAI monsterAI, Procreation procreation)
    {
      var texts = new List<string>();
      texts.Add(FireRange(obj));
      texts.Add(ViewRange(obj));
      texts.Add(AlertRange(monsterAI));
      texts.Add(Hearing(obj));
      texts.Add(BreedingLimit(procreation));
      texts.Add(PartnerSearch(procreation));
      texts.Add(FoodLimit(monsterAI));
      texts.Add(GetEatRange(monsterAI));
      return string.Join("\n", texts.Where(text => text != ""));
    }
  }
}

