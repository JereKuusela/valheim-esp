using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(CookingStation), "GetHoverText")]
  public class CookingStation_GetHoverText
  {
    private static string GetItem(ZNetView nview, int slot) => nview.GetZDO().GetString("slot" + slot, "");
    private static float GetTime(ZNetView nview, int slot) => nview.GetZDO().GetFloat("slot" + slot, 0f);
    private static string GetSlotText(CookingStation instance, ZNetView nview, int slot)
    {
      if (!Settings.showProgress) return "";
      var itemName = GetItem(nview, slot);
      var cookedTime = GetTime(nview, slot);
      if (itemName == "") return "";
      var item = Patch.CookingStation_GetItemConversion(instance, itemName);
      if (item == null) return "";
      var limit = item.m_cookTime;
      if (limit == 0) return "";
      var value = cookedTime;
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static void Postfix(CookingStation __instance, ZNetView ___m_nview, ref string __result)
    {
      for (var slot = 0; slot < __instance.m_slots.Length; slot++)
      {
        __result += GetSlotText(__instance, ___m_nview, slot);
      }
      var wearNTear = __instance.GetComponent<WearNTear>();
      __result += WearNTearUtils.GetText(wearNTear);
    }
  }
}