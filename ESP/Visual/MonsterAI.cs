using HarmonyLib;
using UnityEngine;

namespace ESP
{
  [HarmonyPatch(typeof(MonsterAI), "Awake")]
  public class MonsterAI_Awake
  {
    private static void DrawAlertRange(MonsterAI instance, Character character)
    {
      var range = instance.m_alertRange;
      var angle = instance.m_viewAngle;
      var text = Texts.GetSense(instance);
      var obj = Drawer.CreateObject(instance.gameObject);
      Drawer.DrawArcY(obj, character.m_eye.position - character.transform.position, range, angle, Color.red, 0.1f);
      Drawer.DrawArcX(obj, character.m_eye.position - character.transform.position, range, angle, Color.red, 0.1f);
      Drawer.AddText(obj, Format.Name(character), text);
      Drawer.AddMeshCollider(obj);
    }
    public static void Postfix(MonsterAI __instance, Character ___m_character)
    {
      if (!Settings.showBaseAI)
        return;
      DrawAlertRange(__instance, ___m_character);
    }
  }
}