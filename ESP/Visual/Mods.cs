using System.Reflection;
using HarmonyLib;
using Service;
using UnityEngine;
using Visualization;
namespace ESP;

public class JewelcraftingPatcher
{
  public static void DoPatching(Assembly assembly)
  {
    if (assembly == null) return;
    Harmony harmony = new("esp.jewelcrafting");
    var mOriginal = AccessTools.Method(assembly.GetType("Jewelcrafting.DestructibleSetup+GemSpawner"), "Awake");
    if (mOriginal == null)
    {
      Log.Warning("\"Jewelcrafting\" detected. Unable to patch \"Awake\" for visual rays.");
      return;
    }
    Log.Info("\"Jewelcrafting\" detected. Patching \"Awake\" for visual rays.");
    var mPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => Postfix(__instance));
    harmony.Patch(mOriginal, null, new(mPostfix));
  }

  static void Postfix(MonoBehaviour __instance)
  {
    var obj = Draw.DrawMarkerLine(Tag.SpawnPointRespawning, __instance);
    Text.AddText(obj, Utils.GetPrefabName(__instance.gameObject));
  }
}
