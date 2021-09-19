using System;
using HarmonyLib;
using UnityEngine;

namespace ESP {
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
      if (Settings.PickableRayWidth == 0) return false;
      return IsResourceEnabled(obj.m_itemPrefab.name);
    }
    public static bool IsEnabled(MineRock obj) {
      var width = LocationUtils.GetRayWidth(obj.m_damageModifiers);
      if (width == 0) return false;
      var text = obj.GetComponent<HoverText>();
      if (text) return IsResourceEnabled(text.m_text);
      return IsResourceEnabled(obj.m_name);
    }
    public static bool IsEnabled(MineRock5 obj) {
      var width = LocationUtils.GetRayWidth(obj.m_damageModifiers);
      if (width == 0) return false;
      var text = obj.GetComponent<HoverText>();
      if (text) return IsResourceEnabled(text.m_text);
      return IsResourceEnabled(obj.m_name);
    }
    public static bool IsEnabled(TreeLog obj) {
      var width = LocationUtils.GetRayWidth(obj.m_damages);
      if (width == 0) return false;
      var text = obj.GetComponent<HoverText>();
      if (text) return IsResourceEnabled(text.m_text);
      return IsResourceEnabled(obj.name);
    }
    public static bool IsEnabled(TreeBase obj) {
      var width = LocationUtils.GetRayWidth(obj.m_damageModifiers);
      if (width == 0) return false;
      var text = obj.GetComponent<HoverText>();
      if (text) return IsResourceEnabled(text.m_text);
      return IsResourceEnabled(obj.name);
    }
    public static bool IsEnabled(Destructible obj) {
      var width = LocationUtils.GetRayWidth(obj.m_damages);
      if (width == 0) return false;
      var text = obj.GetComponent<HoverText>();
      if (text) return IsResourceEnabled(text.m_text);
      return IsResourceEnabled(obj.name);
    }
    public static float GetRayWidth(HitData.DamageModifiers modifiers) {
      if (modifiers.m_chop == HitData.DamageModifier.Immune) return Settings.OreRayWidth;
      if (modifiers.m_pickaxe == HitData.DamageModifier.Immune) return Settings.TreeRayWidth;
      return Settings.DestructibleRayWidth;
    }
    public static Color GetRayColor(HitData.DamageModifiers modifiers) {
      if (modifiers.m_chop == HitData.DamageModifier.Immune) return Settings.OreRayColor;
      if (modifiers.m_pickaxe == HitData.DamageModifier.Immune) return Settings.TreeRayColor;
      return Settings.DestructibleRayColor;
    }
  }
  [HarmonyPatch(typeof(BaseAI), "Awake")]
  public class BaseAI_Ray {
    public static void Postfix(Character ___m_character) {
      var obj = ___m_character;
      if (Settings.CreatureRayWidth == 0 || !CharacterUtils.IsTracked(obj)) return;
      var line = Drawer.DrawMarkerLine(obj, Settings.CreatureRayColor, Settings.CreatureRayWidth, Drawer.CREATURE);
      Drawer.AddText(line);
    }
  }
  [HarmonyPatch(typeof(Pickable), "Awake")]
  public class Pickable_Ray {
    private static bool IsEnabled(Pickable instance) {
      if (Settings.PickableRayWidth == 0) return false;
      var name = instance.m_itemPrefab.name.ToLower();
      var excluded = Settings.ExcludedResources.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return false;
      return true;
    }
    private static Color GetColor(Pickable instance) {
      return instance.m_hideWhenPicked && instance.m_respawnTimeMinutes > 0 ? Settings.PickableRespawningColor : Settings.PickableOneTimeColor;
    }
    public static void Postfix(Pickable __instance, ZNetView ___m_nview) {
      if (!IsEnabled(__instance))
        return;
      var color = GetColor(__instance);
      var obj = Drawer.DrawMarkerLine(__instance, color, Settings.PickableRayWidth, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(Location), "Awake")]
  public class Location_Ray {
    public static void Postfix(Location __instance) {
      if (Settings.LocationRayWidth == 0)
        return;
      var obj = Drawer.DrawMarkerLine(__instance, Settings.LocationRayColor, Settings.LocationRayWidth, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(Container), "Awake")]
  public class Container_Ray {
    public static void Postfix(Container __instance, Piece ___m_piece) {
      if (Settings.ChestRayWidth == 0 || !___m_piece || ___m_piece.IsPlacedByPlayer()) return;
      var text = Format.String(__instance.GetHoverName());
      var obj = Drawer.DrawMarkerLine(__instance, Settings.ContainerRayColor, Settings.ChestRayWidth, Drawer.OTHER);
      Drawer.AddText(obj, text);
    }
  }
  [HarmonyPatch(typeof(MineRock), "Start")]
  public class MineRock_Ray {
    public static void Postfix(MineRock __instance) {
      if (!LocationUtils.IsEnabled(__instance)) return;
      var width = LocationUtils.GetRayWidth(__instance.m_damageModifiers);
      var color = LocationUtils.GetRayColor(__instance.m_damageModifiers);
      var obj = Drawer.DrawMarkerLine(__instance, color, width, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(MineRock5), "Start")]
  public class MineRock5_Ray {
    public static void Postfix(MineRock5 __instance) {
      if (!LocationUtils.IsEnabled(__instance)) return;
      var width = LocationUtils.GetRayWidth(__instance.m_damageModifiers);
      var color = LocationUtils.GetRayColor(__instance.m_damageModifiers);
      var obj = Drawer.DrawMarkerLine(__instance, color, width, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(Destructible), "Awake")]
  public class Destructible_Ray {
    public static void Postfix(Destructible __instance) {
      if (!LocationUtils.IsEnabled(__instance)) return;
      var width = LocationUtils.GetRayWidth(__instance.m_damages);
      var color = LocationUtils.GetRayColor(__instance.m_damages);
      var obj = Drawer.DrawMarkerLine(__instance, color, width, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(TreeBase), "Awake")]
  public class TreeBase_Ray {
    public static void Postfix(TreeBase __instance) {

      if (!LocationUtils.IsEnabled(__instance)) return;
      var width = LocationUtils.GetRayWidth(__instance.m_damageModifiers);
      var color = LocationUtils.GetRayColor(__instance.m_damageModifiers);
      var obj = Drawer.DrawMarkerLine(__instance, color, width, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(TreeLog), "Awake")]
  public class TreeLog_Ray {
    public static void Postfix(TreeLog __instance) {
      if (!LocationUtils.IsEnabled(__instance)) return;
      var width = LocationUtils.GetRayWidth(__instance.m_damages);
      var color = LocationUtils.GetRayColor(__instance.m_damages);
      var obj = Drawer.DrawMarkerLine(__instance, color, width, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(CreatureSpawner), "Awake")]
  public class CreatureSpawner_Ray {
    private static bool IsEnabled(CreatureSpawner obj) {
      if (Settings.CreatureSpawnersRayWidth == 0) return false;
      return !LocationUtils.IsIn(Settings.ExcludedCreatureSpawners, obj.name);
    }
    private static Color GetColor(CreatureSpawner obj) {
      return obj.m_respawnTimeMinuts > 0f ? Settings.SpawnerRespawningColor : Settings.SpawnerOneTimeColor;
    }
    public static void Postfix(CreatureSpawner __instance) {
      var obj = __instance;
      if (!IsEnabled(obj)) return;
      var color = GetColor(obj);
      var line = Drawer.DrawMarkerLine(obj, color, Settings.CreatureSpawnersRayWidth, Drawer.OTHER);
      Drawer.AddText(line, Format.Name(obj));
    }
  }
}