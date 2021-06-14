using System;

namespace ESP
{
  public partial class HoverTextUtils
  {
    public static string GetText(TreeLog obj)
    {
      if (!obj || !Settings.showStructureStats) return "";
      var text = "";
      var maxHealth = obj.m_health;
      var health = Patch.GetFloat(obj, "health", maxHealth);

      text += "\n" + TextUtils.GetHealth(health, maxHealth);
      text += "\nHit noise: " + TextUtils.Int(obj.m_hitNoise);
      text += DamageModifierUtils.GetText(obj.m_damages, false);
      return text;
    }
    public static string GetText(TreeBase obj)
    {
      if (!obj || !Settings.showStructureStats) return "";
      var text = "";
      var maxHealth = obj.m_health;
      var health = Patch.GetFloat(obj, "health", maxHealth);

      text += "\n" + TextUtils.GetHealth(health, maxHealth);
      text += "\nHit noise: " + TextUtils.Int(100);
      text += DamageModifierUtils.GetText(obj.m_damageModifiers, false);
      return text;
    }
    public static string GetText(Destructible obj)
    {
      if (!obj || !Settings.showStructureStats) return "";
      var text = "";
      var health = Patch.GetFloat(obj, "health", obj.m_health);
      var maxHealth = obj.m_health;

      text += "\n" + TextUtils.GetHealth(health, maxHealth);
      text += "\nHit noise: " + TextUtils.Int(obj.m_hitNoise);
      text += DamageModifierUtils.GetText(obj.m_damages, false);
      return text;
    }
    private static string GetMaterialName(WearNTear.MaterialType material)
    {
      if (material == WearNTear.MaterialType.HardWood) return "Hard wood";
      if (material == WearNTear.MaterialType.Stone) return "Stone";
      if (material == WearNTear.MaterialType.Iron) return "Iron";
      if (material == WearNTear.MaterialType.Wood) return "Wood";
      return "Unknown";
    }
    public static string GetText(WearNTear obj)
    {
      if (!obj || !Settings.showStructureStats) return "";
      var text = "";
      var health = obj.GetHealthPercentage();

      text += "\n" + TextUtils.GetHealth(health * obj.m_health, obj.m_health);
      text += DamageModifierUtils.GetText(obj.m_damages);

      float maxSupport, minSupport, horizontalLoss, verticalLoss;
      Patch.WearNTear_GetMaterialProperties(obj, out maxSupport, out minSupport, out horizontalLoss, out verticalLoss);
      var support = Math.Min(Patch.WearNTear_GetSupport(obj), maxSupport);
      text += "\n" + TextUtils.String(GetMaterialName(obj.m_materialType)) + ": " + TextUtils.Progress(support, minSupport) + " support";
      text += "\n" + TextUtils.Percent(horizontalLoss) + " horizontal loss, " + TextUtils.Percent(verticalLoss) + " vertical loss";
      return text;
    }
    public static string GetText(Beehive obj)
    {
      if (!obj || !Settings.showProgress) return "";
      var limit = obj.m_secPerUnit;
      if (limit == 0) return "";
      var value = Patch.GetFloat(obj, "product");
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static string GetText(Ship obj)
    {
      if (!obj || !Settings.showShipStats) return "";
      if (!obj.IsPlayerInBoat(Player.m_localPlayer.GetZDOID())) return "";
      return ShipUtils.text;
    }
    private static string GetItem(CookingStation obj, int slot) => Patch.GetString(obj, "slot" + slot);
    private static float GetTime(CookingStation obj, int slot) => Patch.GetFloat(obj, "slot" + slot);
    private static string GetSlotText(CookingStation obj, int slot)
    {
      if (!Settings.showProgress) return "";
      var itemName = GetItem(obj, slot);
      var cookedTime = GetTime(obj, slot);
      if (itemName == "") return "";
      var item = Patch.CookingStation_GetItemConversion(obj, itemName);
      if (item == null) return "";
      var limit = item.m_cookTime;
      if (limit == 0) return "";
      var value = cookedTime;
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static string GetText(CookingStation obj)
    {
      if (!obj || !Settings.showProgress) return "";
      var text = "";
      for (var slot = 0; slot < obj.m_slots.Length; slot++)
        text += GetSlotText(obj, slot);
      return text;
    }
    public static string GetText(Fermenter obj)
    {
      if (!obj || !Settings.showProgress) return "";
      var limit = obj.m_fermentationDuration;
      if (limit == 0) return "";
      var value = Patch.Fermenter_GetFermentationTime(obj);
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static string GetText(Fireplace obj)
    {
      if (!obj || !Settings.showProgress) return "";
      var limit = obj.m_secPerFuel;
      if (limit == 0) return "";
      var value = Patch.GetFloat(obj, "fuel") * limit;
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static string GetText(MineRock obj)
    {
      if (!obj || !Settings.showStructureStats) return "";
      var text = "";
      var maxHealth = obj.m_health;

      text += "\nHealth per area: " + TextUtils.Int(maxHealth);
      text += DamageModifierUtils.GetText(obj.m_damageModifiers, false);
      text += "\nHit noise: " + TextUtils.Int(100);
      return text;
    }
    public static string GetText(MineRock5 obj)
    {
      if (!obj || !Settings.showStructureStats) return "";
      var text = "";
      var maxHealth = obj.m_health;

      text += "\nHealth per area: " + TextUtils.Int(maxHealth);
      text += DamageModifierUtils.GetText(obj.m_damageModifiers, false);
      text += "\nHit noise: " + TextUtils.Int(100);
      return text;
    }
    public static string GetText(Plant obj)
    {
      if (!obj || !Settings.showProgress) return "";
      var limit = Patch.Plant_GetGrowTime(obj);
      if (limit == 0) return "";
      var value = Patch.Plant_TimeSincePlanted(obj);
      return "\n" + TextUtils.ProgressPercent("Progress", value, limit);
    }
    public static string GetText(EffectArea obj)
    {
      if (!obj || !Settings.showEffectAreas) return "";
      return "\n" + EffectAreaUtils.GetTypeText(obj.m_type) + " " + TextUtils.Radius(obj.GetRadius());
    }
    public static string GetText(PrivateArea obj)
    {
      if (!obj || !Settings.showEffectAreas) return "";
      return "\nProtection " + TextUtils.Radius(obj.m_radius);
    }
  }
}

