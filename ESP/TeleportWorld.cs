using HarmonyLib;

namespace ESP
{
  [HarmonyPatch(typeof(TeleportWorld), "GetHoverText")]
  public class Teleport_GetHoverText
  {
    public static void Postfix(TeleportWorld __instance, ref string __result)
    {
      var wearNTear = __instance.GetComponent<WearNTear>();
      __result += WearNTearUtils.GetText(wearNTear);
    }
  }
}
