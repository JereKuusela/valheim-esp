namespace ESP
{
  public partial class Texts
  {
    public static string Get(ItemDrop obj)
    {
      if (!obj || !Settings.itemDrops) return "";
      var text = "\nStack size: " + Format.Int(obj.m_itemData.m_shared.m_maxStackSize);
      var timer = Patch.ItemDrop_GetTimeSinceSpawned(obj);
      var inBase = Patch.ItemDrop_IsInsideBase(obj);
      var playerInRange = Player.IsPlayerInRange(obj.transform.position, 25f);
      text += "\n" + Format.ProgressPercent("Despawn timer", timer, 3600);
      if (inBase)
        text += "\n" + Format.String("Despawn prevented by player base", "green");
      else if (timer > 3600 && !inBase && playerInRange)
        text += "\n" + Format.String("Despawn prevented by a nearby player (25 meters)", "green");
      else
        text += "\n" + Format.String("Despawning", "red");

      return text;
    }
  }
}