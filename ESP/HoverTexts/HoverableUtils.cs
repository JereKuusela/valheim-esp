using UnityEngine;

namespace ESP
{
  /// <summary>Custom text that doesn't show anything without content.</summary>
  public class CustomHoverText : MonoBehaviour, Hoverable
  {
    public string GetHoverText()
    {
      var text = "";
      Hoverables.AddTexts(gameObject, ref text);
      if (text == "") return "";
      return GetHoverName() + text;
    }
    public string GetHoverName()
    {
      if (title == "")
        title = Format.Name(this);
      return title;
    }
    private string title = "";
  }
  public class Hoverables
  {
    public static bool extraInfo
    {
      get => Settings.extraInfo && Cheats.IsAdmin;
      set
      {
        if (value)
          Cheats.CheckAdmin();
        Settings.configExtraInfo.Value = value;
      }
    }
    public static void AddHoverText(MonoBehaviour obj)
    {
      if (obj.gameObject.GetComponent<Hoverable>() == null)
        obj.gameObject.AddComponent<CustomHoverText>();
    }
    public static void AddTexts(GameObject obj, ref string __result)
    {
      if (!extraInfo) return;
      var character = obj.GetComponentInParent<Character>();
      var baseAI = obj.GetComponentInParent<BaseAI>();
      __result += Texts.Get(obj.GetComponentInParent<TreeLog>());
      __result += Texts.Get(obj.GetComponentInParent<TreeBase>());
      __result += Texts.Get(obj.GetComponentInParent<Destructible>());
      __result += Texts.Get(obj.GetComponentInParent<Pickable>());
      __result += Texts.Get(obj.GetComponentInParent<CreatureSpawner>());
      __result += Texts.Get(obj.GetComponentInParent<CraftingStation>());
      __result += Texts.Get(obj.GetComponentInParent<Beehive>());
      __result += Texts.Get(obj.GetComponentInParent<Bed>());
      __result += Texts.Get(obj.GetComponentInParent<CookingStation>());
      __result += Texts.Get(obj.GetComponentInParent<Fermenter>());
      __result += Texts.Get(obj.GetComponentInParent<Fireplace>());
      __result += Texts.Get(obj.GetComponentInParent<Smelter>());
      __result += Texts.Get(obj.GetComponentInParent<WearNTear>());
      __result += Texts.Get(obj.GetComponentInParent<Piece>());
      __result += Texts.Get(obj.GetComponentInParent<Plant>());
      __result += Texts.Get(obj.GetComponentInParent<PrivateArea>());
      __result += Texts.Get(obj.GetComponentInParent<MineRock>());
      __result += Texts.Get(obj.GetComponentInParent<MineRock5>());
      __result += Texts.Get(obj.GetComponentInParent<ItemDrop>());
      __result += Texts.Get(obj.GetComponentInParent<SmokeSpawner>());
      __result += Texts.Get(obj.GetComponentInChildren<SmokeSpawner>());
      __result += Texts.Get(obj.GetComponentInParent<Smoke>());
      if (Settings.showShipStats)
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

