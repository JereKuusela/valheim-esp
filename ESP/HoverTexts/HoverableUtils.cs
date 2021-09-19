using System.Collections.Generic;
using UnityEngine;

namespace ESP {
  /// <summary>Custom text that doesn't show anything without content.</summary>
  public class CustomHoverText : MonoBehaviour, Hoverable {
    public string GetHoverText() {
      var text = "";
      Hoverables.AddTexts(gameObject, ref text);
      if (text == "") return "";
      return GetHoverName() + text;
    }
    public string GetHoverName() {
      if (title == "")
        title = Format.Name(this);
      return title;
    }
    private string title = "";
  }
  public class Hoverables {
    public static bool extraInfo {
      get => Settings.ExtraInfo && Admin.Enabled;
      set {
        if (value)
          Admin.Check();
        Settings.configExtraInfo.Value = value;
      }
    }
    public static void AddHoverText(MonoBehaviour obj) {
      if (obj.gameObject.GetComponent<Hoverable>() == null)
        obj.gameObject.AddComponent<CustomHoverText>();
    }
    public static void AddTexts(GameObject obj, ref string __result) {
      if (!extraInfo) return;
      var lines = new List<string>();
      lines.Add("Coordinates: " + Format.Coordinates(obj.transform.position));
      var character = obj.GetComponentInParent<Character>();
      var baseAI = obj.GetComponentInParent<BaseAI>();
      lines.Add(Texts.Get(obj.GetComponentInParent<TreeLog>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<TreeBase>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Destructible>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Pickable>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<CreatureSpawner>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<CraftingStation>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Beehive>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Bed>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<CookingStation>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Fermenter>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Fireplace>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Smelter>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<WearNTear>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Piece>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Plant>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<PrivateArea>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<MineRock>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<MineRock5>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<DropOnDestroyed>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<ItemDrop>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<SmokeSpawner>()));
      lines.Add(Texts.Get(obj.GetComponentInChildren<SmokeSpawner>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Smoke>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Container>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<Location>()));
      lines.Add(Texts.GetVegetation(obj));
      if (Settings.ShowShipStats)
        lines.Add(Texts.Get(obj.GetComponentInParent<Ship>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<EffectArea>()));
      lines.Add(Texts.Get(character, baseAI, obj.GetComponentInParent<MonsterAI>()));
      lines.Add(Texts.GetStatusStats(character));
      lines.Add(Texts.Get(obj.GetComponentInParent<Tameable>()));
      lines.Add(Texts.Get(obj.GetComponentInParent<CharacterDrop>(), character));
      lines.Add(Texts.Get(baseAI, obj.GetComponentInParent<Growup>()));
      if (obj.GetComponentInParent<BaseAI>())
        lines.Add(Texts.GetAttack(obj.GetComponentInParent<Humanoid>()));
      __result += "\n" + Format.JoinLines(lines);
    }
  }
}

