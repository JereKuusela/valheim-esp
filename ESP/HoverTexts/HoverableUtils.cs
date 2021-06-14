using UnityEngine;

namespace ESP
{
  public class Hoverables
  {
    public static void AddHoverText(GameObject obj)
    {
      if (obj.GetComponent<Hoverable>() == null)
        obj.AddComponent<HoverText>().m_text = Format.Name(obj);
    }
    public static void AddTexts(GameObject obj, ref string __result)
    {
      if (!Settings.showExtraInfo) return;
      var character = obj.GetComponentInParent<Character>();
      var baseAI = obj.GetComponentInParent<BaseAI>();
      __result += Texts.Get(obj.GetComponentInParent<TreeLog>());
      __result += Texts.Get(obj.GetComponentInParent<TreeBase>());
      __result += Texts.Get(obj.GetComponentInParent<Destructible>());
      __result += Texts.Get(obj.GetComponentInParent<Pickable>());
      __result += Texts.Get(obj.GetComponentInParent<CreatureSpawner>());
      __result += Texts.Get(obj.GetComponentInParent<Beehive>());
      __result += Texts.Get(obj.GetComponentInParent<CookingStation>());
      __result += Texts.Get(obj.GetComponentInParent<Fermenter>());
      __result += Texts.Get(obj.GetComponentInParent<Fireplace>());
      __result += Texts.Get(obj.GetComponentInParent<Smelter>());
      __result += Texts.Get(obj.GetComponentInParent<WearNTear>());
      __result += Texts.Get(obj.GetComponentInParent<Plant>());
      __result += Texts.Get(obj.GetComponentInParent<PrivateArea>());
      __result += Texts.Get(obj.GetComponentInParent<MineRock>());
      __result += Texts.Get(obj.GetComponentInParent<MineRock5>());
      __result += Texts.Get(obj.GetComponentInParent<Ship>());
      __result += Texts.Get(obj.GetComponentInParent<EffectArea>());
      __result += Texts.Get(character, baseAI, obj.GetComponentInParent<MonsterAI>());
      __result += Texts.GetStatusStats(character);
      __result += Texts.Get(obj.GetComponentInParent<Tameable>());
      __result += Texts.Get(obj.GetComponentInParent<CharacterDrop>(), character);
      __result += Texts.Get(baseAI, obj.GetComponentInParent<Growup>());
      if (obj.GetComponentInParent<BaseAI>())
        __result += Texts.GetAttack(obj.GetComponentInParent<Humanoid>());
    }
  }
}
