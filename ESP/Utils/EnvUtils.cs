using System;
using Service;
using UnityEngine;
namespace ESP;
public class EnvUtils {
  public static string GetCurrentEnvironment() => Format.String(EnvMan.instance.GetCurrentEnvironment().m_name);
  public static string GetProgress() {
    var limit = EnvMan.instance.m_environmentDuration;
    var value = ZNet.instance.GetTimeSeconds() % limit;
    return Format.ProgressPercent("Next", value, limit);
  }
  public static string GetEnvironmentRoll() {
    var seed = (long)ZNet.instance.GetTimeSeconds() / EnvMan.instance.m_environmentDuration;
    var state = UnityEngine.Random.state;
    UnityEngine.Random.InitState((int)seed);
    var roll = UnityEngine.Random.Range(0f, 1f);
    UnityEngine.Random.state = state;
    return Format.Percent(roll) + " (seed " + seed + ")";
  }
  private static string GetClock() {
    var limit = EnvMan.instance.m_dayLengthSec;
    var fraction = (ZNet.instance.GetTimeSeconds() % limit) / limit;
    var seconds = fraction * 3600 * 24;
    var hours = Math.Floor(seconds / 3600);
    var minutes = Math.Floor((seconds - hours * 3600) / 60);
    return hours.ToString().PadLeft(2, '0') + ":" + minutes.ToString().PadLeft(2, '0');
  }
  public static string GetTime() {
    var time = EnvMan.instance.IsCold() ? "Night" : "Day";
    return Format.String(GetClock()) + " (" + Format.String(time) + ")";
  }
  public static string GetEnvironment(EnvEntry env, float totalWeight) {
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
  public static string GetWindWithAngle() {
    var windDir = EnvMan.instance.GetWindDir();
    var angle = Mathf.Atan2(windDir.x, windDir.z) / Math.PI * 180f;
    return GetWind() + " from " + Format.Int(angle) + " degrees";
  }
  public static string GetWind() {
    var windIntensity = EnvMan.instance.GetWindIntensity();
    return "Wind: " + Format.Percent(windIntensity);
  }
  public static string GetWindRoll() {
    UnityEngine.Random.State state = UnityEngine.Random.state;
    var angle = 0f;
    var intensity = 0.5f;
    var time = (long)ZNet.instance.GetTimeSeconds();
    EnvMan.instance.AddWindOctave(time, 1, ref angle, ref intensity);
    EnvMan.instance.AddWindOctave(time, 2, ref angle, ref intensity);
    EnvMan.instance.AddWindOctave(time, 4, ref angle, ref intensity);
    EnvMan.instance.AddWindOctave(time, 8, ref angle, ref intensity);
    UnityEngine.Random.state = state;
    return Format.Percent(intensity) + " and " + Format.Degrees(angle * 180 / Math.PI);
  }
  public static string GetWindHud() {
    var windIntensity = EnvMan.instance.GetWindIntensity();
    return Format.Percent(windIntensity) + " wind";
  }
  public static double GetAvgWind(EnvEntry env) => (env.m_env.m_windMin + env.m_env.m_windMax) / 2.0 * env.m_weight;

  public static string GetLocation(Vector3 position) => "Position: " + Format.Coordinates(position);
  public static string GetAltitude(Vector3 position) => "Altitude: " + Format.Int(position.y - ZoneSystem.instance.m_waterLevel);
  public static string GetBlocked(Vector3 position) => ZoneSystem.instance.IsBlocked(position) ? "blocked" : "";
  public static string GetForest(Vector3 position) {
    var inForest = WorldGenerator.InForest(position);
    return (inForest ? "Forest" : "No forest") + " (" + Format.Float(WorldGenerator.GetForestFactor(position)) + ")";
  }
}
