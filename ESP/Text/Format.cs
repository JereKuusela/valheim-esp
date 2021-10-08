using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;


namespace Text {
  public class Format {
    public const string FORMAT = "0.##";

    public static string GetValidColor(bool valid) => valid ? "yellow" : "grey";
    public static string String(string value, string color = "yellow") => "<color=" + color + ">" + value + "</color>";
    public static string String(string value, bool valid) => "<color=" + GetValidColor(valid) + ">" + value + "</color>";
    public static string Float(double value, string format = FORMAT, string color = "yellow") => String(value.ToString(format, CultureInfo.InvariantCulture), color);
    public static string Multiplier(double value, string color = "yellow") => String(value.ToString(FORMAT, CultureInfo.InvariantCulture) + "x", color);
    public static string Meters(double value, string color = "yellow") => String(value.ToString(FORMAT, CultureInfo.InvariantCulture) + " meters", color);
    public static string Degrees(double value, string color = "yellow") => String(value.ToString(FORMAT, CultureInfo.InvariantCulture) + " degrees", color);
    public static string Fixed(double value) {
      return String(value.ToString("N2", CultureInfo.InvariantCulture).PadLeft(5, '0'));
    }
    public static string Percent(double value, string color = "yellow") => String((100.0 * value).ToString(FORMAT, CultureInfo.InvariantCulture) + " %", color);
    public static string PercentInt(double value, string color = "yellow") => String(value.ToString("P0", CultureInfo.InvariantCulture), color);

    public static string Range(double min, double max, string color = "yellow") {
      if (min == max)
        return String(max.ToString(FORMAT, CultureInfo.InvariantCulture), color);
      return String(min.ToString(FORMAT, CultureInfo.InvariantCulture), color) + "-" + String(max.ToString(FORMAT, CultureInfo.InvariantCulture), color);
    }
    public static string PercentRange(double min, double max) {
      if (min == max)
        return max.ToString("P0", CultureInfo.InvariantCulture);
      return min.ToString("P0", CultureInfo.InvariantCulture) + "-" + max.ToString("P0", CultureInfo.InvariantCulture);
    }
    public static string Progress(double value, double limit, bool percent = false) => String(value.ToString("N0")) + "/" + String(limit.ToString("N0")) + (percent ? " (" + PercentInt(value / limit) + ")" : "");
    public static string Int(double value, string color = "yellow") => String(value.ToString("N0"), color);
    public static string ProgressPercent(string header, double value, double limit) => header + ": " + Progress(value, limit) + " seconds (" + Percent(value / limit) + ")";

    public static string GetLevel(int min, int max, double chance, double limit = 0) {
      min--;
      max--;
      var level = Format.Range(min, max);
      var chanceText = level.Contains("-") ? " (" + Format.Percent(chance / 100f) + " per star)" : "";
      var levelLimit = (limit > 0) ? " after " + Format.Int(limit) + " meters" : "";
      return "Stars: " + level + chanceText + levelLimit;
    }
    public static string JoinLines(IEnumerable<string> lines) => string.Join("\n", lines.Where(line => line != ""));
    public static string JoinRow(IEnumerable<string> lines) => string.Join(", ", lines.Where(line => line != ""));

    public static string GetAttempt(double time, double limit, double chance) =>
      Format.ProgressPercent("Attempt", time, limit) + ", " + Format.Percent(chance / 100.0) + " chance";

    public static string GetGlobalKeys(List<string> required, List<string> notRequired) {
      required = required.Select(key => String(key, ZoneSystem.instance.GetGlobalKey(key))).ToList();
      notRequired = notRequired.Select(key => String("not " + key, !ZoneSystem.instance.GetGlobalKey(key))).ToList();
      var keys = required.Concat(notRequired);
      return JoinRow(keys);
    }
    public static string Coordinates(Vector3 coordinates, string format = "F0", string color = "yellow") {
      var values = coordinates.ToString(format).Replace("(", "").Replace(")", "").Split(',').Select(value => String(value.Trim(), color));
      return JoinRow(values);
    }
    public static string GetHealth(double health, double limit)
      => "Health: " + Format.Progress(health, limit) + " (" + Format.Percent(health / limit) + ")";

    public static string Name(string name, string color = "yellow") => String(Localization.instance.Localize(name).Replace("(Clone)", ""), color);
    public static string Name(Heightmap.Biome obj, string color = "yellow") => Name(Names.GetName(obj), color);
    private static string Name(Character obj) => obj ? obj.m_name : "";
    public static string Name(ItemDrop.ItemData obj, string color = "yellow") => obj != null ? Name(obj.m_shared.m_name, color) : "";
    private static string Name(Pickable obj) => obj ? obj.m_itemPrefab.name : "";
    public static string Name(CreatureSpawner obj) => obj ? Utils.GetPrefabName(obj.m_creaturePrefab) : "";
    public static string Name(IEnumerable<GameObject> objs, string color = "yellow") => JoinRow(objs.Select(prefab => Format.Name(prefab, color)));
    public static string Name(IEnumerable<ItemDrop> objs, string color = "yellow") => JoinRow(objs.Select(prefab => Format.Name(prefab, color)));
    private static string Name(Bed obj) => obj ? obj.GetHoverName() : "";
    private static string Name(Piece obj) => obj ? obj.m_name : "";
    private static string Name(TreeLog obj) => obj ? obj.name : "";
    private static string Name(Location obj) => obj ? obj.name : "";
    private static string Name(MineRock obj) => obj ? obj.m_name : "";
    private static string Name(MineRock5 obj) => obj ? obj.m_name : "";
    private static string Name(TreeBase obj) => obj ? obj.name : "";
    private static string Name(Destructible obj) => obj ? obj.name : "";
    private static string Name(Smoke obj) => obj ? "Smoke" : "";
    private static string Name(HoverText obj) => obj ? obj.m_text : "";
    public static string Name(MonoBehaviour obj, string color = "yellow") {
      var text = "";
      if (text == "") text = Format.Name(obj.GetComponentInParent<HoverText>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<Smoke>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<CreatureSpawner>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<Pickable>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<Bed>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<TreeLog>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<Location>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<MineRock>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<MineRock5>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<TreeBase>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<Destructible>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<Character>());
      if (text == "") text = Format.Name(obj.GetComponentInParent<Piece>());
      if (text == "") text = Name(obj.gameObject, color);
      return Name(text, color);
    }
    public static string Name(GameObject obj, string color = "yellow") => obj ? Name(Utils.GetPrefabName(obj), color) : "";

    public static string Radius(float radius) => "Radius: " + Format.Float(radius);
  }
}