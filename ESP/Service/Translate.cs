using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Service;
public class Translate {
  public static string Name(string name, string color = "yellow") => Format.String(Localization.instance.Localize(name).Replace("(Clone)", ""), color);
  public static string Name(Heightmap.Biome obj, string color = "yellow") => Name(Names.GetName(obj), color);
  private static string Name(Character obj) => obj ? obj.m_name : "";
  public static string Name(ItemDrop.ItemData obj, string color = "yellow") => obj != null ? Name(obj.m_shared.m_name, color) : "";
  private static string Name(Pickable obj) => obj ? obj.m_itemPrefab.name : "";
  public static string Name(CreatureSpawner obj) => obj ? Utils.GetPrefabName(obj.m_creaturePrefab) : "";
  public static string Name(IEnumerable<GameObject> objs, string color = "yellow") => Format.JoinRow(objs.Select(prefab => Name(prefab, color)));
  public static string Name(IEnumerable<ItemDrop> objs, string color = "yellow") => Format.JoinRow(objs.Select(prefab => Name(prefab, color)));
  private static string Name(Bed obj) => obj ? obj.GetHoverName() : "";
  private static string Name(Piece obj) => obj ? obj.m_name : "";
  private static string Name(TreeLog obj) => obj ? obj.name : "";
  private static string Name(Location obj) => obj ? obj.name : "";
  private static string Name(MineRock obj) => obj ? obj.m_name : "";
  private static string Name(MineRock5 obj) => obj ? obj.m_name : "";
  private static string Name(TreeBase obj) => obj ? obj.name : "";
  private static string Name(Destructible obj) => obj ? obj.name : "";
  private static string Name(Smoke obj) => obj ? "Smoke" : "";
  private static string Name(HoverText obj) => obj ? obj.m_text : "";
  public static string Name(MonoBehaviour obj, string color = "yellow") {
    var text = "";
    if (text == "") text = Name(obj.GetComponentInParent<HoverText>());
    if (text == "") text = Name(obj.GetComponentInParent<Smoke>());
    if (text == "") text = Name(obj.GetComponentInParent<CreatureSpawner>());
    if (text == "") text = Name(obj.GetComponentInParent<Pickable>());
    if (text == "") text = Name(obj.GetComponentInParent<Bed>());
    if (text == "") text = Name(obj.GetComponentInParent<TreeLog>());
    if (text == "") text = Name(obj.GetComponentInParent<Location>());
    if (text == "") text = Name(obj.GetComponentInParent<MineRock>());
    if (text == "") text = Name(obj.GetComponentInParent<MineRock5>());
    if (text == "") text = Name(obj.GetComponentInParent<TreeBase>());
    if (text == "") text = Name(obj.GetComponentInParent<Destructible>());
    if (text == "") text = Name(obj.GetComponentInParent<Character>());
    if (text == "") text = Name(obj.GetComponentInParent<Piece>());
    if (text == "") text = Name(obj.gameObject, color);
    return Name(text, color);
  }
  public static string Name(GameObject obj, string color = "yellow") => obj ? Name(Utils.GetPrefabName(obj), color) : "";
}
