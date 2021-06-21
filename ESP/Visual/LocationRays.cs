using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public class LocationUtils
  {
    public static float GetRayWidth(HitData.DamageModifiers modifiers)
    {
      if (modifiers.m_chop == HitData.DamageModifier.Immune) return Settings.oreRayWidth;
      if (modifiers.m_pickaxe == HitData.DamageModifier.Immune) return Settings.treeRayWidth;
      return Settings.destructibleRayWidth;
    }
  }
  [HarmonyPatch(typeof(BaseAI), "Awake")]
  public class BaseAI_Ray
  {
    public static void Postfix(Character ___m_character)
    {
      var obj = ___m_character;
      if (Settings.creatureRayWidth == 0 || CharacterUtils.IsExcluded(obj)) return;
      var line = Drawer.DrawMarkerLine(obj.gameObject, Color.magenta, Settings.creatureRayWidth, Drawer.CREATURE);
      Drawer.AddText(line);
    }
  }
  [HarmonyPatch(typeof(Pickable), "Awake")]
  public class Pickable_Ray
  {
    private static bool IsEnabled(Pickable instance)
    {
      if (Settings.pickableRayWidth == 0) return false;
      var name = instance.m_itemPrefab.name.ToLower();
      var excluded = Settings.excludedPickables.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return false;
      return true;
    }
    private static Color GetColor(Pickable instance)
    {
      return instance.m_hideWhenPicked && instance.m_respawnTimeMinutes > 0 ? Color.green : Color.blue;
    }
    public static void Postfix(Pickable __instance, ZNetView ___m_nview)
    {
      if (!IsEnabled(__instance))
        return;
      var color = GetColor(__instance);
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, color, Settings.pickableRayWidth, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(Location), "Awake")]
  public class Location_Ray
  {
    public static void Postfix(Location __instance)
    {
      if (Settings.locationRayWidth == 0)
        return;
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, Color.black, Settings.locationRayWidth, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(Container), "Awake")]
  public class Container_Ray
  {
    public static void Postfix(Container __instance, Piece ___m_piece)
    {
      if (Settings.chestRayWidth == 0 || !___m_piece || ___m_piece.IsPlacedByPlayer()) return;
      var text = Format.String(__instance.GetHoverName());
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, Color.white, Settings.chestRayWidth, Drawer.OTHER);
      Drawer.AddText(obj, text);
    }
  }
  [HarmonyPatch(typeof(MineRock), "Start")]
  public class MineRock_Ray
  {
    public static void Postfix(MineRock __instance)
    {
      var width = LocationUtils.GetRayWidth(__instance.m_damageModifiers);
      if (width == 0) return;
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, Color.gray, width, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(MineRock5), "Start")]
  public class MineRock5_Ray
  {
    public static void Postfix(MineRock5 __instance)
    {
      var width = LocationUtils.GetRayWidth(__instance.m_damageModifiers);
      if (width == 0) return;
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, Color.gray, width, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(Destructible), "Awake")]
  public class Destructible_Ray
  {
    public static void Postfix(Destructible __instance)
    {
      var width = LocationUtils.GetRayWidth(__instance.m_damages);
      if (width == 0) return;
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, Color.gray, width, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(TreeBase), "Awake")]
  public class TreeBase_Ray
  {
    public static void Postfix(TreeBase __instance)
    {
      var width = LocationUtils.GetRayWidth(__instance.m_damageModifiers);
      if (width == 0) return;
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, Color.gray, width, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(TreeLog), "Awake")]
  public class TreeLog_Ray
  {
    public static void Postfix(TreeLog __instance)
    {
      var width = LocationUtils.GetRayWidth(__instance.m_damages);
      if (width == 0) return;
      var obj = Drawer.DrawMarkerLine(__instance.gameObject, Color.gray, width, Drawer.OTHER);
      Drawer.AddText(obj, Format.Name(__instance));
    }
  }
  [HarmonyPatch(typeof(CreatureSpawner), "Awake")]
  public class CreatureSpawner_Ray
  {
    private static bool IsEnabled(CreatureSpawner obj)
    {
      if (Settings.creatureSpawnersRayWidth == 0) return false;
      var name = obj.name.ToLower();
      var excluded = Settings.excludedCreatureSpawners.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return false;
      return true;
    }
    private static Color GetColor(CreatureSpawner obj)
    {
      return obj.m_respawnTimeMinuts > 0f ? Color.yellow : Color.red;
    }
    public static void Postfix(CreatureSpawner __instance)
    {
      var obj = __instance;
      if (!IsEnabled(obj)) return;
      var color = GetColor(obj);
      var line = Drawer.DrawMarkerLine(obj.gameObject, color, Settings.creatureSpawnersRayWidth, Drawer.OTHER);
      Drawer.AddText(line, Format.Name(obj));
    }
  }
}