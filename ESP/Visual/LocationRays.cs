using System;
using HarmonyLib;
using Service;
using Visualization;
namespace ESP;
public class LocationUtils {
  public static bool IsIn(string arrayStr, string name) {
    var localized = Localization.instance.Localize(name);
    var nameLower = localized.ToLower().Replace("(clone)", "");
    var array = arrayStr.ToLower().Split(',');
    return Array.Exists(array, item => {
      if (item.StartsWith("*") && item.EndsWith("*")) return nameLower.Contains(item.Replace("*", ""));
      if (item.StartsWith("*")) return nameLower.EndsWith(item.Replace("*", ""));
      if (item.EndsWith("*")) return nameLower.StartsWith(item.Replace("*", ""));
      return item == nameLower;
    });
  }
  private static bool IsResourceEnabled(string name) => !IsIn(Settings.ExcludedResources, name);
  public static bool IsEnabled(Pickable obj) {
    if (!obj.m_itemPrefab) return false;
    var tag = GetTag(obj);
    if (Settings.IsDisabled(tag)) return false;
    return IsResourceEnabled(obj.m_itemPrefab.name);
  }
  public static bool IsEnabled(MineRock obj) {
    var tag = GetTag(obj.m_damageModifiers);
    if (Settings.IsDisabled(tag)) return false;
    var text = obj.GetComponent<HoverText>();
    if (text) return IsResourceEnabled(text.m_text);
    return IsResourceEnabled(obj.m_name);
  }
  public static bool IsEnabled(MineRock5 obj) {
    var tag = GetTag(obj.m_damageModifiers);
    if (Settings.IsDisabled(tag)) return false;
    var text = obj.GetComponent<HoverText>();
    if (text) return IsResourceEnabled(text.m_text);
    return IsResourceEnabled(obj.m_name);
  }
  public static bool IsEnabled(TreeLog obj) {
    var tag = GetTag(obj.m_damages);
    if (Settings.IsDisabled(tag)) return false;
    var text = obj.GetComponent<HoverText>();
    if (text) return IsResourceEnabled(text.m_text);
    return IsResourceEnabled(obj.name);
  }
  public static bool IsEnabled(TreeBase obj) {
    var tag = GetTag(obj.m_damageModifiers);
    if (Settings.IsDisabled(tag)) return false;
    var text = obj.GetComponent<HoverText>();
    if (text) return IsResourceEnabled(text.m_text);
    return IsResourceEnabled(obj.name);
  }
  public static bool IsEnabled(Destructible obj) {
    var tag = GetTag(obj.m_damages);
    if (Settings.IsDisabled(tag)) return false;
    var text = obj.GetComponent<HoverText>();
    if (text) return IsResourceEnabled(text.m_text);
    return IsResourceEnabled(obj.name);
  }
  public static string GetTag(HitData.DamageModifiers modifiers) {
    if (modifiers.m_chop == HitData.DamageModifier.Immune) return Tag.Ore;
    if (modifiers.m_pickaxe == HitData.DamageModifier.Immune) return Tag.Tree;
    return Tag.Destructible;
  }
  public static string GetTag(Pickable obj) {
    return obj.m_hideWhenPicked && obj.m_respawnTimeMinutes > 0f ? Tag.PickableRespawning : Tag.PickableOneTime;
  }
}
[HarmonyPatch(typeof(BaseAI), nameof(BaseAI.Awake))]
public class BaseAI_Ray {
  static void Postfix(Character ___m_character) {
    var obj = ___m_character;
    if (Settings.IsDisabled(Tag.TrackedCreature) || !CharacterUtils.IsTracked(obj)) return;
    var line = Draw.DrawMarkerLine(Tag.TrackedCreature, obj);
    Text.AddText(line);
  }
}
[HarmonyPatch(typeof(Pickable), nameof(Pickable.Awake))]
public class Pickable_Ray {
  static void Postfix(Pickable __instance, ZNetView ___m_nview) {
    if (!LocationUtils.IsEnabled(__instance)) return;
    var tag = LocationUtils.GetTag(__instance);
    var obj = Draw.DrawMarkerLine(tag, __instance);
    Text.AddText(obj, Translate.Name(__instance));
  }
}
[HarmonyPatch(typeof(Location), nameof(Location.Awake))]
public class Location_Ray {
  static void Postfix(Location __instance) {
    if (Settings.IsDisabled(Tag.Location)) return;
    var obj = Draw.DrawMarkerLine(Tag.Location, __instance);
    Text.AddText(obj, Translate.Name(__instance));
  }
}
[HarmonyPatch(typeof(Container), nameof(Container.Awake))]
public class Container_Ray {
  static void Postfix(Container __instance, Piece ___m_piece) {
    if (Settings.IsDisabled(Tag.Chest) || !___m_piece || ___m_piece.IsPlacedByPlayer()) return;
    var text = Format.String(__instance.GetHoverName());
    var obj = Draw.DrawMarkerLine(Tag.Chest, __instance);
    Text.AddText(obj, text);
  }
}
[HarmonyPatch(typeof(MineRock), nameof(MineRock.Start))]
public class MineRock_Ray {
  static void Postfix(MineRock __instance) {
    if (!LocationUtils.IsEnabled(__instance)) return;
    var damages = __instance.m_damageModifiers;
    var obj = Draw.DrawMarkerLine(LocationUtils.GetTag(damages), __instance);
    Text.AddText(obj, Translate.Name(__instance));
  }
}
[HarmonyPatch(typeof(MineRock5), nameof(MineRock5.Start))]
public class MineRock5_Ray {
  static void Postfix(MineRock5 __instance) {
    if (!LocationUtils.IsEnabled(__instance)) return;
    var damages = __instance.m_damageModifiers;
    var obj = Draw.DrawMarkerLine(LocationUtils.GetTag(damages), __instance);
    Text.AddText(obj, Translate.Name(__instance));
  }
}
[HarmonyPatch(typeof(Destructible), nameof(Destructible.Awake))]
public class Destructible_Ray {
  static void Postfix(Destructible __instance) {
    if (!LocationUtils.IsEnabled(__instance)) return;
    var damages = __instance.m_damages;
    var obj = Draw.DrawMarkerLine(LocationUtils.GetTag(damages), __instance);
    Text.AddText(obj, Translate.Name(__instance));
  }
}
[HarmonyPatch(typeof(TreeBase), nameof(TreeBase.Awake))]
public class TreeBase_Ray {
  static void Postfix(TreeBase __instance) {
    if (!LocationUtils.IsEnabled(__instance)) return;
    var damages = __instance.m_damageModifiers;
    var obj = Draw.DrawMarkerLine(LocationUtils.GetTag(damages), __instance);
    Text.AddText(obj, Translate.Name(__instance));
  }
}
[HarmonyPatch(typeof(TreeLog), nameof(TreeLog.Awake))]
public class TreeLog_Ray {
  static void Postfix(TreeLog __instance) {
    if (!LocationUtils.IsEnabled(__instance)) return;
    var damages = __instance.m_damages;
    var obj = Draw.DrawMarkerLine(LocationUtils.GetTag(damages), __instance);
    Text.AddText(obj, Translate.Name(__instance));
  }
}
[HarmonyPatch(typeof(CreatureSpawner), nameof(CreatureSpawner.Awake))]
public class CreatureSpawner_Ray {
  private static bool IsEnabled(CreatureSpawner obj) {
    var tag = obj.m_respawnTimeMinuts > 0f ? Tag.SpawnPointRespawning : Tag.SpawnPointOneTime;
    if (Settings.IsDisabled(tag)) return false;
    return !LocationUtils.IsIn(Settings.ExcludedCreatureSpawners, obj.name);
  }
  static void Postfix(CreatureSpawner __instance) {
    var obj = __instance;
    if (!IsEnabled(obj)) return;
    var tag = obj.m_respawnTimeMinuts > 0f ? Tag.SpawnPointRespawning : Tag.SpawnPointOneTime;
    var line = Draw.DrawMarkerLine(tag, obj);
    Text.AddText(line, Translate.Name(obj));
  }
}
