# ESP

This mod provides tools to get a deeper understanding of the game mechanics.

# Manual Installation:

1. Install the [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)
2. Download the latest zip
3. Extract it in the \<GameDirectory\>\BepInEx\plugins\ folder.

# Features

- HUD to show time of day, weather, current speed and current noise.
- HUD also shows hotkeys to toggle diferent module on and off.
- Automatically enables devcommands, debugmode, god mode, free build and free fly when in single player.
- Lots of settings to tweak the tool as needed (most things can be turned off).

# DPS meter (toggle with P)

- Can be toggle on and off with P key (which can also be used to reset the timer).
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

# Zone visualization (toggle with Y)

- Shows location of zone corners (color of the line depends on the biome) with following information:
  - Biome, time of the day, current weather, current wind and average wind for the biome.
  - Avalaible weathers including their chances, wind limits and other properties.
  - Timer for the next weather chance.
- Shows spawan zone system (colors depend on the biome) with following information:
  - Name of the creature, spawn timer, conditions, limits, max stars, amount of spawned, etc. 
- Shows random event system (black line) with following information:
  - Timer for the next event attempt.
  - Available events including their conditions. Grey color shows failed conditions and events that are not currently possible.
  - If an event is going, shows event name, timer and event spawners.

# Creature visualization (toggle with U)

- Shows location of creatures.
- Shows creature hearing range (unless infinite).
- Shows creature vision range and angle (and also the alert range).
- Shows fire radius for creatures that avoid or are afraid of fires.
- Shows breeding limit range and parter check range for breedable creatures.
- Shows food search check range and eating range for tameable creatures.

# Other visualization (toggle with I)

- Shows location of pickables (stones, berries, etc) and their respawn timers (green for respawnable, blue for one time).
- Shows location of pregenerated structures (black color) and their internal names.
- Shows location of fixed creature spawn points and their respawn timers (yellow for respawnable, red for one time).
- Shows location of hidden chests (white color).
- Shows structure support color for all structures.
- Shows area effects from structures (player base, fire, heat, etc).

# Tooltips (toggle with O)

- Chop and pickaxe damages added to weapon tooltips.
- Attack speed, stamina usage, hitbox, accuracy and secondary attack effects added to weapon tooltips.
- Structure internal names, health, damage resistances, support stats (based on material) and current support.
- Destructible, tree and rock internal names, health, hit noise and damage resistances.
- Creature health, stagger limit, mass, knockback resistance, damage resistances and item drops.
- Creature attacks with name, cooldown, damages, range, angle and hitbox.
- Fireplace, torches and smelters fuel amount.
- Beehive, smelter, kiln, etc. progression.
- Plant growth time and progression.
- Ship speed and wind conditions.
- Creature spawner spawn timer, spawned enemies and ranges.

# Changelog
- v1.1.0: 
	- Enabled for dedicated servers (when server admin).
	- Fixed structures flashing when disabling visuals.
- v1.0.0: 
	- Initial release