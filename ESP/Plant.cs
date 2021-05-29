using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  [HarmonyPatch(typeof(Plant), "GetHoverText")]
  public class Plant_GetHoverText
  {
    // Copypaste from decompiled.
    private static float GetGrowTime(Plant instance, ZNetView m_nview)
    {
      UnityEngine.Random.State state = UnityEngine.Random.state;
      UnityEngine.Random.InitState((int)((ulong)m_nview.GetZDO().m_uid.id + (ulong)m_nview.GetZDO().m_uid.userID));
      float value = UnityEngine.Random.value;
      UnityEngine.Random.state = state;
      return Mathf.Lerp(instance.m_growTime, instance.m_growTimeMax, value);
    }
    // Copypaste from decompiled.
    private static float TimeSincePlanted(ZNetView m_nview)
    {
      DateTime d = new DateTime(m_nview.GetZDO().GetLong("plantTime", ZNet.instance.GetTime().Ticks));
      return (float)(ZNet.instance.GetTime() - d).TotalSeconds;
    }
    public static void Postfix(Plant __instance, ZNetView ___m_nview, ref string __result)
    {
      if (!Settings.showProgress)
        return;
      var value = TimeSincePlanted(___m_nview);
      var limit = GetGrowTime(__instance, ___m_nview);
      if (limit > 0)
        __result += "\n" + TextUtils.ProgressValue("Progress", value, limit);
    }
  }
}