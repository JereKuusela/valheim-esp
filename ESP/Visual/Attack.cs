using System.Collections.Generic;
using HarmonyLib;
using Service;
using UnityEngine;
using Visualization;

namespace ESP;

[HarmonyPatch(typeof(Attack), nameof(Attack.OnAttackTrigger))]
public class AttackTrigger
{
  static void Postfix(Attack __instance)
  {
    if (!__instance.m_character) return;
    AttackUtils.DrawAttack(__instance.m_character, __instance);
  }
}
public partial class AttackUtils
{
  private static string GetText(Attack attack)
  {
    var lines = new List<string>
    {
      $"Type: {attack.m_attackType}",
      $"Height: {Format.Meters(attack.m_attackHeight)}",
      $"Offset: {Format.Meters(attack.m_attackOffset)}",
      $"Range: {Format.Meters(attack.m_attackRange)}",
      $"Angle: {Format.Degrees(attack.m_attackAngle)}",
      $"Ray: {Format.Meters(attack.m_attackRayWidth)}"
    };
    return string.Join("\n", lines);
  }
  public static void DrawAttack(Humanoid parent, Attack attack)
  {
    var type = attack.m_attackType;
    var height = attack.m_attackHeight;
    var offset = attack.m_attackOffset;
    var range = attack.m_attackRange;
    var ray = attack.m_attackRayWidth;
    var angle = attack.m_attackAngle;
    var visuals = Draw.GetRenderers(parent, Tag.Attack);
    foreach (var renderer in visuals)
      Object.Destroy(renderer.gameObject);
    if (type != Attack.AttackType.Horizontal && type != Attack.AttackType.Vertical) return;
    var origin = Vector3.up * height + Vector3.right * offset;
    var half = angle / 2f;
    for (float a = -half; a <= half; a += 4f)
    {
      var rot = Quaternion.identity;
      if (type == Attack.AttackType.Horizontal)
        rot = Quaternion.Euler(0f, -a, 0f);
      else if (type == Attack.AttackType.Vertical)
        rot = Quaternion.Euler(a, 0f, 0f);
      var end = origin + rot * Vector3.forward * (range - ray);
      var line = Draw.DrawLine(Tag.Attack, parent, origin, end);
      var name = attack.m_weapon.m_dropPrefab ? Utils.GetPrefabName(attack.m_weapon.m_dropPrefab) : "";
      Draw.AddText(line, name, GetText(attack));
      var renderer = line.GetComponent<LineRenderer>();
      if (renderer) renderer.widthMultiplier = 2 * Mathf.Max(ray, 0.01f);
    }
  }
}