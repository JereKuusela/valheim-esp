using System;
using UnityEngine;

namespace ESP
{
  public class EnvUtils
  {
    public static string GetCurrentEnvironment() => Format.String(EnvMan.instance.GetCurrentEnvironment().m_name);
    public static string GetProgress()
    {
      var limit = EnvMan.instance.m_environmentDuration;
      var value = ZNet.instance.GetTimeSeconds() % limit;
      return Format.ProgressPercent("Next", value, limit);
    }
    public static string GetEnvironmentRoll(float weight)
    {
      var seed = (long)ZNet.instance.GetTimeSeconds() / EnvMan.instance.m_environmentDuration; ;
      var state = UnityEngine.Random.state;
      UnityEngine.Random.InitState((int)seed);
      var roll = Format.Percent(UnityEngine.Random.Range(0f, weight) / weight);
      UnityEngine.Random.state = state;
      return roll;
    }
    private static string GetClock()
    {
      var fraction = Patch.EnvMan_GetDayFraction(EnvMan.instance);
      var seconds = fraction * 3600 * 24;
      var hours = Math.Floor(seconds / 3600);
      var minutes = Math.Floor((seconds - hours * 3600) / 60);
      return hours.ToString().PadLeft(2, '0') + ":" + minutes.ToString().PadLeft(2, '0');
    }
    public static string GetTime()
    {
      var time = EnvMan.instance.IsCold() ? "Night" : "Day";
      return Format.String(GetClock()) + " (" + Format.String(time) + ")";
    }
    public static string GetEnvironment(EnvEntry env, float totalWeight)
    {
      var current = EnvMan.instance.GetCurrentEnvironment().m_name;
      var text = Format.String(env.m_env.m_name, env.m_env.m_name == current);
      text += " (" + Format.Percent(env.m_weight / totalWeight) + "): ";
      text += Format.Range(env.m_env.m_windMin, env.m_env.m_windMax) + " wind";
      if (env.m_env.m_alwaysDark)
        text += ", " + Format.String("Dark");
      if (env.m_env.m_isFreezing)
        text += ", " + Format.String("Freezing");
      if (env.m_env.m_isFreezingAtNight)
        text += ", " + Format.String("Freezing at night");
      if (env.m_env.m_isCold)
        text += ", " + Format.String("Cold");
      if (!env.m_env.m_isColdAtNight)
        text += ", " + Format.String("Never cold");
      if (env.m_env.m_isWet)
        text += ", " + Format.String("Wet");
      return text;
    }
    public static string GetWind()
    {
      var windDir = EnvMan.instance.GetWindDir();
      var angle = Mathf.Atan2(windDir.x, windDir.z) / Math.PI * 180f;
      var windIntensity = EnvMan.instance.GetWindIntensity();
      return "Wind: " + Format.Percent(windIntensity) + " from " + Format.Int(angle) + " degrees";
    }
    public static double GetAvgWind(EnvEntry env) => (env.m_env.m_windMin + env.m_env.m_windMax) / 2.0 * env.m_weight;
  }
}

