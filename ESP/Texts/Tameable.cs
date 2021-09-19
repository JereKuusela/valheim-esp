using System.Collections.Generic;

namespace ESP {
  public partial class Texts {
    private static string GetFed(Tameable obj, MonsterAI monsterAI) {
      if (!Settings.Breeding) return "";
      var feedingTime = Patch.GetLong(obj, "TameLastFeeding");
      var elapsed = Patch.GetElapsed(obj, "TameLastFeeding");
      var value = feedingTime == 0 ? 0 : obj.m_fedDuration - elapsed;
      var fed = value > 0;
      if (fed)
        return Format.ProgressPercent("Food", value, obj.m_fedDuration);
      if (monsterAI) {
        value = Patch.ConsumeSearchTimer(monsterAI);
        return Format.ProgressPercent("Searching food", value, monsterAI.m_consumeSearchInterval);
      }
      return "";
    }
    private static string GetTaming(Tameable obj) {
      if (!Settings.Breeding) return "";
      var value = obj.m_tamingTime - Patch.Tameable_GetRemainingTime(obj);
      var limit = obj.m_tamingTime;
      return Format.ProgressPercent("Taming", value, limit);
    }
    private static string GetPregnancy(Procreation obj) {
      if (!Settings.Breeding || !obj || !obj.IsPregnant()) return "";
      var value = Patch.GetElapsed(obj, "pregnant");
      var limit = obj.m_pregnancyDuration;
      return Format.ProgressPercent("Pregnancy", value, limit);
    }
    private static string GetLove(Procreation obj) {
      if (!Settings.Breeding || !obj || obj.IsPregnant()) return "";
      var value = Patch.GetInt(obj, "lovePoints");
      var limit = obj.m_requiredLovePoints;
      return "Breeding: " + Format.Progress(value, limit) + ", " + Format.Percent(obj.m_pregnancyChance) + " chance every " + Format.Int(obj.m_updateInterval) + " seconds";
    }
    private static string GetPartners(Procreation obj) {
      if (!Settings.Breeding || !obj || obj.IsPregnant()) return "";
      var prefab = Patch.GetPrefab(obj);
      var all = SpawnSystem.GetNrOfInstances(prefab, obj.transform.position, obj.m_partnerCheckRange, false, false) - 1;
      var partners = SpawnSystem.GetNrOfInstances(prefab, obj.transform.position, obj.m_partnerCheckRange, false, true) - 1;
      return "Partners: " + Format.Int(partners) + " within " + obj.m_partnerCheckRange + " meters (" + Format.Int(all) + " possible)";
    }
    private static string GetLimit(Procreation obj) {
      if (!Settings.Breeding || !obj) return "";
      var offspring = ZNetScene.instance.GetPrefab(Utils.GetPrefabName(obj.m_offspring));
      var prefab = Patch.GetPrefab(obj);
      var ownAmount = SpawnSystem.GetNrOfInstances(prefab, obj.transform.position, obj.m_totalCheckRange, false, false);
      var offspringAmount = SpawnSystem.GetNrOfInstances(offspring, obj.transform.position, obj.m_totalCheckRange, false, false);
      var value = ownAmount + offspringAmount;
      var limit = obj.m_maxCreatures;
      return "Limit: " + Format.Progress(value, limit) + " within " + obj.m_totalCheckRange + " meters";
    }
    public static string Get(Tameable obj) {
      if (!obj) return "";
      var procreation = obj.GetComponent<Procreation>();
      var character = obj.GetComponent<Character>();
      var monsterAI = obj.GetComponent<MonsterAI>();
      if (!character) return "";
      var lines = new List<string>();
      lines.Add(GetFed(obj, monsterAI));
      if (character.IsTamed()) {
        lines.Add(GetLove(procreation));
        lines.Add(GetPregnancy(procreation));
        lines.Add(GetPartners(procreation));
        lines.Add(GetLimit(procreation));
      } else {
        lines.Add(GetTaming(obj));
      }
      return Format.JoinLines(lines);
    }
  }
}