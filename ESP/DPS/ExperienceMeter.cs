using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(Player), "RaiseSkill")]
  public class Player_RaiseSkill
  {
    public static void Prefix(Player __instance, Skills.SkillType skill, float value, SEMan ___m_seman)
    {
      if (__instance != Player.m_localPlayer) return;
      var mod = 1f;
      ___m_seman.ModifyRaiseSkill(skill, ref mod);
      value *= mod;
      ExperienceMeter.AddExperience(skill, value);
      ExperienceMeter.SetExperienceModifier(mod);
    }
  }
  public class ExperienceMeter
  {
    private static DateTime? startTime = null;
    private static DateTime? endTime = null;
    private static Dictionary<Skills.SkillType, float> experiences = new Dictionary<Skills.SkillType, float>();
    private static float experienceModifier = 1.0f;
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
      experienceModifier = 1.0f;
      experiences.Clear();
    }
    public static void AddExperience(Skills.SkillType skill, float value = 1f)
    {
      Start();
      if (!startTime.HasValue) return;
      if (!experiences.ContainsKey(skill))
        experiences.Add(skill, 0);
      experiences[skill] += value;
      endTime = DateTime.Now;
    }
    public static void SetExperienceModifier(float value)
    {
      if (!startTime.HasValue) return;
      experienceModifier = value;
    }
    public static List<string> Get()
    {
      if (!Settings.showExperienceMeter) return null;
      var time = 1.0;
      if (startTime.HasValue && endTime.HasValue)
        time = endTime.Value.Subtract(startTime.Value).TotalMilliseconds;
      time /= 60000.0;
      var lines = experiences.Select(kvp => kvp.Key.ToString() + ": " + Format.Float(kvp.Value) + " (" + Format.Float(kvp.Value / time) + " per minute)").ToList();
      lines.Insert(0, "Experience gain: " + Format.Percent(experienceModifier));
      return lines;
    }
  }
}