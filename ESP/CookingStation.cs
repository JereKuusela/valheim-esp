using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(CookingStation), "GetHoverText")]
  public class CookingStation_GetHoverText
  {
    private static string GetItem(ZNetView m_nview, int slot)
    {
      return m_nview.GetZDO().GetString("slot" + slot, "");
    }
    private static float GetTime(ZNetView m_nview, int slot)
    {
      return m_nview.GetZDO().GetFloat("slot" + slot, 0f);
    }
    public static void Postfix(CookingStation __instance, ZNetView ___m_nview, ref string __result)
    {
      if (!Settings.showProgress)
        return;
      for (int i = 0; i < __instance.m_slots.Length; i++)
      {
        var itemName = GetItem(___m_nview, i);
        var cookedTime = GetTime(___m_nview, i);
        if (itemName == "") continue;
        var item = Patch.CookingStation_GetItemConversion(__instance, itemName);
        if (item == null) continue;
        var value = cookedTime;
        var limit = item.m_cookTime;
        if (limit > 0)
          __result += "\n" + TextUtils.ProgressPercent("Progress", value, limit);

      }
    }
  }
}