namespace ESP
{
  public partial class HoverTextUtils
  {
    private static string GetFedText(Tameable obj)
    {
      if (!Settings.showBreedingStats) return "";
      var feedingTime = Patch.GetLong(obj, "TameLastFeeding");
      var elapsed = Patch.GetElapsed(obj, "TameLastFeeding");
      var value = feedingTime == 0 ? 0 : obj.m_fedDuration - elapsed;
      var limit = obj.m_fedDuration;
      return "\n" + TextUtils.ProgressPercent("Food", value, limit);
    }
    private static string GetTamingText(Tameable obj)
    {
      if (!Settings.showBreedingStats) return "";
      var value = obj.m_tamingTime - Patch.Tameable_GetRemainingTime(obj);
      var limit = obj.m_tamingTime;
      return "\n" + TextUtils.ProgressPercent("Taming", value, limit);
    }
    private static string GetPregnancyText(Procreation obj)
    {
      if (!Settings.showBreedingStats || !obj || !obj.IsPregnant()) return "";
      var value = Patch.GetElapsed(obj, "pregnant");
      var limit = obj.m_pregnancyDuration;
      return "\n" + TextUtils.ProgressPercent("Pregnancy", value, limit);
    }
    private static string GetLoveText(Procreation obj)
    {
      if (!Settings.showBreedingStats || !obj || obj.IsPregnant()) return "";
      var value = Patch.GetInt(obj, "lovePoints");
      var limit = obj.m_requiredLovePoints;
      return "\nBreeding: " + TextUtils.Progress(value, limit) + ", " + TextUtils.Percent(obj.m_pregnancyChance) + " chance every " + TextUtils.Int(obj.m_updateInterval) + " seconds";
    }
    private static string GetPartnersText(Procreation obj)
    {
      if (!Settings.showBreedingStats || !obj || obj.IsPregnant()) return "";
      var prefab = Patch.GetPrefab(obj);
      var all = SpawnSystem.GetNrOfInstances(prefab, obj.transform.position, obj.m_partnerCheckRange, false, false) - 1;
      var partners = SpawnSystem.GetNrOfInstances(prefab, obj.transform.position, obj.m_partnerCheckRange, false, true) - 1;
      return "\nPartners: " + TextUtils.Int(partners) + " within " + obj.m_partnerCheckRange + " meters (" + TextUtils.Int(all) + " possible)";
    }
    private static string GetLimitText(Procreation obj)
    {
      if (!Settings.showBreedingStats || !obj) return "";
      var offspring = ZNetScene.instance.GetPrefab(Utils.GetPrefabName(obj.m_offspring));
      var prefab = Patch.GetPrefab(obj);
      var ownAmount = SpawnSystem.GetNrOfInstances(prefab, obj.transform.position, obj.m_totalCheckRange, false, false);
      var offspringAmount = SpawnSystem.GetNrOfInstances(offspring, obj.transform.position, obj.m_totalCheckRange, false, false);
      var value = ownAmount + offspringAmount;
      var limit = obj.m_maxCreatures;
      return "\nLimit: " + TextUtils.Progress(value, limit) + " within " + obj.m_totalCheckRange + " meters";
    }
    public static string GetText(Tameable obj)
    {
      if (!obj) return "";
      var procreation = obj.GetComponent<Procreation>();
      var character = obj.GetComponent<Character>();
      if (!character) return "";
      var text = GetFedText(obj);
      if (character.IsTamed())
      {
        text += GetLoveText(procreation);
        text += GetPregnancyText(procreation);
        text += GetPartnersText(procreation);
        text += GetLimitText(procreation);
      }
      else
      {
        text += GetTamingText(obj);
      }
      return text;
    }
  }
}