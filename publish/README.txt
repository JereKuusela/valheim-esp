# ESP

This mod adds lots of new information to tooltips and visualizes many hidden mechanics which can be used to improve your gameplay and perfect your build. Also includes a DPS meter.

Works on dedicated servers for server admins.

Some use cases:

- Perfecting your builds with a better understanding of structure support, structure health, cover and smoke mechanics.
- Spawn proofing your bases with the player base effect visualized.
- Determining the best weapons for the job by using the DPS meter and checking the detailed weapon tooltips.
- Finding the best ways to deal with tougher enemies by knowing their resistances and attacks.
- Getting deeper insight of exact mechanics and interaction by looking at the detailed visualizations and tooltips.

# Manual Installation

1. Install the [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim)
2. Download the latest zip
3. Extract it in the \<GameDirectory\>\BepInEx\plugins\ folder.

# Structures

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

# Gathering / Exploration

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
- Ship speed (both total and to forward direction), wind angle and wind strength on the HUD when sailing.

# Creatures

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

# Environment

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

# Creature spawning

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

# Combat

- Player speed and noise on the HUD.
- More stats for weapons:
	- Chop and pickaxe damages.
	- Tool tier.
	- Attack speed.
	- Stamina usage (max and with current weapon skill),
	- Attack type and hitbox.
	- Accuracy and projectile speed for bows.
	- Secondary attack stats (damage multiplier, knocback multiplier, staggering multiplier).
- Settings for player damage multiplier.
- Setting to multiply stamina usage (easier testing when stamina is infinite).
- Setting to enable permanent dodging (allows testing which attacks can be dodged).
- Setting to multiply dig radius (for easier mining).

# Ruler

- Allows setting a reference point at the current location.
- When sets, shows distance from the reference point (coordinates, distance and horizontal distance).


# Changelog

- v1.5.0:
	- Updated for Hearth & Home patch.
	- Added average ship speed to HUD.
	- Added setting to ignore forsaken power cooldowns.
	- Added setting to set the maximum attack chain level.
	- Added generator stats to locations.
	- Added generator stats to vegetation (trees, plants, rocks, etc.)
- v1.4.0:
	- Added settings to customize all colors used by the visuals.
	- Reordered settings to more sensible sections.
	- Added sleeping status to sleeping enemies.
	- Added wake up range and noise for sleeping enemies.
	- Added setting to set the player always dodging (to test which attacks can be dodged).
	- Added setting to increase player damage (past the damage cap).
	- Added setting to multiply stamina usage (for no stamina usage).
	- Added setting to multiply dig radius (causes visual glitches).
	- Added visual for mine rock support bounding boxes (disabled by default).
	- Added settings for custom spheres to containers and crafting stations (for people using "craft from containers" mods).
	- Changing the setting that was used to exclude pickables to exclude all resources.
	- Added wildcard (*) support to tracking and exclusions.
	- Changed experience meter to ignore the first experience gain so that experience per minute shows up correctly.
	- Added current skill level, experience amount and experience limit to experience meter.
	- Fixed experience meter showing wrong values.
	- Fixed smoke visual being affected by cover ray setting.
	- Improved localization of some object names.
- v1.3.0:
	- Added item drops to resources like rocks, minerals and trees.
	- Added chests contents to pregenerated chests.
	- Added ruler. Ruler point can be set at the current location. HUD shows distance to the set location.
	- Improved experience meter to automatically update the experience gain percentage.
	- Torches no longer show cover system as they don't use it.
	- Windmills no longer show amount of smoke.
	- Fixed ward protection radius shown a# ESP

This mod adds lots of new information to tooltips and visualizes many hidden mechanics which can be used to improve your gameplay and perfect your build. Also includes a DPS meter.

Works on dedicated servers for server admins.

Some use cases:

- Perfecting your builds with a better understanding of structure support, structure health, cover and smoke mechanics.
- Spawn proofing your bases with the player base effect visualized.
- Determining the best weapons for the job by using the DPS meter and checking the detailed weapon tooltips.
- Finding the best ways to deal with tougher enemies by knowing their resistances and attacks.
- Getting deeper insight of exact mechanics and interaction by looking at the detailed visualizations and tooltips.

# Manual Installation

1. Install the [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)
2. Download the latest zip
3. Extract it in the \<GameDirectory\>\BepInEx\plugins\ folder.

# Structures

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

# Gathering / Exploration

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
- Stack size and despawn timer on dropped items.
- Ship speed (both current and average), wind angle and wind strength on the HUD when sailing.

# Creatures

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

# Environment

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

# Creature spawning

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

# Combat

- Player speed and noise on the HUD.
- More stats for weapons:
	- Chop and pickaxe damages.
	- Tool tier.
	- Attack speed.
	- Stamina usage (max and with current weapon skill),
	- Attack type and hitbox.
	- Accuracy and projectile speed for bows.
	- Secondary attack stats (damage multiplier, knocback multiplier, staggering multiplier).
- Setting to overwrite player skill values (for easier testing).
- Settings for player damage multiplier, player damage range and creature damage range.
- Setting to multiply stamina usage (easier testing when stamina is infinite).
- Setting to enable permanent dodging (allows testing which attacks can be dodged).
- Setting to multiply dig radius (for easier mining).
- DPS can be toggled on and off with P key (which can also be used to reset the timer).
- The DPS meter tracks start and end of attacks which makes it more accurate than the default DPS tool (which only tracks hits).
- The DPS meter automatically stops when you stop attacking.
- Message box on the left hide shows following statistics:
	- Total time and amount of hits.
	- DPS, total damage and damage per used stamina (includes all stamina usage).
	- Also shows listed/base damage (what you see on weapon skills). This value ignores randomness and weapon skill (except for stamina usage).
	- Used stamina per second and total used stamina.
	- Caused staggering per second and total caused staggering.
	- Attack speed and hits per second.
	- Damage taken (per second and total).
	- Damage to structures, trees, stones and other destructibles are tracked separately.
- Experiencem meter can be toggled on and off with L key (which can also be used to reset the meter).
- Message box on the left hide shows following statistics:
	- Experience gain modifier.
	- Experience gained per skill (both total and per minute).
	- Current level and progress towards the next level.

# Ruler

- Allows setting a reference point at the current location.
- When sets, shows distance from the reference point (coordinates, distance and horizontal distance).


# Changelog

- v1.5.0:
	- Updated for Hearth & Home patch.
	- Added average ship speed to HUD.
	- Added setting to ignore forsaken power cooldowns.
	- Added setting to set the maximum attack chain level.
	- Added generator stats to locations.
	- Added generator stats to vegetation (trees, plants, rocks, etc.)
- v1.4.0:
	- Added settings to customize all colors used by the visuals.
	- Reordered settings to more sensible sections.
	- Added sleeping status to sleeping enemies.
	- Added wake up range and noise for sleeping enemies.
	- Added setting to set the player always dodging (to test which attacks can be dodged).
	- Added setting to increase player damage (past the damage cap).
	- Added setting to multiply stamina usage (for no stamina usage).
	- Added setting to multiply dig radius (causes visual glitches).
	- Added visual for mine rock support bounding boxes (disabled by default).
	- Added settings for custom spheres to containers and crafting stations (for people using "craft from containers" mods).
	- Changing the setting that was used to exclude pickables to exclude all resources.
	- Added wildcard (*) support to tracking and exclusions.
	- Changed experience meter to ignore the first experience gain so that experience per minute shows up correctly.
	- Added current skill level, experience amount and experience limit to experience meter.
	- Fixed experience meter showing wrong values.
	- Fixed smoke visual being affected by cover ray setting.
	- Improved localization of some object names.
- v1.3.0:
	- Added item drops to resources like rocks, minerals and trees.
	- Added chests contents to pregenerated chests.
	- Added ruler. Ruler point can be set at the current location. HUD shows distance to the set location.
	- Improved experience meter to automatically update the experience gain percentage.
	- Torches no longer show cover system as they don't use it.
	- Windmills no longer show amount of smoke.
	- Fixed ward protection radius shown as sphere instead of a cylinder.
	- Fixed tool tiers being always stone / antler for creature attacks.
- v1.2.0:
	- Added minimum tool tiers to trees, ores and other destructibles tooltips.
	- Added location ray to trees, ores and other destructibles (disabled by default).
	- Added hit box type to weapon and enemy attack tooltips.
	- Added tool tier to weapons and enemy attack tooltips.
	- Added visual range for comfort.
	- Added setting for disabling visualization for certain structure effects.
	- Added comfort amount and category to structure tooltips.
	- Added stack size, despawn timer and despawn info to the item drop tooltips.
	- Added visual range for bed, beehive, crafting station, fermenter, fireplace and windmill cover systems.
	- Added cover percentage to structure tooltips.
	- Added wind limit and current amount to the fireplace/torch tooltips.
	- Added roof limit text to the fireplace/torch tooltips.
	- Added smoke stats to smelters and fireplaces tooltips.
	- Added visualization for smoke, including custom tooltip.
	- Added creature tracker that shows amount of tracked creatures nearby (Serpent tracked by default).
	- Changed creauture rays to only work for tracked creatures.
	- Added object coordinates to tooltips.
	- Improved message system to show everything at the same time (including messages from the game).
	- Added wind strength, current coordinates and forest factor to the HUD.
	- Added experience meter which can be used to track experience gain.
	- Enemy attacks no longer show damage, hit box or other info for non-attacks.
	- Item stands and crafting station upgrades now show additional information.
	- Fixed visualization toggle affecting whether support data was shown on tooltips.
	- Fixed ward damage resistances not showing chop and pickaxe immunity.
	- Fixed resistant resistance not showing in some cases.
	- Fixed battleaxe not showing secondary attack stats.
	- Deathsquito hardcoded to be immune to staggering (no stagger animation).
	- Creature tooltip now shows while the creature is staggering.
	- Spawn zones now show the spawn radius from players.
	- Fixed the visualization of structure support not automatically updating.
	- Added localization to many object names.
	- Improved custom tooltips that nothing is shown when extra info is turned off.
	- Fixed marker lines not always being vertical.
	- Fixed no monsters area of the trader being the wrong size.
	- Reduced draw width of many visualizations (less clutter).
	- Fixed rays not always being straight up.
- v1.1.0:
	- Enabled for dedicated servers (when server admin).
	- Fixed structures flashing when disabling visuals.
- v1.0.0:
	- Initial releases sphere instead of a cylinder.
	- Fixed tool tiers being always stone / antler for creature attacks.
- v1.2.0:
	- Added minimum tool tiers to trees, ores and other destructibles tooltips.
	- Added location ray to trees, ores and other destructibles (disabled by default).
	- Added hit box type to weapon and enemy attack tooltips.
	- Added tool tier to weapons and enemy attack tooltips.
	- Added visual range for comfort.
	- Added setting for disabling visualization for certain structure effects.
	- Added comfort amount and category to structure tooltips.
	- Added stack size, despawn timer and despawn info to the item drop tooltips.
	- Added visual range for bed, beehive, crafting station, fermenter, fireplace and windmill cover systems.
	- Added cover percentage to structure tooltips.
	- Added wind limit and current amount to the fireplace/torch tooltips.
	- Added roof limit text to the fireplace/torch tooltips.
	- Added smoke stats to smelters and fireplaces tooltips.
	- Added visualization for smoke, including custom tooltip.
	- Added creature tracker that shows amount of tracked creatures nearby (Serpent tracked by default).
	- Changed creauture rays to only work for tracked creatures.
	- Added object coordinates to tooltips.
	- Improved message system to show everything at the same time (including messages from the game).
	- Added wind strength, current coordinates and forest factor to the HUD.
	- Added experience meter which can be used to track experience gain.
	- Enemy attacks no longer show damage, hit box or other info for non-attacks.
	- Item stands and crafting station upgrades now show additional information.
	- Fixed visualization toggle affecting whether support data was shown on tooltips.
	- Fixed ward damage resistances not showing chop and pickaxe immunity.
	- Fixed resistant resistance not showing in some cases.
	- Fixed battleaxe not showing secondary attack stats.
	- Deathsquito hardcoded to be immune to staggering (no stagger animation).
	- Creature tooltip now shows while the creature is staggering.
	- Spawn zones now show the spawn radius from players.
	- Fixed the visualization of structure support not automatically updating.
	- Added localization to many object names.
	- Improved custom tooltips that nothing is shown when extra info is turned off.
	- Fixed marker lines not always being vertical.
	- Fixed no monsters area of the trader being the wrong size.
	- Reduced draw width of many visualizations (less clutter).
	- Fixed rays not always being straight up.
- v1.1.0:
	- Enabled for dedicated servers (when server admin).
	- Fixed structures flashing when disabling visuals.
- v1.0.0:
	- Initial release