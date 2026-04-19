namespace Service;

/// <summary>
/// Pre-calculated stable hash codes for ESP permission features.
/// Uses lowercase names for case-insensitive matching.
/// </summary>
public static class PermissionHash
{
  public const string Section = "esp";

  // Tooltip settings.
  public static readonly int Drops = "drops".GetStableHashCode();
  public static readonly int Breeding = "breeding".GetStableHashCode();
  public static readonly int Status = "status".GetStableHashCode();
  public static readonly int Attacks = "attacks".GetStableHashCode();
  public static readonly int Resistances = "resistances".GetStableHashCode();
  public static readonly int ExtraInfo = "extrainfo".GetStableHashCode();
  public static readonly int CustomOnly = "customonly".GetStableHashCode();
  public static readonly int WeaponInfo = "weaponinfo".GetStableHashCode();
  public static readonly int Progress = "progress".GetStableHashCode();
  public static readonly int Support = "support".GetStableHashCode();
  public static readonly int Structures = "structures".GetStableHashCode();
  public static readonly int Creatures = "creatures".GetStableHashCode();
  public static readonly int Destructibles = "destructibles".GetStableHashCode();
  public static readonly int Pickables = "pickables".GetStableHashCode();
  public static readonly int ItemDrops = "itemdrops".GetStableHashCode();
  public static readonly int ShowShipStats = "showshipstats".GetStableHashCode();
  public static readonly int Locations = "locations".GetStableHashCode();
  public static readonly int Vegetation = "vegetation".GetStableHashCode();

  // HUD settings.
  public static readonly int ShowShipStatsOnHud = "showshipstatsonhud".GetStableHashCode();
  public static readonly int Hud = "hud".GetStableHashCode();
  public static readonly int Time = "time".GetStableHashCode();
  public static readonly int Position = "position".GetStableHashCode();
  public static readonly int Altitude = "altitude".GetStableHashCode();
  public static readonly int Forest = "forest".GetStableHashCode();
  public static readonly int Blocked = "blocked".GetStableHashCode();
  public static readonly int Stagger = "stagger".GetStableHashCode();
  public static readonly int Heat = "heat".GetStableHashCode();
  public static readonly int Speed = "speed".GetStableHashCode();
  public static readonly int Stealth = "stealth".GetStableHashCode();
  public static readonly int Weather = "weather".GetStableHashCode();
  public static readonly int Wind = "wind".GetStableHashCode();

  // Visual tags.
  public static readonly int CreatureCollider = "creaturecollider".GetStableHashCode();
  public static readonly int StructureCollider = "structurecollider".GetStableHashCode();
  public static readonly int DestructibleCollider = "destructiblecollider".GetStableHashCode();
  public static readonly int Attack = "attack".GetStableHashCode();
  public static readonly int StructureCover = "structurecover".GetStableHashCode();
  public static readonly int StructureCoverBlocked = "structurecoverblocked".GetStableHashCode();
  public static readonly int StructureSupport = "structuresupport".GetStableHashCode();
  public static readonly int CreatureNoise = "creaturenoise".GetStableHashCode();
  public static readonly int CreatureHearRange = "creaturehearrange".GetStableHashCode();
  public static readonly int CreatureViewRange = "creatureviewrange".GetStableHashCode();
  public static readonly int CreatureAlertRange = "creaturealertrange".GetStableHashCode();
  public static readonly int CreatureFireRange = "creaturefirerange".GetStableHashCode();
  public static readonly int CreatureBreedingTotalRange = "creaturebreedingtotalrange".GetStableHashCode();
  public static readonly int CreatureBreedingPartnerRange = "creaturebreedingpartnerrange".GetStableHashCode();
  public static readonly int CreatureFoodSearchRange = "creaturefoodsearchrange".GetStableHashCode();
  public static readonly int CreatureEatingRange = "creatureeatingrange".GetStableHashCode();
  public static readonly int TrackedObject = "trackedobject".GetStableHashCode();
  public static readonly int PickableOneTime = "pickableonetime".GetStableHashCode();
  public static readonly int PickableRespawning = "pickablerespawning".GetStableHashCode();
  public static readonly int EventZone = "eventzone".GetStableHashCode();
  public static readonly int Location = "location".GetStableHashCode();
  public static readonly int Chest = "chest".GetStableHashCode();
  public static readonly int Tree = "tree".GetStableHashCode();
  public static readonly int Ore = "ore".GetStableHashCode();
  public static readonly int TrophySpeak = "trophyspeak".GetStableHashCode();
  public static readonly int Destructible = "destructible".GetStableHashCode();
  public static readonly int SpawnPointOneTime = "spawnpointonetime".GetStableHashCode();
  public static readonly int SpawnPointRespawning = "spawnpointrespawning".GetStableHashCode();
  public static readonly int SpawnerRay = "spawnerray".GetStableHashCode();
  public static readonly int SpawnerTriggerRange = "spawnertriggerrange".GetStableHashCode();
  public static readonly int SpawnerLimitRange = "spawnernearrange".GetStableHashCode();
  public static readonly int SpawnerSpawnRange = "spawnerspawnrange".GetStableHashCode();
  public static readonly int AltarRay = "altarray".GetStableHashCode();
  public static readonly int AltarSpawnRadius = "altarspawnradius".GetStableHashCode();
  public static readonly int AltarItemStandRange = "altaritemstandrange".GetStableHashCode();
  public static readonly int ZoneCorner = "zonecorner".GetStableHashCode();
  public static readonly int ZoneCornerAshlands = "zonecornerashlands".GetStableHashCode();
  public static readonly int ZoneCornerBlackForest = "zonecornerblackforest".GetStableHashCode();
  public static readonly int ZoneCornerDeepNorth = "zonecornerdeepnorth".GetStableHashCode();
  public static readonly int ZoneCornerMeadows = "zonecornermeadows".GetStableHashCode();
  public static readonly int ZoneCornerMistlands = "zonecornermistlands".GetStableHashCode();
  public static readonly int ZoneCornerMountain = "zonecornermountain".GetStableHashCode();
  public static readonly int ZoneCornerOcean = "zonecornerocean".GetStableHashCode();
  public static readonly int ZoneCornerPlains = "zonecornerplains".GetStableHashCode();
  public static readonly int ZoneCornerSwamp = "zonecornerswamp".GetStableHashCode();
  public static readonly int ZoneCornerUnknown = "zonecornerunknown".GetStableHashCode();
  public static readonly int SpawnZone = "spawnzone".GetStableHashCode();
  public static readonly int SpawnZoneAshlands = "spawnzoneashlands".GetStableHashCode();
  public static readonly int SpawnZoneBlackForest = "spawnzoneblackforest".GetStableHashCode();
  public static readonly int SpawnZoneDeepNorth = "spawnzonedeepnorth".GetStableHashCode();
  public static readonly int SpawnZoneMeadows = "spawnzonemeadows".GetStableHashCode();
  public static readonly int SpawnZoneMistlands = "spawnzonemistlands".GetStableHashCode();
  public static readonly int SpawnZoneMountain = "spawnzonemountain".GetStableHashCode();
  public static readonly int SpawnZoneOcean = "spawnzoneocean".GetStableHashCode();
  public static readonly int SpawnZonePlains = "spawnzoneplains".GetStableHashCode();
  public static readonly int SpawnZoneSwamp = "spawnzoneswamp".GetStableHashCode();
  public static readonly int SpawnZoneUnknown = "spawnzoneunknown".GetStableHashCode();
  public static readonly int RandomEventSystem = "randomeventsystem".GetStableHashCode();
  public static readonly int EffectAreaPrivateArea = "effectareaprivatearea".GetStableHashCode();
  public static readonly int EffectAreaComfort = "effectareacomfort".GetStableHashCode();
  public static readonly int EffectAreaBurning = "effectareaburning".GetStableHashCode();
  public static readonly int EffectAreaHeat = "effectareaheat".GetStableHashCode();
  public static readonly int EffectAreaFire = "effectareafire".GetStableHashCode();
  public static readonly int EffectAreaNoMonsters = "effectareanomonsters".GetStableHashCode();
  public static readonly int EffectAreaTeleport = "effectareateleport".GetStableHashCode();
  public static readonly int EffectAreaPlayerBase = "effectareaplayerbase".GetStableHashCode();
  public static readonly int EffectAreaOther = "effectareaother".GetStableHashCode();
  public static readonly int EffectAreaCustomContainer = "effectareacustomcontainer".GetStableHashCode();
  public static readonly int EffectAreaCustomCrafting = "effectareacustomcrafting".GetStableHashCode();
  public static readonly int EffectAreaWarmCozy = "effectareawarmcozy".GetStableHashCode();
  public static readonly int Smoke = "smoke".GetStableHashCode();
  public static readonly int PlayerCover = "playercover".GetStableHashCode();
  public static readonly int PlayerCoverBlocked = "playercoverblocked".GetStableHashCode();
  public static readonly int Terrain = "terrain".GetStableHashCode();
}