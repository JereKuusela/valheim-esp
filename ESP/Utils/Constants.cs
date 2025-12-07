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
    if (biome == Heightmap.Biome.AshLands) return ZoneCornerAshlands;
    if (biome == Heightmap.Biome.BlackForest) return ZoneCornerBlackForest;
    if (biome == Heightmap.Biome.DeepNorth) return ZoneCornerDeepNorth;
    if (biome == Heightmap.Biome.Meadows) return ZoneCornerMeadows;
    if (biome == Heightmap.Biome.Mistlands) return ZoneCornerMistlands;
    if (biome == Heightmap.Biome.Mountain) return ZoneCornerMountain;
    if (biome == Heightmap.Biome.Ocean) return ZoneCornerOcean;
    if (biome == Heightmap.Biome.Plains) return ZoneCornerPlains;
    if (biome == Heightmap.Biome.Swamp) return ZoneCornerSwamp;
    return ZoneCornerUnknown;
  }
  public static string GetSpawnZone(Heightmap.Biome biome)
  {
    if (biome == Heightmap.Biome.AshLands) return SpawnZoneAshlands;
    if (biome == Heightmap.Biome.BlackForest) return SpawnZoneBlackForest;
    if (biome == Heightmap.Biome.DeepNorth) return SpawnZoneDeepNorth;
    if (biome == Heightmap.Biome.Meadows) return SpawnZoneMeadows;
    if (biome == Heightmap.Biome.Mistlands) return SpawnZoneMistlands;
    if (biome == Heightmap.Biome.Mountain) return SpawnZoneMountain;
    if (biome == Heightmap.Biome.Ocean) return SpawnZoneOcean;
    if (biome == Heightmap.Biome.Plains) return SpawnZonePlains;
    if (biome == Heightmap.Biome.Swamp) return SpawnZoneSwamp;
    return SpawnZoneUnknown;
  }
  public static string GetEffectArea(EffectArea.Type type)
  {
    if ((type & EffectArea.Type.Burning) != 0) return EffectAreaBurning;
    if ((type & EffectArea.Type.Heat) != 0) return EffectAreaHeat;
    if ((type & EffectArea.Type.Fire) != 0) return EffectAreaFire;
    if ((type & EffectArea.Type.NoMonsters) != 0) return EffectAreaNoMonsters;
    if ((type & EffectArea.Type.Teleport) != 0) return EffectAreaTeleport;
    if ((type & EffectArea.Type.PlayerBase) != 0) return EffectAreaPlayerBase;
    if ((type & EffectArea.Type.WarmCozyArea) != 0) return EffectAreaWarmCozy;
    return EffectAreaOther;
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
  public const string TrackedObject = "TrackedObject";
  public const string PickableOneTime = "PickableOneTime";
  public const string PickableRespawning = "PickableRespawning";
  public const string EventZone = "EventZone";
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
  public const string EffectAreaPrivateArea = "EffectAreaSpawnSuppression";
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
  public const string Time = "Time";
  public const string Weather = "Weather";
  public const string Wind = "Wind";
  public const string Position = "Position";
  public const string Altitude = "Altitude";
  public const string Forest = "Forest";
  public const string Blocked = "Blocked";
  public const string HUD = "HUD";
  public const string Stagger = "Stagger";
  public const string Heat = "Heat";
  public const string Speed = "Speed";
  public const string Stealth = "Stealth";
}
