using HarmonyLib;
using UnityEngine;

namespace ESP
{
  public class MineRock5Utils
  {
    public static string GetText(MineRock5 mineRock5)
    {
      if (!mineRock5 || !Settings.showStructureHealth) return "";
      var text = "";
      var maxHealth = mineRock5.m_health;

      text += "\nHealth per area: " + TextUtils.Int(maxHealth);
      text += DamageModifierUtils.GetText(mineRock5.m_damageModifiers, false);
      text += "\nHit noise: " + TextUtils.Int(100);
      return text;
    }
  }
  [HarmonyPatch(typeof(MineRock5), "DamageArea")]
  public class MineRock5_DamageArea
  {
    public static void Postfix(MineRock5 __instance, HitData hit)
    {
      if (hit.GetAttacker() == Player.m_localPlayer)
        DPSMeter.AddStructureDamage(hit, __instance);
    }
  }
  [HarmonyPatch(typeof(MineRock5), "GetHoverText")]
  public class MineRock5_GetHoverText
  {
    public static void Postfix(MineRock5 __instance, ref string __result)
    {
      __result += MineRock5Utils.GetText(__instance);
    }
  }
}