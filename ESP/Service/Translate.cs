using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Service;
public class Translate
{
  public static string Name(string name, string color = "#FFFF00") => Format.String(Localization.instance.Localize(name).Replace("(Clone)", ""), color);
  public static string Name(Heightmap.Biome obj, string color = "#FFFF00") => Name(Enum.GetName(typeof(Heightmap.Biome), obj), color);
  private static string Name(Character obj) => obj ? obj.m_name : "";
  public static string Name(ItemDrop.ItemData obj, string color = "#FFFF00") => obj?.m_dropPrefab ? Name(Utils.GetPrefabName(obj?.m_dropPrefab), color) : Name(obj?.m_shared?.m_name ?? "");
  private static string Name(Pickable obj) => obj ? string.IsNullOrEmpty(obj.m_overrideName) ? obj.m_itemPrefab?.name ?? "" : obj.m_overrideName : "";
  public static string Name(CreatureSpawner obj) => obj && obj.m_creaturePrefab ? Utils.GetPrefabName(obj.m_creaturePrefab) : "";
  public static string Name(IEnumerable<GameObject> objs, string color = "#FFFF00") => Format.JoinRow(objs.Select(prefab => Id(prefab, color)));
  public static string Name(IEnumerable<ItemDrop> objs, string color = "#FFFF00") => Format.JoinRow(objs.Select(prefab => Name(prefab, color)));
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
  public static string Name(MonoBehaviour obj, string color = "yellow")
  {
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
    if (text == "") text = Id(obj.gameObject, color);
    return Name(text, color);
  }
  public static string Id(GameObject obj, string color = "yellow")
  {
    var view = obj.GetComponentInParent<ZNetView>();
    return view ? Id(view, color) : Format.String(Utils.GetPrefabName(obj), color);
  }
  public static string Id(ZNetView view, string color = "yellow")
  {
    var hash = view.GetZDO()?.GetPrefab() ?? 0;
    if (ZNetScene.instance.m_namedPrefabs.TryGetValue(hash, out var prefab))
      return Format.String(prefab.name, color);
    return "";
  }
}
