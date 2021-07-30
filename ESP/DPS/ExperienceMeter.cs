using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Skills.Skill), "Raise")]
  public class Skill_Raise
  {
    public static void Prefix(Skills.Skill __instance, float factor)
    {
      ExperienceMeter.AddExperience(__instance.m_info.m_skill, factor * __instance.m_info.m_increseStep);
    }
  }
  public class ExperienceMeter
  {
    private static DateTime? startTime = null;
    private static DateTime? endTime = null;
    private static Dictionary<Skills.SkillType, float> experiences = new Dictionary<Skills.SkillType, float>();
    public static void Start()
    {
      if (!Settings.showExperienceMeter) return;
      if (startTime.HasValue) return;
      Reset();
      startTime = DateTime.Now;
    }
    public static void Reset()
    {
      startTime = null;
      endTime = null;
      experiences.Clear();
    }
    public static void AddExperience(Skills.SkillType skill, float value = 1f)
    {
      if (!startTime.HasValue)
      {
        Start();
        return;
      }
      if (!experiences.ContainsKey(skill))
        experiences.Add(skill, 0);
      experiences[skill] += value;
      endTime = DateTime.Now;
    }
    public static float GetExperienceModifier()
    {
      var seMan = Patch.m_seman(Player.m_localPlayer);
      var mod = 1f;
      seMan.ModifyRaiseSkill(Skills.SkillType.All, ref mod);
      return mod;
    }
    public static float GetLevel(Skills.SkillType type)
    {
      var skills = Patch.m_skills(Player.m_localPlayer);
      var skill = Patch.Skills_GetSkill(skills, type);
      return skill.m_level;
    }
    public static float GetCurrent(Skills.SkillType type)
    {
      var skills = Patch.m_skills(Player.m_localPlayer);
      var skill = Patch.Skills_GetSkill(skills, type);
      return skill.m_accumulator;
    }
    public static float GetTotal(Skills.SkillType type)
    {
      var skills = Patch.m_skills(Player.m_localPlayer);
      var skill = Patch.Skills_GetSkill(skills, type);
      return Patch.Skill_GetNextLevelRequirement(skill);
    }
    public static List<string> Get()
    {
      if (!Settings.showExperienceMeter) return null;
      var time = 1.0;
      if (startTime.HasValue && endTime.HasValue)
        time = endTime.Value.Subtract(startTime.Value).TotalMilliseconds;
      time /= 60000.0;
      var lines = experiences.Select(kvp =>
      {
        var text = kvp.Key.ToString() + " " + GetLevel(kvp.Key) + " (" + Format.Progress(GetCurrent(kvp.Key), GetTotal(kvp.Key)) + "): ";
        text += Format.Float(kvp.Value) + " (" + Format.Float(kvp.Value / time) + " per minute)";
        return text;
      }).ToList();
      lines.Insert(0, "Experience gain: " + Format.Percent(GetExperienceModifier()));
      return lines;
    }
  }
}