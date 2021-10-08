using System;
using BepInEx.Configuration;
using Visualization;

namespace ESP {
  public partial class Settings {
    public static ConfigEntry<bool> configShowCreatureFireRange;
    public static ConfigEntry<bool> configShowTrackedCreatures;
    public static ConfigEntry<bool> configShowCreatureBreedingTotalRange;
    public static ConfigEntry<bool> configShowCreatureBreedingPartnerRange;
    public static ConfigEntry<bool> configShowCreatureFoodSearchRange;
    public static ConfigEntry<bool> configShowCreatureEatingRange;
    public static ConfigEntry<bool> configShowStructureCover;
    public static ConfigEntry<bool> configShowPlayerCover;
    public static ConfigEntry<bool> configShowSmoke;
    public static ConfigEntry<bool> configShowCreatureAlertRange;
    public static ConfigEntry<bool> configShowCreatureNoise;
    public static ConfigEntry<bool> configShowCreatureHearRange;
    public static ConfigEntry<bool> configShowCreatureViewRange;
    public static ConfigEntry<bool> configShowSpawnZones;
    public static ConfigEntry<bool> configShowRandomEventSystem;
    public static ConfigEntry<bool> configShowSpawners;
    public static ConfigEntry<bool> configShowSpawnPoints;
    public static ConfigEntry<bool> configShowZoneCorners;
    public static ConfigEntry<bool> configShowPickables;
    public static ConfigEntry<bool> configShowEffectAreas;
    public static ConfigEntry<bool> configShowChests;
    public static ConfigEntry<bool> configShowOres;
    public static ConfigEntry<bool> configShowTrees;
    public static ConfigEntry<bool> configShowDestrucibles;
    public static ConfigEntry<bool> configShowLocations;
    public static ConfigEntry<bool> configShowStructureSupport;
    private static void SetTag(string name, bool value) {
      var entry = GetTagEntry(name);
      if (entry.Value != value)
        entry.Value = value;
    }
    private static void ToggleTag(string name) {
      var entry = GetTagEntry(name);
      entry.Value = !entry.Value;
    }
    private static void SetGroup(string name, bool value) {
      var entry = GetGroupEntry(name);
      if (entry.Value != value)
        entry.Value = value;
    }
    private static void ToggleGroup(string name) {
      var entry = GetGroupEntry(name);
      entry.Value = !entry.Value;
    }
    private static ConfigEntry<bool> GetTagEntry(string name) {
      name = name.ToLower();
      if (name == Tag.StructureCover.ToLower()) return configShowStructureCover;
      if (name == Tag.StructureSupport.ToLower()) return configShowStructureSupport;
      if (name == Tag.CreatureNoise.ToLower()) return configShowCreatureNoise;
      if (name == Tag.CreatureHearRange.ToLower()) return configShowCreatureHearRange;
      if (name == Tag.CreatureViewRange.ToLower()) return configShowCreatureViewRange;
      if (name == Tag.CreatureAlertRange.ToLower()) return configShowCreatureAlertRange;
      if (name == Tag.CreatureFireRange.ToLower()) return configShowCreatureFireRange;
      if (name == Tag.CreatureBreedingTotalRange.ToLower()) return configShowCreatureBreedingTotalRange;
      if (name == Tag.CreatureBreedingPartnerRange.ToLower()) return configShowCreatureBreedingPartnerRange;
      if (name == Tag.CreatureFoodSearchRange.ToLower()) return configShowCreatureFoodSearchRange;
      if (name == Tag.CreatureEatingRange.ToLower()) return configShowCreatureEatingRange;
      if (name == Tag.TrackedCreature.ToLower()) return configShowTrackedCreatures;
      if (name == Tag.Pickable.ToLower()) return configShowPickables;
      if (name == Tag.Location.ToLower()) return configShowLocations;
      if (name == Tag.Chest.ToLower()) return configShowChests;
      if (name == Tag.Tree.ToLower()) return configShowTrees;
      if (name == Tag.Ore.ToLower()) return configShowOres;
      if (name == Tag.Destructible.ToLower()) return configShowDestrucibles;
      if (name == Tag.SpawnPoint.ToLower()) return configShowSpawnPoints;
      if (name == Tag.Spawner.ToLower()) return configShowSpawners;
      if (name == Tag.ZoneCorner.ToLower()) return configShowZoneCorners;
      if (name == Tag.SpawnZone.ToLower()) return configShowSpawnZones;
      if (name == Tag.RandomEventSystem.ToLower()) return configShowRandomEventSystem;
      if (name == Tag.EffectArea.ToLower()) return configShowEffectAreas;
      if (name == Tag.Smoke.ToLower()) return configShowSmoke;
      if (name == Tag.PlayerCover.ToLower()) return configShowPlayerCover;
      throw new NotImplementedException();
    }
    private static ConfigEntry<bool> GetGroupEntry(string name) {
      name = name.ToLower();
      if (name == Group.Creature.ToLower()) return configShowCreatures;
      if (name == Group.Zone.ToLower()) return configShowZones;
      if (name == Group.Other.ToLower()) return configShowOthers;
      throw new NotImplementedException();
    }
    private static void UpdateTag(string name) {
      var entry = GetTagEntry(name);
      if (entry == null) UnityEngine.Debug.LogError("Setting not initialized: " + name);
      Visibility.SetTag(name, entry.Value);
    }
    private static void UpdateGroup(string name) {
      var entry = GetGroupEntry(name);
      if (entry == null) UnityEngine.Debug.LogError("Setting not initialized: " + name);
      Visibility.SetGroup(name, entry.Value);
    }
    private static void InitVisuals(ConfigFile config) {
      var section = "4. Visuals";
      configShowZones = config.Bind(section, "Show zones", false, "Show visualization for zones (toggle with Y button in the game)");
      configShowZones.SettingChanged += (s, e) => UpdateGroup(Group.Zone);
      UpdateGroup(Group.Zone);
      configShowCreatures = config.Bind(section, "Show creatures", false, "Show visualization for creatures (toggle with U button in the game)");
      configShowCreatures.SettingChanged += (s, e) => UpdateGroup(Group.Creature);
      UpdateGroup(Group.Creature);
      configShowOthers = config.Bind(section, "Show visualization", false, "Show visualization for everything else (toggle with I button in the game)");
      configShowOthers.SettingChanged += (s, e) => UpdateGroup(Group.Other);
      UpdateGroup(Group.Other);

      configShowStructureCover = config.Bind(section, "Structure cover", true, "");
      configShowStructureCover.SettingChanged += (s, e) => {
        Visibility.SetGroup(Tag.StructureCover, configShowStructureCover.Value);
        SupportUtils.UpdateVisibility();
      };
      UpdateTag(Tag.StructureCover);
      configShowStructureSupport = config.Bind(section, "Structure support", true, "");
      configShowStructureSupport.SettingChanged += (s, e) => UpdateTag(Tag.StructureSupport);
      UpdateTag(Tag.StructureSupport);
      configShowCreatureNoise = config.Bind(section, "Creature noise", true, "");
      configShowCreatureNoise.SettingChanged += (s, e) => UpdateTag(Tag.CreatureNoise);
      UpdateTag(Tag.CreatureNoise);
      configShowCreatureHearRange = config.Bind(section, "Creature hear range", true, "");
      configShowCreatureHearRange.SettingChanged += (s, e) => UpdateTag(Tag.CreatureHearRange);
      UpdateTag(Tag.CreatureHearRange);
      configShowCreatureViewRange = config.Bind(section, "Creature vuew range", true, "");
      configShowCreatureViewRange.SettingChanged += (s, e) => UpdateTag(Tag.CreatureViewRange);
      UpdateTag(Tag.CreatureViewRange);
      configShowCreatureAlertRange = config.Bind(section, "Creature alert range", true, "");
      configShowCreatureAlertRange.SettingChanged += (s, e) => UpdateTag(Tag.CreatureAlertRange);
      UpdateTag(Tag.CreatureAlertRange);
      configShowCreatureFireRange = config.Bind(section, "Creature fire range", true, "");
      configShowCreatureFireRange.SettingChanged += (s, e) => UpdateTag(Tag.CreatureFireRange);
      UpdateTag(Tag.CreatureFireRange);
      configShowCreatureBreedingTotalRange = config.Bind(section, "Creature breeding total range", true, "");
      configShowCreatureBreedingTotalRange.SettingChanged += (s, e) => UpdateTag(Tag.CreatureBreedingTotalRange);
      UpdateTag(Tag.CreatureBreedingTotalRange);
      configShowCreatureBreedingPartnerRange = config.Bind(section, "Creature breeding partner range", true, "");
      configShowCreatureBreedingPartnerRange.SettingChanged += (s, e) => UpdateTag(Tag.CreatureBreedingPartnerRange);
      UpdateTag(Tag.CreatureBreedingPartnerRange);
      configShowCreatureEatingRange = config.Bind(section, "Creature eating range", true, "");
      configShowCreatureEatingRange.SettingChanged += (s, e) => UpdateTag(Tag.CreatureEatingRange);
      UpdateTag(Tag.CreatureEatingRange);
      configShowCreatureFoodSearchRange = config.Bind(section, "Creature food search range", true, "");
      configShowCreatureFoodSearchRange.SettingChanged += (s, e) => UpdateTag(Tag.CreatureFoodSearchRange);
      UpdateTag(Tag.CreatureFoodSearchRange);
      configShowTrackedCreatures = config.Bind(section, "Tracked creature rays", true, "");
      configShowTrackedCreatures.SettingChanged += (s, e) => UpdateTag(Tag.TrackedCreature);
      UpdateTag(Tag.TrackedCreature);
      configShowPickables = config.Bind(section, "Pickable rays", true, "");
      configShowPickables.SettingChanged += (s, e) => UpdateTag(Tag.Pickable);
      UpdateTag(Tag.Pickable);
      configShowLocations = config.Bind(section, "Location rays", true, "");
      configShowLocations.SettingChanged += (s, e) => UpdateTag(Tag.Location);
      UpdateTag(Tag.Location);
      configShowChests = config.Bind(section, "Chest rays", true, "");
      configShowChests.SettingChanged += (s, e) => UpdateTag(Tag.Chest);
      UpdateTag(Tag.Chest);
      configShowTrees = config.Bind(section, "Tree rays", true, "");
      configShowTrees.SettingChanged += (s, e) => UpdateTag(Tag.Tree);
      UpdateTag(Tag.Tree);
      configShowOres = config.Bind(section, "Ore rays", true, "");
      configShowOres.SettingChanged += (s, e) => UpdateTag(Tag.Ore);
      UpdateTag(Tag.Ore);
      configShowDestrucibles = config.Bind(section, "Destructible rays", true, "");
      configShowDestrucibles.SettingChanged += (s, e) => UpdateTag(Tag.Destructible);
      UpdateTag(Tag.Destructible);
      configShowSpawnPoints = config.Bind(section, "Spawn points", true, "");
      configShowSpawnPoints.SettingChanged += (s, e) => UpdateTag(Tag.SpawnPoint);
      UpdateTag(Tag.SpawnPoint);
      configShowSpawners = config.Bind(section, "Creature spawners", true, "");
      configShowSpawners.SettingChanged += (s, e) => UpdateTag(Tag.Spawner);
      UpdateTag(Tag.Spawner);
      configShowSpawnZones = config.Bind(section, "Spawn zones", true, "");
      configShowSpawnZones.SettingChanged += (s, e) => UpdateTag(Tag.SpawnZone);
      UpdateTag(Tag.SpawnZone);
      configShowZoneCorners = config.Bind(section, "Zone corner rays", true, "");
      configShowZoneCorners.SettingChanged += (s, e) => UpdateTag(Tag.ZoneCorner);
      UpdateTag(Tag.ZoneCorner);
      configShowRandomEventSystem = config.Bind(section, "Random event system", true, "");
      configShowRandomEventSystem.SettingChanged += (s, e) => UpdateTag(Tag.RandomEventSystem);
      UpdateTag(Tag.RandomEventSystem);
      configShowEffectAreas = config.Bind(section, "Area effects", true, "");
      configShowEffectAreas.SettingChanged += (s, e) => UpdateTag(Tag.EffectArea);
      UpdateTag(Tag.EffectArea);
      configShowSmoke = config.Bind(section, "Smoke", true, "");
      configShowSmoke.SettingChanged += (s, e) => UpdateTag(Tag.Smoke);
      UpdateTag(Tag.Smoke);
      configShowPlayerCover = config.Bind(section, "Player cover", true, "");
      configShowPlayerCover.SettingChanged += (s, e) => UpdateTag(Tag.PlayerCover);
      UpdateTag(Tag.PlayerCover);
    }
  }
}
