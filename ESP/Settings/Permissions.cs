using System;
using System.Reflection;
using HarmonyLib;
using Visualization;

namespace Service;

public class PermissionManager
{

  public static bool IsVisualFeatureEnabled(int featureHash, bool localConfigValue)
  {
    return IsFeatureEnabled("esp_visuals", featureHash, localConfigValue);
  }

  public static bool IsStatsFeatureEnabled(int featureHash, bool localConfigValue)
  {
    return IsFeatureEnabled("esp_stats", featureHash, localConfigValue);
  }

  public static bool IsHudFeatureEnabled(int featureHash, bool localConfigValue)
  {
    return IsFeatureEnabled("esp_hud", featureHash, localConfigValue);
  }

  private static bool IsFeatureEnabled(string sectionKey, int featureHash, bool localConfigValue)
  {
    if (isFeatureEnabledByHashMethod == null) return localConfigValue && (!ZNet.instance || ZNet.instance.IsServer());
    return (bool)isFeatureEnabledByHashMethod.Invoke(null, [sectionKey, featureHash, localConfigValue]);
  }

  private static MethodInfo? isFeatureEnabledByHashMethod;
  public static void SetupSDC(Assembly assembly)
  {
    if (assembly == null) return;
    var type = assembly.GetType("ServerDevcommands.PermissionApi");
    if (type == null) return;
    var method = AccessTools.Method(type, "IsFeatureEnabledByHash");
    if (method == null) return;
    isFeatureEnabledByHashMethod = method;

    var subscribe = AccessTools.Method(type, "Subscribe", [typeof(Action)]);
    if (subscribe == null) return;

    Action handler = Visibility.Reload;
    subscribe.Invoke(null, [handler]);
  }
}