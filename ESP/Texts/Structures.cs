using System;
using UnityEngine;

namespace ESP
{
  public partial class Texts
  {
    private static string Get(Piece.ComfortGroup group)
    {
      if (group == Piece.ComfortGroup.Banner) return "Banne";
      if (group == Piece.ComfortGroup.Bed) return "Bed";
      if (group == Piece.ComfortGroup.Chair) return "Chair";
      if (group == Piece.ComfortGroup.Fire) return "Fire";
      return "None";
    }
    public static string Get(TreeLog obj)
    {
      if (!Settings.destructibles || !obj) return "";
      var text = "";
      var maxHealth = obj.m_health;
      var health = Patch.GetFloat(obj, "health", maxHealth);

      text += "\n" + Format.GetHealth(health, maxHealth);
      text += "\nHit noise: " + Format.Int(obj.m_hitNoise);
      text += "\n" + Texts.GetToolTier(obj.m_minToolTier, obj.m_damages.m_chop != HitData.DamageModifier.Immune, obj.m_damages.m_pickaxe != HitData.DamageModifier.Immune);
      text += DamageModifierUtils.Get(obj.m_damages, false, false);
      return text;
    }
    public static string Get(TreeBase obj)
    {
      if (!Settings.destructibles || !obj) return "";
      var text = "";
      var maxHealth = obj.m_health;
      var health = Patch.GetFloat(obj, "health", maxHealth);

      text += "\n" + Format.GetHealth(health, maxHealth);
      text += "\nHit noise: " + Format.Int(100);
      text += "\n" + Texts.GetToolTier(obj.m_minToolTier, obj.m_damageModifiers.m_chop != HitData.DamageModifier.Immune, obj.m_damageModifiers.m_pickaxe != HitData.DamageModifier.Immune);
      text += DamageModifierUtils.Get(obj.m_damageModifiers, false, false);
      return text;
    }
    public static string Get(Destructible obj)
    {
      if (!Settings.destructibles || !obj) return "";
      var text = "";
      var health = Patch.GetFloat(obj, "health", obj.m_health);
      var maxHealth = obj.m_health;

      text += "\n" + Format.GetHealth(health, maxHealth);
      text += "\nHit noise: " + Format.Int(obj.m_hitNoise);
      text += "\n" + Texts.GetToolTier(obj.m_minToolTier, obj.m_damages.m_chop != HitData.DamageModifier.Immune, obj.m_damages.m_pickaxe != HitData.DamageModifier.Immune);
      text += DamageModifierUtils.Get(obj.m_damages, false, false);
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
    public static string Get(WearNTear obj)
    {
      if (!Settings.structures || !obj) return "";
      var text = "";
      var health = obj.GetHealthPercentage();

      text += "\n" + Format.GetHealth(health * obj.m_health, obj.m_health);
      text += DamageModifierUtils.Get(obj.m_damages, true, false);

      if (SupportUtils.Enabled(obj))
      {
        float maxSupport, minSupport, horizontalLoss, verticalLoss;
        Patch.WearNTear_GetMaterialProperties(obj, out maxSupport, out minSupport, out horizontalLoss, out verticalLoss);
        var support = Math.Min(Patch.GetFloat(obj, "support", maxSupport), maxSupport);
        text += "\n" + Format.String(GetMaterialName(obj.m_materialType)) + ": " + Format.Progress(support, minSupport) + " support";
        text += "\n" + Format.Percent(horizontalLoss) + " horizontal loss, " + Format.Percent(verticalLoss) + " vertical loss";
      }
      return text;
    }
    public static string Get(Piece obj)
    {
      if (!Settings.structures || !obj) return "";
      var text = "";
      if (obj.m_comfort > 0)
      {
        text += "\nComfort: " + Format.Int(obj.m_comfort);
        if (obj.m_comfortGroup != Piece.ComfortGroup.None)
          text += " (" + Get(obj.m_comfortGroup) + ")";
      }
      return text;
    }
    public static string Get(Beehive obj)
    {
      if (!Settings.structures || !Settings.progress || !obj) return "";
      var limit = obj.m_secPerUnit;
      if (limit == 0) return "";
      var value = Patch.GetFloat(obj, "product");
      var text = "\n" + Format.ProgressPercent("Progress", value, limit);
      text += "\n" + GetCover(obj);
      return text;
    }
    public static string GetCover(Beehive obj)
    {
      if (!obj) return "";
      return GetCover(CoverUtils.GetCoverPoint(obj), obj.m_maxCover, false, false);
    }
    private static string GetItem(CookingStation obj, int slot) => Patch.GetString(obj, "slot" + slot);
    private static float GetTime(CookingStation obj, int slot) => Patch.GetFloat(obj, "slot" + slot);
    private static string GetSlotText(CookingStation obj, int slot)
    {
      var itemName = GetItem(obj, slot);
      var cookedTime = GetTime(obj, slot);
      if (itemName == "") return "";
      var item = Patch.CookingStation_GetItemConversion(obj, itemName);
      if (item == null) return "";
      var limit = item.m_cookTime;
      if (limit == 0) return "";
      var value = cookedTime;
      return "\n" + Format.ProgressPercent("Progress", value, limit);
    }
    public static string Get(CookingStation obj)
    {
      if (!Settings.structures || !Settings.progress || !obj) return "";
      var text = "";
      for (var slot = 0; slot < obj.m_slots.Length; slot++)
        text += GetSlotText(obj, slot);
      return text;
    }
    public static string Get(Fermenter obj)
    {
      if (!Settings.structures || !Settings.progress || !obj) return "";
      var limit = obj.m_fermentationDuration;
      if (limit == 0) return "";
      var value = Patch.Fermenter_GetFermentationTime(obj);
      var text = "\n" + Format.ProgressPercent("Progress", value, limit);
      text += "\n" + GetCover(obj);
      return text;
    }
    public static string GetCover(Fermenter obj)
    {
      if (!obj) return "";
      return GetCover(CoverUtils.GetCoverPoint(obj), Constants.CoverFermenterLimit);
    }
    public static string Get(SmokeSpawner obj)
    {
      if (!obj) return "";
      var text = GetSmokeLimit();
      text += "\nProduces smoke every " + Format.Float(obj.m_interval) + " s, unless smoke within " + Format.Float(obj.m_testRadius) + " m";
      return text;
    }
    public static string Get(Smoke obj)
    {
      if (!obj) return "";
      var text = ": " + Format.Progress(Smoke.GetTotalSmoke(), Constants.SmokeAmountLimit, true);
      text += "\n" + Format.ProgressPercent("Expires", Patch.m_time(obj), obj.m_ttl);
      var collider = obj.GetComponent<SphereCollider>();
      if (collider)
        text += "\nRadius: " + Format.Float(collider.radius * obj.transform.lossyScale.x);
      var body = Patch.m_body(obj);
      text += "\nMass: " + Format.Float(body.mass);
      text += "\nVelocity: " + Format.String(body.velocity.ToString("F3"));
      var ratio = 1f - Mathf.Clamp01(Patch.m_time(obj) / obj.m_ttl);
      var vel = obj.m_vel;
      vel.y *= ratio;
      text += "\nTarget: " + Format.String(vel.ToString("F3"));
      return text;
    }
    private static string GetSmokeLimit() => "\nSmoke: " + Format.Progress(Smoke.GetTotalSmoke(), Constants.SmokeAmountLimit, true);
    public static string Get(Fireplace obj)
    {
      if (!Settings.structures || !Settings.progress || !obj) return "";
      var limit = obj.m_secPerFuel;
      if (limit == 0) return "";
      var value = Patch.GetFloat(obj, "fuel") * limit;
      var text = "\n" + Format.ProgressPercent("Progress", value, limit);
      text += "\n" + GetCover(obj);
      return text;
    }
    private static string GetWind(Fireplace obj)
    {
      if (!obj) return "";
      var wind = EnvMan.instance.GetWindIntensity();
      var limit = Constants.WindFireplaceLimit;
      var pastLimit = wind >= limit;
      var text = "Wind (" + Format.Percent(limit, pastLimit ? "red" : "yellow") + ")";
      text += ": " + Format.Percent(wind);
      return text;
    }
    private static string GetDistanceFromRoof(Fireplace obj)
    {
      if (!obj) return "";
      if (Physics.Raycast(CoverUtils.GetCoverPoint(obj), Vector3.up, out var raycastHit, Constants.RoofFireplaceLimit, Patch.m_solidRayMask(obj)))
      {
        var distance = raycastHit.distance;
        return "Roof (" + Format.Float(Constants.RoofFireplaceLimit, Format.FORMAT, "red") + " m): " + Format.Float(distance) + " m";
      }
      return "";
    }
    public static string GetCover(Fireplace obj)
    {
      if (!obj) return "";
      var text = GetCover(CoverUtils.GetCoverPoint(obj), Constants.CoverFireplaceLimit, false);
      text += "\n" + GetWind(obj);
      var roofText = GetDistanceFromRoof(obj);
      if (roofText != "")
        text += "\n" + roofText;
      return text;
    }
    public static string Get(MineRock obj)
    {
      if (!Settings.destructibles || !obj) return "";
      var text = "";
      var maxHealth = obj.m_health;

      text += "\nHealth per area: " + Format.Int(maxHealth);
      text += "\nHit noise: " + Format.Int(100);
      text += "\n" + Texts.GetToolTier(obj.m_minToolTier, obj.m_damageModifiers.m_chop != HitData.DamageModifier.Immune, obj.m_damageModifiers.m_pickaxe != HitData.DamageModifier.Immune);
      text += DamageModifierUtils.Get(obj.m_damageModifiers, false, false);
      return text;
    }
    public static string Get(DropTable obj)
    {
      if (!Settings.destructibles) return "";
      var text = "";
      if (obj.m_dropChance < 1f)
        text += Format.Percent(obj.m_dropChance) + " for ";
      text += Format.Range(obj.m_dropMin, obj.m_dropMax) + " items";
      return text;
    }
    public static string Get(MineRock5 obj)
    {
      if (!Settings.destructibles || !obj) return "";
      var text = "";
      var maxHealth = obj.m_health;

      text += "\nHealth per area: " + Format.Int(maxHealth);
      text += "\nHit noise: " + Format.Int(100);
      text += Texts.GetToolTier(obj.m_minToolTier, obj.m_damageModifiers.m_chop != HitData.DamageModifier.Immune, obj.m_damageModifiers.m_chop != HitData.DamageModifier.Immune);
      text += DamageModifierUtils.Get(obj.m_damageModifiers, false, false);
      return text;
    }
    public static string Get(Plant obj)
    {
      if (!Settings.progress || !obj) return "";
      var limit = Patch.Plant_GetGrowTime(obj);
      if (limit == 0) return "";
      var value = Patch.Plant_TimeSincePlanted(obj);
      var text = "\n" + Format.ProgressPercent("Progress", value, limit);
      text += "\nRadius: " + Format.Meters(obj.m_growRadius);
      text += "\nGrows: " + Format.Name(obj.m_grownPrefabs);
      if (obj.m_destroyIfCantGrow)
        text += "\nDestroyed if can't grow";
      return text;
    }
    public static string Get(EffectArea obj)
    {
      if (Settings.effectAreaLineWidth == 0 || !obj) return "";
      return "\n" + EffectAreaUtils.GetTypeText(obj.m_type) + " " + Format.Radius(obj.GetRadius());
    }
    public static string Get(PrivateArea obj)
    {
      if (Settings.effectAreaLineWidth == 0 || !obj) return "";
      return "\nProtection " + Format.Radius(obj.m_radius);
    }

    private static string GetProgressText(Smelter instance)
    {
      var limit = instance.m_secPerProduct;
      if (limit == 0) return "";
      var value = Patch.Smelter_GetBakeTimer(instance);
      return "\n" + Format.ProgressPercent("Progress", value, limit);
    }
    private static string GetFuelText(Smelter instance)
    {
      var maxFuel = instance.m_maxFuel;
      var secPerFuel = instance.m_secPerProduct / instance.m_fuelPerProduct;
      if (maxFuel == 0) return "";
      var limit = maxFuel * secPerFuel;
      if (limit == 0) return "";
      var value = Patch.Smelter_GetFuel(instance) * secPerFuel;
      return "\n" + Format.ProgressPercent("Fuel", value, limit);
    }
    private static string GetPowerText(Windmill windmill)
    {
      if (!windmill) return "";
      var cover = Patch.m_cover(windmill);
      var speed = Utils.LerpStep(windmill.m_minWindSpeed, 1f, EnvMan.instance.GetWindIntensity());
      var powerText = "Power: " + Format.Percent(windmill.GetPowerOutput());
      var speedText = Format.Percent(speed) + " speed";
      var coverText = Format.Percent(cover) + " cover";
      return "\n" + powerText + " from " + speedText + " and " + coverText;
    }
    public static string Get(Smelter obj)
    {
      if (!Settings.structures || !Settings.progress || !obj) return "";
      return GetProgressText(obj) + GetFuelText(obj) + GetPowerText(obj.m_windmill) + "\n" + GetSmokeLimit();
    }

    private static float GetRelativeAngle(Ship ship)
    {
      var forward = ship.transform.forward * 1f;
      forward.y = 0;
      var forwardAngle = 90f - Mathf.Atan2(ship.transform.forward.z, ship.transform.forward.x) / Math.PI * 180f;
      var windDir = EnvMan.instance.GetWindDir() * 1f;
      windDir.y = 0;
      return Vector3.Angle(forward, windDir);
    }
    private static double GetWindPower(Ship ship)
    {
      var windIntensity = EnvMan.instance.GetWindIntensity();
      var windPower = Mathf.Lerp(0.25f, 1f, windIntensity);
      return windPower * ship.GetWindAngleFactor();
    }
    public static string Get(Ship obj)
    {
      if (!obj) return "";
      var text = "";
      var body = Patch.m_body(obj);
      var forwardSpeed = obj.GetSpeed();
      var forwardAngle = 90f - Mathf.Atan2(obj.transform.forward.z, obj.transform.forward.x) / Math.PI * 180f;
      if (forwardSpeed < 0)
        text += "\nSpeed: " + Format.Fixed(-forwardSpeed) + " m/s away from " + Format.Int(forwardAngle) + " degrees (backward)";
      else
        text += "\nSpeed: " + Format.Fixed(forwardSpeed) + " m/s towards " + Format.Int(forwardAngle) + " degrees (forward)";
      Vector3 velocity = body.velocity * 1f;
      velocity.y = 0f;
      var angle = 90f - Mathf.Atan2(velocity.z, velocity.x) / Math.PI * 180f;
      var speed = velocity.magnitude;
      text += "\nSpeed: " + Format.Fixed(speed) + " m/s towards " + Format.Int(angle) + " degrees";
      text += "\n" + EnvUtils.GetWind();
      text += "\nWind power: " + Format.Percent(GetWindPower(obj)) + " from " + Format.Int(GetRelativeAngle(obj)) + " degrees";
      return text;
    }
    private static string GetCover(Vector3 startPos, double limit, bool checkRoof = true, bool minLimit = true)
    {
      var cover = new Cover();
      var start = Constants.CoverRaycastStart;
      var total = 0;
      var hits = 0;
      Cover.GetCoverForPoint(startPos, out var percent, out var roof);

      foreach (var vector in Patch.m_coverRays(cover))
      {
        total++;
        RaycastHit raycastHit;
        if (Physics.Raycast(startPos + vector * start, vector, out raycastHit, Constants.CoverRayCastLength - start, Patch.m_coverRayMask(cover)))
          hits++;
      }
      var text = "Cover";
      if (limit > 0)
      {
        var pastLimit = minLimit ? percent < limit : percent > limit;
        text += " (" + Format.Percent(limit, pastLimit ? "red" : "yellow") + ")";
      }
      text += ": " + Format.Progress(hits, total, true);
      if (checkRoof && !roof)
        text += "\n" + Format.String("Not under roof!", "red");
      if (Math.Abs(percent - (float)hits / total) > 0.01) text += "\n" + Format.String("Error with cover calculation (" + percent + ")!", "red");
      return text;
    }
    public static string Get(Bed obj)
    {
      if (!obj) return "";
      return "\n" + GetCover(obj);
    }
    public static string GetCover(Bed obj)
    {
      if (!obj) return "";
      return GetCover(CoverUtils.GetCoverPoint(obj), Constants.CoverBedLimit);
    }
    public static string Get(CraftingStation obj)
    {
      if (!obj) return "";
      return "\n" + GetCover(obj);
    }
    public static string GetCover(CraftingStation obj)
    {
      if (!obj) return "";
      return GetCover(CoverUtils.GetCoverPoint(obj), Constants.CoverCraftingStationLimit);
    }
    public static string GetCover(Windmill obj)
    {
      if (!obj) return "";
      return GetCover(CoverUtils.GetCoverPoint(obj), 0, false);
    }
  }
}

