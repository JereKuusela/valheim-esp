using HarmonyLib;
using UnityEngine;

namespace ESP
{
  public class WearNTearUtils
  {
    public static string GetText(WearNTear wearNTear)
    {
      if (!wearNTear || !Settings.showStructureHealth) return "";
      var text = "";
      var health = wearNTear.GetHealthPercentage();

      text += "\n" + TextUtils.GetHealth(health * wearNTear.m_health, wearNTear.m_health);
      text += DamageModifierUtils.GetText(wearNTear.m_damages);
      return text;
    }
  }
  [HarmonyPatch(typeof(WearNTear), "Awake")]
  public class WearNTear_Awake
  {
    public static void Postfix(WearNTear __instance)
    {
      if (!Settings.showStructureHealth)
        return;
      // Hover text supports only one text so no point adding another.
      if (__instance.gameObject.GetComponent<Hoverable>() != null) return;
      var text = __instance.gameObject.AddComponent<WearNTearText>();
      text.wearNTear = __instance;
    }
  }
  public class WearNTearText : MonoBehaviour, Hoverable
  {
    public string GetHoverText() => TextUtils.Name(wearNTear.gameObject) + WearNTearUtils.GetText(wearNTear);
    public string GetHoverName() => wearNTear.name;
    public WearNTear wearNTear;
  }
}