using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace ESP
{
  public class Settings
  {
    public static ConfigEntry<bool> configShowCreatureSpawners;
    public static bool showCreatureSpawners => configShowCreatureSpawners.Value;
    public static ConfigEntry<bool> configShowBiomes;
    public static bool showBiomes => configShowBiomes.Value;
    public static ConfigEntry<bool> configShowEffectAreas;
    public static bool showEffectAreas => configShowEffectAreas.Value;
    public static ConfigEntry<bool> configShowPickables;
    public static bool showPickables => configShowPickables.Value;
    public static ConfigEntry<bool> configShowBaseAI;
    public static bool showBaseAI => configShowBaseAI.Value;
    public static ConfigEntry<bool> configshowNoise;
    public static bool showNoise => configshowNoise.Value;
    public static ConfigEntry<bool> configShowProgress;
    public static bool showProgress => configShowProgress.Value;
  }


  [BepInPlugin("valheim.jerekuusela.esp", "ESP", "1.0.0.0")]
  public class ESP : BaseUnityPlugin
  {
    void Awake()
    {
      Settings.configShowCreatureSpawners = Config.Bind("General", "ShowCreatureSpawners", true, "Whether to visualize creature spawn points");
      Settings.configShowBiomes = Config.Bind("General", "ShowBiomes", true, "Whether to visualize biomes");
      Settings.configShowEffectAreas = Config.Bind("General", "ShowEffectAreas", true, "Whether to visualize structure effect areas");
      Settings.configShowPickables = Config.Bind("General", "ShowPickables", true, "Whether to visualize pickables");
      Settings.configShowBaseAI = Config.Bind("General", "ShowCreatures", true, "Whether to visualize creatures");
      Settings.configshowNoise = Config.Bind("General", "ShowNoise", true, "Whether to visualize noise");
      Settings.configShowProgress = Config.Bind("General", "ShowProgress", true, "Whether to show progress for plants and structures");
      var harmony = new Harmony("valheim.jerekuusela.reverse_engineer");
      harmony.PatchAll();
    }
  }
}
