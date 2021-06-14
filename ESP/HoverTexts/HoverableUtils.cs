using UnityEngine;

namespace ESP
{
  public class HoverableUtils
  {
    public static void AddHoverText(GameObject obj)
    {
      if (obj.GetComponent<Hoverable>() != null) return;
      obj.AddComponent<HoverText>().m_text = TextUtils.Name(obj);
    }
    public static void AddTexts(GameObject obj, ref string __result)
    {
      if (!Settings.showExtraInfo) return;
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<TreeLog>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<TreeBase>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<Destructible>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<Pickable>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<CreatureSpawner>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<Beehive>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<CookingStation>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<Fermenter>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<Fireplace>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<Smelter>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<WearNTear>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<Plant>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<PrivateArea>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<Tameable>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<MineRock>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<MineRock5>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<Ship>());
      __result += HoverTextUtils.GetText(obj.GetComponentInParent<EffectArea>());
      if (obj.GetComponentInParent<BaseAI>())
        __result += HoverTextUtils.GetAttackText(obj.GetComponentInParent<Humanoid>());
    }
  }
}

