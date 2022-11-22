using System.Collections.Generic;
using System.Linq;
using Service;
namespace ESP;
public partial class Text
{
  public static string GetLevel(int min, int max, double chance, double limit = 0)
  {
    min--;
    max--;
    var level = Format.Range(min, max);
    var chanceText = level.Contains("-") ? " (" + Format.Percent(chance / 100f) + " per star)" : "";
    var levelLimit = (limit > 0) ? " after " + Format.Int(limit) + " meters" : "";
    return "Stars: " + level + chanceText + levelLimit;
  }
  public static string GetAttempt(double time, double limit, double chance) =>
    Format.ProgressPercent("Attempt", time, limit) + ", " + Format.Percent(chance / 100.0) + " chance";

  public static string GetGlobalKeys(List<string> required, List<string> notRequired)
  {
    required = required.Select(key => Format.String(key, ZoneSystem.instance.GetGlobalKey(key))).ToList();
    notRequired = notRequired.Select(key => Format.String("not " + key, !ZoneSystem.instance.GetGlobalKey(key))).ToList();
    var keys = required.Concat(notRequired);
    return Format.JoinRow(keys);
  }
  public static string GetHealth(double health, double limit)
    => "Health: " + Format.Progress(health, limit) + " (" + Format.Percent(health / limit) + ")";

  public static string Radius(float radius) => "Radius: " + Format.Float(radius);
}
