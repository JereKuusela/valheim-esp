using HarmonyLib;
using System;

namespace ESP
{
  public class HoverTextUtils
  {
    private static string GetRespawnTime(Pickable pickable, ZNetView zNetView)
    {
      if (!pickable.m_hideWhenPicked || pickable.m_respawnTimeMinutes == 0) return "Never";
      var time = ZNet.instance.GetTime();
      var d = new DateTime(zNetView.GetZDO().GetLong("picked_time", 0L));
      var timer = (time - d).TotalMinutes;
      var picked = zNetView.GetZDO().GetBool("picked", false); ;
      var timerString = picked ? timer.ToString("N0") : "Not picked";
      return timerString + " / " + pickable.m_respawnTimeMinutes.ToString("N0") + " minutes";
    }
    public static string GetText(Pickable pickable)
    {
      if (!pickable || !Settings.showStructureHealth) return "";
      var zNetView = Patch.m_nview(pickable);
      var respawn = GetRespawnTime(pickable, zNetView);
      var lines = new string[]{
        "Respawn: " + TextUtils.String(respawn)
      };
      if (pickable.m_amount > 0)
        lines.AddItem("Amount: " + TextUtils.String(pickable.m_amount.ToString()));
      return "\n" + lines.Join(null, "\n");
    }
    public static string GetText(TreeLog treeLog)
    {
      if (!treeLog || !Settings.showStructureHealth) return "";
      var zNetView = Patch.m_nview(treeLog);
      var text = "";
      var maxHealth = treeLog.m_health;
      var health = zNetView.GetZDO().GetFloat("health", maxHealth);

      text += "\n" + TextUtils.GetHealth(health, maxHealth);
      text += "\nHit noise: " + TextUtils.Int(treeLog.m_hitNoise);
      text += DamageModifierUtils.GetText(treeLog.m_damages, false);
      return text;
    }
    public static string GetText(TreeBase treeBase)
    {
      if (!treeBase || !Settings.showStructureHealth) return "";
      var zNetView = Patch.m_nview(treeBase);
      var text = "";
      var maxHealth = treeBase.m_health;
      var health = zNetView.GetZDO().GetFloat("health", maxHealth);

      text += "\n" + TextUtils.GetHealth(health, maxHealth);
      text += "\nHit noise: " + TextUtils.Int(100);
      text += DamageModifierUtils.GetText(treeBase.m_damageModifiers, false);
      return text;
    }
    public static string GetText(Destructible destructible)
    {
      if (!destructible || !Settings.showStructureHealth) return "";
      var zNetView = Patch.m_nview(destructible);
      var text = "";
      var health = zNetView.GetZDO().GetFloat("health", destructible.m_health);
      var maxHealth = destructible.m_health;

      text += "\n" + TextUtils.GetHealth(health, maxHealth);
      text += "\nHit noise: " + TextUtils.Int(destructible.m_hitNoise);
      text += DamageModifierUtils.GetText(destructible.m_damages, false);
      return text;
    }
  }
  [HarmonyPatch(typeof(HoverText), "GetHoverText")]
  public class HoverText_GetHoverText
  {
    public static void Postfix(HoverText __instance, ref string __result)
    {
      __result += HoverTextUtils.GetText(__instance.GetComponent<TreeLog>());
      __result += HoverTextUtils.GetText(__instance.GetComponent<TreeBase>());
      __result += HoverTextUtils.GetText(__instance.GetComponent<Destructible>());
      __result += HoverTextUtils.GetText(__instance.GetComponent<Pickable>());
    }
  }
}