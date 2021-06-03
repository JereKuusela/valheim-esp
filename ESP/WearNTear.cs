using HarmonyLib;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch(typeof(WearNTear), "Awake")]
  public class WearNTear_Awake
  {
    public static void Postfix(WearNTear __instance)
    {
      if (!Settings.showProgress)
        return;
      var text = __instance.gameObject.AddComponent<WearNTearText>();
      text.wearNTear = __instance;
    }
  }
  public class WearNTearText : MonoBehaviour, Hoverable
  {
    public string GetHoverText()
    {
      var text = TextUtils.String(wearNTear.name);
      var health = wearNTear.GetHealthPercentage();

      text += "\n" + TextUtils.GetHealth(health * wearNTear.m_health, wearNTear.m_health);
      text += DamageModifierUtils.GetText(wearNTear.m_damages);
      return text;
    }
    public string GetHoverName() => wearNTear.name;
    public WearNTear wearNTear;
  }
}