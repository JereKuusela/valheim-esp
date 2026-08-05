# Permissions

ESP integrates with the permission system provided by Server Devcommands.

This is an optional feature, admins get full access to all features by default.

To use permissions for ESP:

- Install Server Devcommands on the server.
  - ESP is not needed on the server for this feature.
- Install Server Devcommands on the client that wants to use ESP.
  - Also install ESP on the client.
- Configure rules in `permissions.yaml`.

ESP checks three permission sections:

- `esp_hud`
- `esp_stats`
- `esp_visuals`

Section and feature names are case-insensitive.

For example to allow a player to use player base visualization:

```yaml
- id: Steam_XXXXXXXXXXXX
  name: Jere
  character: 930479656
  esp_visuals:
    effectareaplayerbase: yes
```

## ESP features by section

### `esp_hud`

- `hud`
- `time`
- `position`
- `altitude`
- `forest`
- `blocked`
- `stagger`
- `heat`
- `ship`
- `speed`
- `stealth`
- `weather`
- `wind`

### `esp_stats`

- `drops`
- `breeding`
- `status`
- `attacks`
- `resistances`
- `extrainfo`
- `customonly`
- `weaponinfo`
- `progress`
- `support`
- `structures`
- `creatures`
- `destructibles`
- `pickables`
- `itemdrops`
- `ships`
- `locations`
- `vegetation`

### `esp_visuals`

- `creaturecollider`
- `structurecollider`
- `destructiblecollider`
- `attack`
- `structurecover`
- `structuresupport`
- `creaturenoise`
- `creaturehearrange`
- `creatureviewrange`
- `creaturealertrange`
- `creaturefirerange`
- `creaturebreedingtotalrange`
- `creaturebreedingpartnerrange`
- `creaturefoodsearchrange`
- `creatureeatingrange`
- `trackedobject`
- `pickableonetime`
- `pickablerespawning`
- `eventzone`
- `location`
- `chest`
- `tree`
- `ore`
- `trophyspeak`
- `destructible`
- `spawnpointonetime`
- `spawnpointrespawning`
- `spawnerray`
- `spawnertriggerrange`
- `spawnernearrange`
- `spawnerspawnrange`
- `altarray`
- `altarspawnradius`
- `altaritemstandrange`
- `zonecorner`
- `spawnzone`
- `randomeventsystem`
- `effectareaspawnsuppression`
- `effectareacomfort`
- `effectareaburning`
- `effectareaheat`
- `effectareafire`
- `effectareanomonsters`
- `effectareateleport`
- `effectareaplayerbase`
- `effectareaother`
- `effectareacustomcontainer`
- `effectareacustomcrafting`
- `effectareawarmcozy`
- `smoke`
- `playercover`
- `terrain`
