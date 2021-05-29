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
  }
}