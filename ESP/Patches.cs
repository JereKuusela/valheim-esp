using HarmonyLib;

namespace ESP
{
  [HarmonyPatch]
  public class Patch
  {
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(SpawnArea), "GetInstances")]
    public static void SpawnArea_GetInstances(SpawnArea instance, out int near, out int total)
    {
      near = 0;
      total = 0;
    }

    [HarmonyReversePatch]
    [HarmonyPatch(typeof(CookingStation), "GetItemConversion")]
    public static CookingStation.ItemConversion CookingStation_GetItemConversion(CookingStation instance, string itemName)
    {
      return null;
    }

    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Fermenter), "GetFermentationTime")]
    public static double Fermenter_GetFermentationTime(Fermenter instance)
    {
      return 0;
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Plant), "GetGrowTime")]
    public static float Plant_GetGrowTime(Plant instance)
    {
      return 0;
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Plant), "TimeSincePlanted")]
    public static double Plant_TimeSincePlanted(Plant instance)
    {
      return 0;
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Smelter), "GetBakeTimer")]
    public static float Smelter_GetBakeTimer(Smelter instance)
    {
      return 0;
    }
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Smelter), "GetFuel")]
    public static float Smelter_GetFuel(Smelter instance)
    {
      return 0;
    }
  }
}