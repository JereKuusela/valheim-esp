using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace ESP
{
  public class Settings
  {
    public static ConfigEntry<bool> configShowSpawnAreas;
    public static bool showSpawnAreas => configShowSpawnAreas.Value;
    public static ConfigEntry<bool> configShowCreatureSpawners;
    public static bool showCreatureSpawners => configShowCreatureSpawners.Value;
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
      Settings.configShowCreatureSpawners = Config.Bind("General", "ShowCreatureSpawners", true, "Enable creature spawn points");
      Settings.configShowSpawnAreas = Config.Bind("General", "ShowSpawnAreas", true, "Enable creature spawners");
      Settings.configShowBiomes = Config.Bind("General", "ShowBiomes", true, "Enable biomes");
      Settings.configShowEffectAreas = Config.Bind("General", "ShowEffectAreas", true, "Enable structure effect areas");
      Settings.configShowPickables = Config.Bind("Pickables", "ShowPickables", true, "Enablee pickables");
      Settings.configExcludedPickables = Config.Bind("Pickables", "ExcludedPickables", "Wood,Stone", "List of items separated by , that are not visualized");
      Settings.configShowBaseAI = Config.Bind("General", "ShowCreatures", true, "Enable creatures");
      Settings.configshowNoise = Config.Bind("General", "ShowNoise", true, "Enable noise");
      Settings.configShowProgress = Config.Bind("General", "ShowProgress", true, "Enable progress for plants and structures");
      Settings.configShowChests = Config.Bind("General", "ShowChests", true, "Enable hidden chests");
      Settings.configShowLocations = Config.Bind("General", "ShowLocations", true, "Enable pre-made structures and other locations");
      var harmony = new Harmony("valheim.jerekuusela.reverse_engineer");
      harmony.PatchAll();
    }
  }
}
