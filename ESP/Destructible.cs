using HarmonyLib;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch(typeof(Destructible), "Awake")]
  public class Destructible_Awake
  {
    public static void Postfix(Destructible __instance, ZNetView ___m_nview)
    {
      if (!Settings.showStructureHealth)
        return;
      // Hover text supports only one text so no point adding another.
      if (__instance.gameObject.GetComponent<Hoverable>() != null) return;
      var text = __instance.gameObject.AddComponent<HoverText>();
      text.m_text = TextUtils.Name(__instance.gameObject);
    }
  }
  [HarmonyPatch(typeof(Destructible), "RPC_Damage")]
  public class Destructible_RPC_Damage
  {
    public static void Postfix(Destructible __instance, HitData hit)
    {
      if (hit.GetAttacker() == Player.m_localPlayer)
        DPSMeter.AddStructureDamage(hit, __instance);
    }
  }
}