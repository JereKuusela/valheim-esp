# ESP

This mod adds lots of new information to tooltips and visualizes many hidden mechanics which can be used to improve your gameplay and perfect your build.

Works on dedicated servers for server admins.

Some use cases:

- Perfecting your builds with a better understanding of structure support, structure health, cover and smoke mechanics.
- Spawn proofing your bases with the player base effect visualized.
- Finding the best ways to deal with tougher enemies by knowing their resistances and attacks.
- Getting deeper insight of exact mechanics and interaction by looking at the detailed visualizations and tooltips.

# Manual Installation

1. Install the [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)
2. Download the latest zip.
3. Extract it in the \<GameDirectory\>\BepInEx\plugins\ folder.
4. Recommended to also install the [Configuration manager](https://github.com/BepInEx/BepInEx.ConfigurationManager/releases/tag/v16.4) for easier configuring.

Note: For all the mods I'm working on, this has the lowest priority and probably won't get update for a while.

# Configuration

By default, most features are active which adds too much information for most use cases. It's recommended to turn off most features, until you have a better idea what you are looking for.

The best way is to use the configuration manager since it provides a decent UI and works during the game. Three values are possible for features:

- 1: Feature is enabled.
- 0: Feature is hidden. Enabling the feature will instantly make it visible.
- -1: Feature is disabled. Enabling the feature may not appear until the area/object is reloaded.

Another way is to use the commands `esp_toggle`, `esp_enable` and `esp_disable`.

- `esp_toggle`: Switches the value between 0 and 1.
- `esp_enable`: Sets the value to 1.
- `esp_disable`: Sets the value to -1.

However once you know what you need, you should bind the `esp_toggle` command to a key to quick turn features on/off. For example: `bind o esp_toggle HUD ExtraInfo` would toggle the HUD and extra hover texts when pressing O button.

Parameter `*` affects all settings. For example `esp_disable *` hides all features.

Unfortunately the feature names for commands aren't documented yet (but the commands support autocomplete).

# Features

## Structures

- Structure stability color for all structures.
- Exact stability values (including the material type).
- Health and damage resistances.
- Cover system visualized (green line for uncovered, red line for covered).
- Cover limit and current cover.
- Wind limit on fireplaces and torches (extinguishes the fire unless properly covered).
- Smoke visualized (black sphere):
	- Radius of the smoke.
	- Current and target velocity.
	- Mass.
	- Timer for destruction.
- Smoke stats on structures that generate smoke.
- Blocked by smoke timer on fireplaces.
- Comfort range visualized (cyan sphere).
- Comfort amount and category.
- Player base range visualized (white sphere).
- Ward protection range visualized (gray sphere).
- Burning, fire and warmth range visualized (yellow, red and magenta spheres).
- Custom visualization for containers and crafting stations (if using for example a "craft from containers" mod).
- Fuel amount of fireplace, torches and smelters.
- Progression of beehives, smelters, kilns and windmills.

## Gathering / Exploration

- Growth timer, health and damage resistances of plants.
- Visual indicator for pickables (green line when respawning, blue line when one time, stones and branches are turned off by default):
	- Respawn timer.
	- World generator parameters.
- Visual indicator for hidden chests (white line).
	- Possible chest items and their chances.
- Visual indicator for pregenerated structures (black line).
	- World generator parameters.
- Visual indicator for trees, minerals and other destructibles (gray line, turned off by default):
	- Current and max health.
	- Damage resistances.
	- Required tool tiers.
	- Noise created when hit.
	- Created object when destroyed.
	- Item drops with amounts and chances.
	- World generator parameters.
- Bounding boxes for mineral/stone support system to locate parts keeping the deposit from collapsing.
- Stack size and despawn timer on dropped items.
- Ship speed (both current and average), wind angle and wind strength on the HUD when sailing.

## Creatures

- Tracker to count amount of creatures in nearby areas (by default tracks Serpents).
- Visual indicator for tracked creatures to make detecting them easier (magenta line).
- Hearing range, unless infinite (green sphere).
- Vision range and angle (white cone).
- Alert range (red cone).
- Fire check range for creatures that avoid or are afraid of fires (magenta sphere).
- Breeding limit range (cyan sphere) and partner check range for breedable creatures (magenta sphere).
- Food search check range (gray sphere) and eating range for tameable creatures (white sphere).
- Lots of stats on tooltips:
	- Status: is alerted, is in hunt mode, is staggering, is sleeping and current action.
	- Current and max health.
	- Accumulated stagger and stagger limit.
	- Mass and knockback resistance.
	- Damage resistances.
	- Item drops.
	- Attacks with damage, tool tier, range, hitbox and cooldown.
	- Food timer.
	- Breeding progress.
	- Breeding limit.
	- Wake up range and noise.

## Environment

- Time of the day, weather, wind, coordinates, altitude and forest factor on the HUD.
- Coordinates on most tooltips.
- Zone corners are visualized with the color of the biome:
	- Biome, time of the day, weather, wind and average wind on zone corner tooltips.
	- Avalaible weathers including their chances, wind limits and other properties.
	- Timer for the next weather change.
- Random event system at middle of zones (black line):
	- Timer for the next event check.
	- Available events including their conditions. Grey color shows failed conditions and events that are not currently possible.
	- If an event is going, shows event name, remaining time and event spawners.

## Creature spawning

- Zone based spawners at middle of zones (color of the line depends on the biome):
	- Name of the creature, spawn limit, max stars.
	- Required global keys (boss kills), biome, time of the day, weather and altitude.
	- Timer for the next spawn attemp.
	- Amount of creatures spawned.
- Zone based event spawners at middle of zones (black line):
	- If no event going, shows available events.
- Creature spawn points visualized (yellow for respawning, red for one time).
	- Respawn timer.
	- Max stars.
	- Is a patrol point (creatures tries to return if it gets too far).
	- Trigger range.
- Physical spawners visualized:
	- Trigger range (red sphere).
	- Spawn range (cyan sphere).
	- Spawn limit range (white sphere).
	- Spawned creatures with the chances.
	- Max stars.
	- Spawn limit with current and max amount.

## Combat

- Player speed and noise on the HUD.
- More stats for weapons:
	- Chop and pickaxe damages.
	- Tool tier.
	- Attack speed.
	- Stamina usage (max and with current weapon skill),
	- Attack type and hitbox.
	- Accuracy and projectile speed for bows.
	- Secondary attack stats (damage multiplier, knocback multiplier, staggering multiplier).


# Changelog

- v1.9:
	- Removes remaining cheat settings as obsolete.
	- Fixes structure and player cover not updating. Thanks Nodus Cursorius!
	- Fixes errors with ships.
	- Fixes the bounds check causing errors with some objects.
	- Fixes spawn system rays having too much spacing ending up in wrong zones.
	- Fixes spawn systems of new creatures having wrong timer.

- v1.8:
	- Removes the ruler feature (split to a separate mod).
	- Fixes errors with custom crafting stations.

- v1.7:
	- Adds support for `*` value to config commands.
	- Adds support for new noise parameters.
	- Changes the command `esp_disable` to set the values to -1 instead 0.
	- Removes minimap coordinates as redundant.
	- Removes some debug mode related settings as obsolete.
	- Fixes locations not showing generator info.
	- Fixes comfort visual being shown for all pieces.
	- Fixes command autocomplete missing most keys.
	- Fixes some settings not working correctly.

- v1.6:
	- Adds commands for toggling settings on/off (can be bound to keys).
	- Adds separate settings for spawner trigger, limit and spawn ranges.
	- Adds position rays for spawners.
	- Adds Warm & Cozy effect area.
	- Adds cover ray visualization for players (disabled by default).
	- Adds more precision to spawn and generator chance percentages.
	- Adds "is blocked" to the HUD (for creature spawning, etc).
	- Adds tracking of all entitires to the creature tracker. Creature and item drops only check for loaded areas.
	- New setting for the range of the tracker (only works for other entities than creatures or item drops).
	- New settings which allow quickly toggling on/off features.
	- Fixes error when pickinng up items (also other similar cases fixed).
	- Removes setting groups which were quickly used show/hide visuals (better build your own with keybindings).
	- Fixes clock showing wrong time.
	- Removes DPS and experience meters as they are now in a own mod.
	- Removes mine rock support (separate mod exists for that with better support).

- v1.5:
	- Updated for Hearth & Home patch.
	- Adds average ship speed to HUD.
	- Adds setting to ignore forsaken power cooldowns.
	- Adds setting to set the maximum attack chain level.
	- Adds generator stats to locations.
	- Adds generator stats to vegetation (trees, plants, rocks, etc.).

- v1.4:
	- Adds settings to customize all colors used by the visuals.
	- Reordered settings to more sensible sections.
	- Adds sleeping status to sleeping enemies.
	- Adds wake up range and noise for sleeping enemies.
	- Adds setting to set the player always dodging (to test which attacks can be dodged).
	- Adds setting to increase player damage (past the damage cap).
	- Adds setting to multiply stamina usage (for no stamina usage).
	- Adds setting to multiply dig radius (causes visual glitches).
	- Adds visual for mine rock support bounding boxes (disabled by default).
	- Adds settings for custom spheres to containers and crafting stations (for people using "craft from containers" mods).
	- Changing the setting that was used to exclude pickables to exclude all resources.
	- Adds wildcard (*) support to tracking and exclusions.
	- Changed experience meter to ignore the first experience gain so that experience per minute shows up correctly.
	- Adds current skill level, experience amount and experience limit to experience meter.
	- Fixes experience meter showing wrong values.
	- Fixes smoke visual being affected by cover ray setting.
	- Improved localization of some object names.

- v1.3:
	- Adds item drops to resources like rocks, minerals and trees.
	- Adds chests contents to pregenerated chests.
	- Adds ruler. Ruler point can be set at the current location. HUD shows distance to the set location.
	- Improved experience meter to automatically update the experience gain percentage.
	- Torches no longer show cover system as they don't use it.
	- Windmills no longer show amount of smoke.
	- Fixes ward protection radius shown as sphere instead of a cylinder.
	- Fixes tool tiers being always stone / antler for creature attacks.

- v1.2:
	- Adds minimum tool tiers to trees, ores and other destructibles tooltips.
	- Adds location ray to trees, ores and other destructibles (disabled by default).
	- Adds hit box type to weapon and enemy attack tooltips.
	- Adds tool tier to weapons and enemy attack tooltips.
	- Adds visual range for comfort.
	- Adds setting for disabling visualization for certain structure effects.
	- Adds comfort amount and category to structure tooltips.
	- Adds stack size, despawn timer and despawn info to the item drop tooltips.
	- Adds visual range for bed, beehive, crafting station, fermenter, fireplace and windmill cover systems.
	- Adds cover percentage to structure tooltips.
	- Adds wind limit and current amount to the fireplace/torch tooltips.
	- Adds roof limit text to the fireplace/torch tooltips.
	- Adds smoke stats to smelters and fireplaces tooltips.
	- Adds visualization for smoke, including custom tooltip.
	- Adds creature tracker that shows amount of tracked creatures nearby (Serpent tracked by default).
	- Changed creauture rays to only work for tracked creatures.
	- Adds object coordinates to tooltips.
	- Improved message system to show everything at the same time (including messages from the game).
	- Adds wind strength, current coordinates and forest factor to the HUD.
	- Adds experience meter which can be used to track experience gain.
	- Enemy attacks no longer show damage, hit box or other info for non-attacks.
	- Item stands and crafting station upgrades now show additional information.
	- Fixes visualization toggle affecting whether support data was shown on tooltips.
	- Fixes ward damage resistances not showing chop and pickaxe immunity.
	- Fixes resistant resistance not showing in some cases.
	- Fixes battleaxe not showing secondary attack stats.
	- Deathsquito hardcoded to be immune to staggering (no stagger animation).
	- Creature tooltip now shows while the creature is staggering.
	- Spawn zones now show the spawn radius from players.
	- Fixes the visualization of structure support not automatically updating.
	- Adds localization to many object names.
	- Improved custom tooltips that nothing is shown when extra info is turned off.
	- Fixes marker lines not always being vertical.
	- Fixes no monsters area of the trader being the wrong size.
	- Reduced draw width of many visualizations (less clutter).
	- Fixes rays not always being straight up.

- v1.1:
	- Enabled for dedicated servers (when server admin).
	- Fixes structures flashing when disabling visuals.

- v1.0:
	- Initial release