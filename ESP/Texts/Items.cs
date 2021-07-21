using System.Collections.Generic;

namespace ESP
{
  public partial class Texts
  {
    public static string Get(ItemDrop obj)
    {
      if (!obj || !Settings.itemDrops) return "";
      var lines = new List<string>();
      lines.Add("Stack size: " + Format.Int(obj.m_itemData.m_shared.m_maxStackSize));
      var timer = Patch.ItemDrop_GetTimeSinceSpawned(obj);
      var inBase = Patch.ItemDrop_IsInsideBase(obj);
      var playerInRange = Player.IsPlayerInRange(obj.transform.position, 25f);
      lines.Add(Format.ProgressPercent("Despawn timer", timer, 3600));
      if (inBase)
        lines.Add(Format.String("Despawn prevented by player base", "green"));
      else if (timer > 3600 && !inBase && playerInRange)
        lines.Add(Format.String("Despawn prevented by a nearby player (25 meters)", "green"));
      else
        lines.Add(Format.String("Despawning", "red"));

      return Format.JoinLines(lines);
    }
  }
}