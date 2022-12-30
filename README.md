# ESP

This mod adds lots of new information to tooltips and visualizes many hidden mechanics which can be used to improve your gameplay and perfect your build.

Install on the admin client (modding [guide](https://youtu.be/L9ljm2eKLrk)).

# Features

- Perfecting your builds with a better understanding of structure support, structure health, cover and smoke mechanics.
- Spawn proofing your bases with the player base effect visualized.
- Finding the best ways to deal with tougher enemies by knowing their resistances and attacks.
- Getting deeper insight of exact mechanics and interaction by looking at the detailed visualizations and tooltips.

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
- `esp_terrain [radius]`: Visualizes the terrain within the radius.

However once you know what you need, you should bind the `esp_toggle` command to a key to quick turn features on/off. For example: `bind o esp_toggle HUD ExtraInfo` would toggle the HUD and extra hover texts when pressing O button.

Parameter `*` affects all settings. For example `esp_disable *` hides all features.

Unfortunately the feature names for commands aren't documented (but the commands support autocomplete).

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
- Boss altars visualized.

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

