namespace ESP;
public static class Constants
{
  public const float CoverRayCastLength = 30f;
  public const float CoverRaycastStart = 0.5f;
  public const float CoverBedLimit = 0.8f;
  public const float CoverCraftingStationLimit = 0.7f;
  public const float CoverFermenterLimit = 0.7f;
  public const float CoverFireplaceLimit = 0.7f;
  public const float WindFireplaceLimit = 0.8f;
  public const float RoofFireplaceLimit = 0.5f;
  public const float CoverPlayerLimit = 0.8f;
  public const float SmokeAmountLimit = 100;
  public const float ComfortRadius = 10f;
  public const float CreatureFireLimitRadius = 3f;
}
public static class Tag
{
  public static string GetZoneCorner(Heightmap.Biome biome)
  {
    if (biome == Heightmap.Biome.AshLands) return Tag.ZoneCornerAshlands;
    if (biome == Heightmap.Biome.BlackForest) return Tag.ZoneCornerBlackForest;
    if (biome == Heightmap.Biome.DeepNorth) return Tag.ZoneCornerDeepNorth;
    if (biome == Heightmap.Biome.Meadows) return Tag.ZoneCornerMeadows;
    if (biome == Heightmap.Biome.Mistlands) return Tag.ZoneCornerMistlands;
    if (biome == Heightmap.Biome.Mountain) return Tag.ZoneCornerMountain;
    if (biome == Heightmap.Biome.Ocean) return Tag.ZoneCornerOcean;
    if (biome == Heightmap.Biome.Plains) return Tag.ZoneCornerPlains;
    if (biome == Heightmap.Biome.Swamp) return Tag.ZoneCornerSwamp;
    return Tag.ZoneCornerUnknown;
  }
  public static string GetSpawnZone(Heightmap.Biome biome)
  {
    if (biome == Heightmap.Biome.AshLands) return Tag.SpawnZoneAshlands;
    if (biome == Heightmap.Biome.BlackForest) return Tag.SpawnZoneBlackForest;
    if (biome == Heightmap.Biome.DeepNorth) return Tag.SpawnZoneDeepNorth;
    if (biome == Heightmap.Biome.Meadows) return Tag.SpawnZoneMeadows;
    if (biome == Heightmap.Biome.Mistlands) return Tag.SpawnZoneMistlands;
    if (biome == Heightmap.Biome.Mountain) return Tag.SpawnZoneMountain;
    if (biome == Heightmap.Biome.Ocean) return Tag.SpawnZoneOcean;
    if (biome == Heightmap.Biome.Plains) return Tag.SpawnZonePlains;
    if (biome == Heightmap.Biome.Swamp) return Tag.SpawnZoneSwamp;
    return Tag.SpawnZoneUnknown;
  }
  public static string GetEffectArea(EffectArea.Type type)
  {
    if ((type & EffectArea.Type.Burning) != 0) return Tag.EffectAreaBurning;
    if ((type & EffectArea.Type.Heat) != 0) return Tag.EffectAreaHeat;
    if ((type & EffectArea.Type.Fire) != 0) return Tag.EffectAreaFire;
    if ((type & EffectArea.Type.NoMonsters) != 0) return Tag.EffectAreaNoMonsters;
    if ((type & EffectArea.Type.Teleport) != 0) return Tag.EffectAreaTeleport;
    if ((type & EffectArea.Type.PlayerBase) != 0) return Tag.EffectAreaPlayerBase;
    if ((type & EffectArea.Type.WarmCozyArea) != 0) return Tag.EffectAreaWarmCozy;
    return Tag.EffectAreaOther;
  }
  public const string CreatureCollider = "CreatureCollider";
  public const string StructureCollider = "StructureCollider";
  public const string DestructibleCollider = "DestructibleCollider";
  public const string Attack = "Attack";
  public const string StructureCover = "StructureCover";
  public const string StructureCoverBlocked = "StructureCoverBlocked";
  public const string StructureSupport = "StructureSupport";
  public const string CreatureNoise = "CreatureNoise";
  public const string CreatureHearRange = "CreatureHearRange";
  public const string CreatureViewRange = "CreatureViewRange";
  public const string CreatureAlertRange = "CreatureAlertRange";
  public const string CreatureFireRange = "CreatureFireRange";
  public const string CreatureBreedingTotalRange = "CreatureBreedingTotalRange";
  public const string CreatureBreedingPartnerRange = "CreatureBreedingPartnerRange";
  public const string CreatureFoodSearchRange = "CreatureFoodSearchRange";
  public const string CreatureEatingRange = "CreatureEatingRange";
  public const string TrackedCreature = "TrackedCreature";
  public const string PickableOneTime = "PickableOneTime";
  public const string PickableRespawning = "PickableRespawning";
  public const string Location = "Location";
  public const string Chest = "Chest";
  public const string Tree = "Tree";
  public const string Ore = "Ore";
  public const string TrophySpeak = "TrophySpeak";
  public const string Destructible = "Destructible";
  public const string SpawnPointOneTime = "SpawnPointOneTime";
  public const string SpawnPointRespawning = "SpawnPointRespawning";
  public const string SpawnerRay = "SpawnerRay";
  public const string SpawnerTriggerRange = "SpawnerTriggerRange";
  public const string SpawnerLimitRange = "SpawnerNearRange";
  public const string SpawnerSpawnRange = "SpawnerSpawnRange";
  public const string AltarRay = "AltarRay";
  public const string AltarSpawnRadius = "AltarSpawnRadius";
  public const string AltarItemStandRange = "AltarItemStandRange";
  public const string ZoneCorner = "ZoneCorner";
  public const string ZoneCornerAshlands = "ZoneCornerAshlands";
  public const string ZoneCornerBlackForest = "ZoneCornerBlackForest";
  public const string ZoneCornerDeepNorth = "ZoneCornerDeepNorth";
  public const string ZoneCornerMeadows = "ZoneCornerMeadows";
  public const string ZoneCornerMistlands = "ZoneCornerMistlands";
  public const string ZoneCornerMountain = "ZoneCornerMountain";
  public const string ZoneCornerOcean = "ZoneCornerOcean";
  public const string ZoneCornerPlains = "ZoneCornerPlains";
  public const string ZoneCornerSwamp = "ZoneCornerSwamp";
  public const string ZoneCornerUnknown = "ZoneCornerUnknown";
  public const string SpawnZone = "SpawnZone";
  public const string SpawnZoneAshlands = "SpawnZoneAshlands";
  public const string SpawnZoneBlackForest = "SpawnZoneBlackForest";
  public const string SpawnZoneDeepNorth = "SpawnZoneDeepNorth";
  public const string SpawnZoneMeadows = "SpawnZoneMeadows";
  public const string SpawnZoneMistlands = "SpawnZoneMistlands";
  public const string SpawnZoneMountain = "SpawnZoneMountain";
  public const string SpawnZoneOcean = "SpawnZoneOcean";
  public const string SpawnZonePlains = "SpawnZonePlains";
  public const string SpawnZoneSwamp = "SpawnZoneSwamp";
  public const string SpawnZoneUnknown = "SpawnZoneUnknown";
  public const string RandomEventSystem = "RandomEventSystem";
  public const string EffectAreaPrivateArea = "EffectAreaPrivateArea";
  public const string EffectAreaComfort = "EffectAreaComfort";
  public const string EffectAreaBurning = "EffectAreaBurning";
  public const string EffectAreaHeat = "EffectAreaHeat";
  public const string EffectAreaFire = "EffectAreaFire";
  public const string EffectAreaNoMonsters = "EffectAreaNoMonsters";
  public const string EffectAreaTeleport = "EffectAreaTeleport";
  public const string EffectAreaPlayerBase = "EffectAreaPlayerBase";
  public const string EffectAreaOther = "EffectAreaOther";
  public const string EffectAreaCustomContainer = "EffectAreaCustomContainer";
  public const string EffectAreaCustomCrafting = "EffectAreaCustomCrafting";
  public const string EffectAreaWarmCozy = "EffectAreaWarmCozy";
  public const string Smoke = "Smoke";
  public const string PlayerCover = "PlayerCover";
  public const string PlayerCoverBlocked = "PlayerCoverBlocked";
  public const string Terrain = "Terrain";
}
public static class Tool
{
  public const string ExtraInfo = "ExtraInfo";
  public const string TimeAndWeather = "TimeAndWeather";
  public const string Position = "Position";
  public const string HUD = "HUD";
}
