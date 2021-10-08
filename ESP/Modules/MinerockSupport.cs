using System;
using System.Linq;
using BepInEx.Configuration;
using HarmonyLib;
using Text;
using UnityEngine;
using Visualization;

namespace Modules {
  public class MinerockSupport {
    public const string TagSupport = "MINEROCK_SUPPORT";
    public static ConfigEntry<float> configLineWidth;
    private static float LineWidth => Math.Max(0.01f, configLineWidth.Value);
    public static ConfigEntry<int> configMaxAmount;
    private static int MaxAmount => configMaxAmount.Value;
    public static ConfigEntry<int> configMinSize;
    private static int MinSize => configMinSize.Value;
    public static ConfigEntry<string> configColor;
    private static Color Color => ESP.Settings.ParseColor(configColor.Value);
    public static void Init(ConfigFile config) {
      var section = "Minerock support";
      configMaxAmount = config.Bind(section, "Max pieces", 50, "Max amount of boxes before showing any (0 to disable).");
      configMinSize = config.Bind(section, "Min size", 10, "Minimum amount of parts to display any boxes.");
      configLineWidth = config.Bind(section, "Line width", 0.02f, "Line width of the bounding boxes.");
      configLineWidth.SettingChanged += (s, e) => {
        Draw.SetLineWidth(TagSupport, LineWidth);
      };
      configColor = config.Bind(section, "Color", "red", "");
      configColor.SettingChanged += (s, e) => {
        Draw.SetColor(TagSupport, Color);
      };
    }
    private static Collider[] tempColliders = new Collider[128];
    public static void DrawSupport(MineRock5 obj) {
      Draw.Remove(obj, TagSupport);
      if (LineWidth == 0) return;
      var areas = ESP.Patch.HitAreas(obj);
      if (areas.Count() < MinSize) return;
      var remaining = areas.Count(area => ESP.Patch.Health(area) > 0f);
      var index = -1;
      var supportedCount = 0;
      foreach (var area in areas) {
        index++;
        var health = ESP.Patch.Health(area);
        if (health <= 0f) continue;
        var bounds = ESP.Patch.Bound(area);
        var pos = ESP.Patch.Pos(bounds);
        var size = ESP.Patch.Size(bounds);
        var rot = ESP.Patch.Rot(bounds);
        var mask = ESP.Patch.RayMask(obj);
        int num = Physics.OverlapBoxNonAlloc(obj.transform.position + pos, size, tempColliders, rot, mask);
        var areaCollider = ESP.Patch.Collider(area);
        for (int i = 0; i < num; i++) {
          var collider = tempColliders[i];
          if (!(collider == areaCollider) && !(collider.attachedRigidbody != null) && !collider.isTrigger) {
            var componentInParent = collider.gameObject.GetComponentInParent<IDestructible>();
            if ((object)componentInParent == obj) continue;
            var supported = ESP.Patch.MineRock5_GetSupport(obj, collider);
            if (supported) {
              supportedCount++;
              var box = Draw.DrawBox(TagSupport, obj, Color, LineWidth, pos, size);
              Draw.AddText(box, "Index: " + Format.Int(index), "Size: " + Format.Coordinates(2 * size, "F1"));
              break;
            }
          }
        }
      }
      if (supportedCount > MaxAmount) Draw.Remove(obj, TagSupport);
    }
  }
  [HarmonyPatch(typeof(MineRock5), "UpdateSupport")]
  public class MineRock5_Support {
    public static void Postfix(MineRock5 __instance) => MinerockSupport.DrawSupport(__instance);
  }
}
