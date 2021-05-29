using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace ESP
{
  public class Settings
  {
    public static ConfigEntry<bool> configShowCreatureRays;
    public static bool showCreatureRays => configShowCreatureRays.Value;
    public static ConfigEntry<string> configExcludedCreatures;
    public static string excludedCreatures => configExcludedCreatures.Value;
    public static ConfigEntry<float> configCharacterRayWidth;
    public static float characterRayWidth => configCharacterRayWidth.Value;

    public static ConfigEntry<bool> configShowSpawnAreas;
    public static bool showSpawnAreas => configShowSpawnAreas.Value;
    public static ConfigEntry<bool> configShowCreatureSpawners;
    public static bool showCreatureSpawners => configShowCreatureSpawners.Value;
    public static ConfigEntry<string> configExcludedCreatureSpawners;
    public static string excludedCreatureSpawners => configExcludedCreatureSpawners.Value;
    public static ConfigEntry<bool> configShowBiomes;
    public static bool showBiomes => configShowBiomes.Value;
    public static ConfigEntry<bool> configShowEffectAreas;
    public static bool showEffectAreas => configShowEffectAreas.Value;
    public static ConfigEntry<bool> configShowPickables;
    public static bool showPickables => configShowPickables.Value;
    public static ConfigEntry<string> configExcludedPickables;
    public static string excludedPickables => configExcludedPickables.Value;
    public static ConfigEntry<bool> configShowBaseAI;
    public static bool showBaseAI => configShowBaseAI.Value;
    public static ConfigEntry<bool> configshowNoise;
    public static bool showNoise => configshowNoise.Value;
    public static ConfigEntry<bool> configShowProgress;
    public static bool showProgress => configShowProgress.Value;
    public static ConfigEntry<bool> configShowCreatureStats;
    public static bool showCreatureStats => configShowCreatureStats.Value;
    public static ConfigEntry<bool> configShowChests;
    public static bool showChests => configShowChests.Value;
    public static ConfigEntry<bool> configShowLocations;
    public static bool showLocations => configShowLocations.Value;
  }


  [BepInPlugin("valheim.jerekuusela.esp", "ESP", "1.0.0.0")]
  public class ESP : BaseUnityPlugin
  {
    void Awake()
    {
      Settings.configShowBaseAI = Config.Bind("Creatures", "Show creature senses", true, "Enable to visualuze creature sight and hear ranges");
      Settings.configShowCreatureStats = Config.Bind("Creatures", "Show creature stats", true, "Enable to show creature health, stagger, mass and resistances");
      Settings.configShowCreatureRays = Config.Bind("Creatures", "Show creature rays", true, "Enable to visualize creature locations");
      Settings.configCharacterRayWidth = Config.Bind("Creatures", "Width of the character rays", 0.5f, "");
      Settings.configExcludedCreatures = Config.Bind("Creatures", "Exclude creatures", "", "List of creatures separated by ,");

      Settings.configShowCreatureSpawners = Config.Bind("SpawnPoints", "ShowSpawnPoints", true, "Enable for creature spawn points");
      Settings.configExcludedCreatureSpawners = Config.Bind("SpawnPoints", "ExcludedSpawnPoints", "", "List of creatures separated by , that are not visualized");
      Settings.configShowSpawnAreas = Config.Bind("General", "ShowCreatureSpawners", true, "Enable for creature spawners");
      Settings.configShowBiomes = Config.Bind("General", "ShowBiomes", true, "Enable biomes");
      Settings.configShowEffectAreas = Config.Bind("General", "ShowEffectAreas", true, "Enable for structure effect areas");
      Settings.configShowPickables = Config.Bind("Pickables", "ShowPickables", true, "Enablee for pickables");
      Settings.configExcludedPickables = Config.Bind("Pickables", "ExcludedPickables", "Wood,Stone", "List of items separated by , that are not visualized");
      Settings.configshowNoise = Config.Bind("General", "ShowNoise", true, "Enable for noise");
      Settings.configShowProgress = Config.Bind("General", "ShowProgress", true, "Enable progress for plants and structures");
      Settings.configShowChests = Config.Bind("General", "ShowChests", true, "Enable for hidden chests");
      Settings.configShowLocations = Config.Bind("General", "ShowLocations", true, "Enable for pre-made structures and other locations");
      var harmony = new Harmony("valheim.jerekuusela.reverse_engineer");
      harmony.PatchAll();
    }
  }
}
