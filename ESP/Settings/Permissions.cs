using System;
using System.Reflection;
using HarmonyLib;
using Visualization;

namespace Service;

public class PermissionManager
{

  public static bool IsFeatureEnabled(string feature, bool localConfigValue) => IsFeatureEnabledByHash(feature.ToLowerInvariant().GetStableHashCode(), localConfigValue);
  public static bool IsFeatureEnabledByHash(int featureHash, bool localConfigValue)
  {
    if (isFeatureEnabledByHashMethod == null) return localConfigValue && (!ZNet.instance || ZNet.instance.IsServer());
    return (bool)isFeatureEnabledByHashMethod.Invoke(null, ["esp", featureHash, localConfigValue]);
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