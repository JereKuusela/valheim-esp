using System;
using UnityEngine;

namespace ESP
{
  public class EnvUtils
  {
    public static string GetCurrentEnvironment() => TextUtils.String(EnvMan.instance.GetCurrentEnvironment().m_name);
    public static string GetProgress()
    {
      var limit = EnvMan.instance.m_environmentDuration;
      var value = ZNet.instance.GetTimeSeconds() % limit;
      return TextUtils.ProgressPercent("Next", value, limit);
    }
    public static string GetEnvironmentRoll(float weight)
    {
      var seed = (long)ZNet.instance.GetTimeSeconds() / EnvMan.instance.m_environmentDuration; ;
      var state = UnityEngine.Random.state;
      UnityEngine.Random.InitState((int)seed);
      var roll = TextUtils.Percent(UnityEngine.Random.Range(0f, weight) / weight);
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
      return TextUtils.String(GetClock()) + " (" + TextUtils.String(time) + ")";
    }
    public static string GetEnvironment(EnvEntry env, float totalWeight)
    {
      var current = EnvMan.instance.GetCurrentEnvironment().m_name;
      var text = TextUtils.String(env.m_env.m_name, env.m_env.m_name == current);
      text += " (" + TextUtils.Percent(env.m_weight / totalWeight) + "): ";
      text += TextUtils.Range(env.m_env.m_windMin, env.m_env.m_windMax) + " wind";
      if (env.m_env.m_alwaysDark)
        text += ", " + TextUtils.String("Dark");
      if (env.m_env.m_isFreezing)
        text += ", " + TextUtils.String("Freezing");
      if (env.m_env.m_isFreezingAtNight)
        text += ", " + TextUtils.String("Freezing at night");
      if (env.m_env.m_isCold)
        text += ", " + TextUtils.String("Cold");
      if (!env.m_env.m_isColdAtNight)
        text += ", " + TextUtils.String("Never cold");
      if (env.m_env.m_isWet)
        text += ", " + TextUtils.String("Wet");
      return text;
    }
    public static string GetWind()
    {
      var windDir = EnvMan.instance.GetWindDir();
      var angle = Mathf.Atan2(windDir.x, windDir.z) / Math.PI * 180f;
      var windIntensity = EnvMan.instance.GetWindIntensity();
      return "Wind: " + TextUtils.Percent(windIntensity) + " from " + TextUtils.Int(angle) + " degrees";
    }
    public static double GetAvgWind(EnvEntry env) => (env.m_env.m_windMin + env.m_env.m_windMax) / 2.0 * env.m_weight;
  }
}

