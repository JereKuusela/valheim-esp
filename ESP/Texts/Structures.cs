using System;
using System.Collections.Generic;
using System.Linq;
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
      var lines = new List<string>();
      var maxHealth = obj.m_health;
      var health = Patch.GetFloat(obj, "health", maxHealth);
      lines.Add(Format.GetHealth(health, maxHealth));
      lines.Add("Hit noise: " + Format.Int(obj.m_hitNoise));
      if (obj.m_subLogPrefab)
        lines.Add("Destroy creates: " + Format.Int(obj.m_subLogPoints.Length) + " " + Format.Name(obj.m_subLogPrefab));
      lines.Add(Texts.GetToolTier(obj.m_minToolTier, obj.m_damages.m_chop != HitData.DamageModifier.Immune, obj.m_damages.m_pickaxe != HitData.DamageModifier.Immune));
      lines.Add(DamageModifierUtils.Get(obj.m_damages, false, false));
      lines.Add(Get(obj.m_dropWhenDestroyed, 1));
      return Format.JoinLines(lines);
    }
    public static string Get(DropTable obj, int areas)
    {
      if (obj == null || obj.m_drops.Count == 0) return "";
      var lines = new List<string>();
      if (obj.m_oneOfEach && obj.m_dropMin == obj.m_dropMax && obj.m_dropMin == obj.m_drops.Count)
      {
        // All items are guaranteed to drop.
        lines.Add("Drops:");
        var drops = obj.m_drops.Select(drop =>
        {
          var averageItems = (drop.m_stackMin + drop.m_stackMax) / 2.0;
          var averageText = Format.Float(averageItems);
          if (areas > 1)
            averageText += " * " + Format.Int(areas) + " = " + Format.Float(areas * averageItems);
          return Format.Name(drop.m_item, "white") + ": " + Format.Range(drop.m_stackMin, drop.m_stackMax) + " items (" + averageText + " on average)";
        });
        lines.AddRange(drops);
      }
      else
      {
        var dropChance = obj.m_dropChance == 1f ? "" : Format.Percent(obj.m_dropChance) + " chance for ";
        var dropText = "Drops: " + dropChance + Format.Range(obj.m_dropMin, obj.m_dropMax) + " drops";
        if (obj.m_oneOfEach)
          dropText += " (max one of each)";
        lines.Add(dropText);
        var averageDrops = obj.m_dropChance * (obj.m_dropMin + obj.m_dropMax) / 2.0;
        var weight = obj.m_drops.Sum(drop => drop.m_weight);
        var drops = obj.m_drops.Select(drop =>
        {
          var chance = weight > 0 ? drop.m_weight / weight : 1f;
          var averageItems = averageDrops * chance * (drop.m_stackMin + drop.m_stackMax) / 2.0;
          var chanceText = chance == 1f ? "" : Format.Percent(chance) + " chance for ";
          var averageText = Format.Float(averageItems);
          if (areas > 1)
            averageText += " * " + Format.Int(areas) + " = " + Format.Float(areas * averageItems);
          return Format.Name(drop.m_item, "white") + ": " + chanceText + Format.Range(drop.m_stackMin, drop.m_stackMax) + " items (" + averageText + " on average)";
        });
        lines.AddRange(drops);
      }
      return Format.JoinLines(lines);
    }
    public static string Get(TreeBase obj)
    {
      if (!Settings.destructibles || !obj) return "";
      var lines = new List<string>();
      var maxHealth = obj.m_health;
      var health = Patch.GetFloat(obj, "health", maxHealth);
      lines.Add(Format.GetHealth(health, maxHealth));
      lines.Add("Hit noise: " + Format.Int(100));
      if (obj.m_logPrefab)
        lines.Add("Destroy creates: " + Format.Name(obj.m_logPrefab));

      lines.Add(Texts.GetToolTier(obj.m_minToolTier, obj.m_damageModifiers.m_chop != HitData.DamageModifier.Immune, obj.m_damageModifiers.m_pickaxe != HitData.DamageModifier.Immune));
      lines.Add(DamageModifierUtils.Get(obj.m_damageModifiers, false, false));
      lines.Add(Get(obj.m_dropWhenDestroyed, 1));
      return Format.JoinLines(lines);
    }
    public static string Get(Destructible obj)
    {
      if (!Settings.destructibles || !obj) return "";
      var lines = new List<string>();
      var health = Patch.GetFloat(obj, "health", obj.m_health);
      var maxHealth = obj.m_health;
      lines.Add(Format.GetHealth(health, maxHealth));
      lines.Add("Hit noise: " + Format.Int(obj.m_hitNoise));
      lines.Add("Destroy noise: " + Format.Int(obj.m_destroyNoise));
      if (obj.m_spawnWhenDestroyed)
        lines.Add("Destroy creates: " + Format.Name(obj.m_spawnWhenDestroyed));
      lines.Add(Texts.GetToolTier(obj.m_minToolTier, obj.m_damages.m_chop != HitData.DamageModifier.Immune, obj.m_damages.m_pickaxe != HitData.DamageModifier.Immune));
      lines.Add(DamageModifierUtils.Get(obj.m_damages, false, false));
      return Format.JoinLines(lines);
    }
    public static string Get(DropOnDestroyed obj)
    {
      if (!Settings.destructibles || !obj) return "";
      var lines = new List<string>();
      lines.Add(Get(obj.m_dropWhenDestroyed, 1));
      return Format.JoinLines(lines);
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
      var lines = new List<string>();
      var health = obj.GetHealthPercentage();

      lines.Add(Format.GetHealth(health * obj.m_health, obj.m_health));
      lines.Add(DamageModifierUtils.Get(obj.m_damages, true, false));

      if (SupportUtils.Enabled(obj))
      {
        float maxSupport, minSupport, horizontalLoss, verticalLoss;
        Patch.WearNTear_GetMaterialProperties(obj, out maxSupport, out minSupport, out horizontalLoss, out verticalLoss);
        var support = Math.Min(Patch.GetFloat(obj, "support", maxSupport), maxSupport);
        lines.Add(Format.String(GetMaterialName(obj.m_materialType)) + ": " + Format.Progress(support, minSupport) + " support");
        lines.Add(Format.Percent(horizontalLoss) + " horizontal loss, " + Format.Percent(verticalLoss) + " vertical loss");
      }
      return Format.JoinLines(lines);
    }
    public static string Get(Piece obj)
    {
      if (!Settings.structures || !obj) return "";
      var text = "";
      if (obj.m_comfort > 0)
      {
        text += "Comfort: " + Format.Int(obj.m_comfort);
        if (obj.m_comfortGroup != Piece.ComfortGroup.None)
          text += " (" + Get(obj.m_comfortGroup) + ")";
      }
      return text;
    }
    public static string Get(Beehive obj)
    {
      if (!Settings.structures || !Settings.progress || !obj) return "";
      var lines = new List<string>();
      var limit = obj.m_secPerUnit;
      if (limit > 0)
      {
        var value = Patch.GetFloat(obj, "product");
        lines.Add(Format.ProgressPercent("Progress", value, limit));
      }
      lines.Add(GetCover(obj));
      return Format.JoinLines(lines);
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
      return Format.ProgressPercent("Progress", value, limit);
    }
    public static string Get(CookingStation obj)
    {
      if (!Settings.structures || !Settings.progress || !obj) return "";
      var lines = new List<string>();
      for (var slot = 0; slot < obj.m_slots.Length; slot++)
        lines.Add(GetSlotText(obj, slot));
      return Format.JoinLines(lines);
    }
    public static string Get(Fermenter obj)
    {
      if (!Settings.structures || !Settings.progress || !obj) return "";
      var lines = new List<string>();
      var limit = obj.m_fermentationDuration;
      if (limit > 0)
      {
        var value = Patch.Fermenter_GetFermentationTime(obj);
        lines.Add(Format.ProgressPercent("Progress", value, limit));
      }
      lines.Add(GetCover(obj));
      return Format.JoinLines(lines);
    }
    public static string GetCover(Fermenter obj)
    {
      if (!obj) return "";
      return GetCover(CoverUtils.GetCoverPoint(obj), Constants.CoverFermenterLimit);
    }
    public static string Get(SmokeSpawner obj)
    {
      if (!obj) return "";
      var lines = new List<string>();
      lines.Add(GetSmokeLimit());
      lines.Add("Produces smoke every " + Format.Float(obj.m_interval) + " s, unless smoke within " + Format.Float(obj.m_testRadius) + " m");
      return Format.JoinLines(lines);
    }
    public static string Get(Smoke obj)
    {
      if (!obj) return "";
      var lines = new List<string>();
      lines.Add(": " + Format.Progress(Smoke.GetTotalSmoke(), Constants.SmokeAmountLimit, true));
      lines.Add(Format.ProgressPercent("Expires", Patch.m_time(obj), obj.m_ttl));
      var collider = obj.GetComponent<SphereCollider>();
      if (collider)
        lines.Add("Radius: " + Format.Float(collider.radius * obj.transform.lossyScale.x));
      var body = Patch.m_body(obj);
      lines.Add("Mass: " + Format.Float(body.mass));
      lines.Add("Velocity: " + Format.String(body.velocity.ToString("F3")));
      var ratio = 1f - Mathf.Clamp01(Patch.m_time(obj) / obj.m_ttl);
      var vel = obj.m_vel;
      vel.y *= ratio;
      lines.Add("Target: " + Format.String(vel.ToString("F3")));
      return Format.JoinLines(lines);
    }
    private static string GetSmokeLimit() => "Smoke: " + Format.Progress(Smoke.GetTotalSmoke(), Constants.SmokeAmountLimit, true);
    public static string Get(Fireplace obj)
    {
      if (!Settings.structures || !Settings.progress || !obj) return "";
      var lines = new List<string>();
      var limit = obj.m_secPerFuel;
      if (limit > 0)
      {
        var value = Patch.GetFloat(obj, "fuel") * limit;
        lines.Add(Format.ProgressPercent("Progress", value, limit));
      }
      lines.Add(GetCover(obj));
      return Format.JoinLines(lines);
    }
    private static string GetWind(Fireplace obj)
    {
      if (!obj || !CoverUtils.ChecksCover(obj)) return "";
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
      var lines = new List<string>();
      if (CoverUtils.ChecksCover(obj)) lines.Add(GetCover(CoverUtils.GetCoverPoint(obj), Constants.CoverFireplaceLimit, false));
      lines.Add(GetWind(obj));
      lines.Add(GetDistanceFromRoof(obj));
      return Format.JoinLines(lines);
    }
    public static string Get(MineRock obj)
    {
      if (!Settings.destructibles || !obj) return "";
      var lines = new List<string>();
      var maxHealth = obj.m_health;
      var areas = Patch.m_hitAreas(obj);
      var index = 0;
      var remaining = areas.Count(area =>
      {
        var key = "Health" + index.ToString();
        index++;
        return Patch.GetFloat(obj, key, maxHealth) > 0;
      });
      lines.Add("Areas: " + Format.Progress(remaining, areas.Count));
      lines.Add("Health per area: " + Format.Int(maxHealth));
      lines.Add("Hit noise: " + Format.Int(100));
      lines.Add(Texts.GetToolTier(obj.m_minToolTier, obj.m_damageModifiers.m_chop != HitData.DamageModifier.Immune, obj.m_damageModifiers.m_pickaxe != HitData.DamageModifier.Immune));
      lines.Add(DamageModifierUtils.Get(obj.m_damageModifiers, false, false));
      lines.Add(Get(obj.m_dropItems, areas.Count));
      return Format.JoinLines(lines);
    }
    public static string Get(MineRock5 obj)
    {
      if (!Settings.destructibles || !obj) return "";
      var lines = new List<string>();
      var maxHealth = obj.m_health;
      var areas = Patch.m_hitAreas(obj);
      var remaining = areas.Count(area =>
      {
        var value = area.GetType().GetField("m_health").GetValue(area);
        return (float)value > 0f;
      });
      lines.Add("Areas: " + Format.Progress(remaining, areas.Count()));
      lines.Add("Health per area: " + Format.Int(maxHealth));
      lines.Add("Hit noise: " + Format.Int(100));
      lines.Add(Texts.GetToolTier(obj.m_minToolTier, obj.m_damageModifiers.m_chop != HitData.DamageModifier.Immune, obj.m_damageModifiers.m_pickaxe != HitData.DamageModifier.Immune));
      lines.Add(DamageModifierUtils.Get(obj.m_damageModifiers, false, false));
      lines.Add(Get(obj.m_dropItems, areas.Count()));
      return Format.JoinLines(lines);
    }
    public static string Get(Plant obj)
    {
      if (!Settings.progress || !obj) return "";
      var lines = new List<string>();
      var limit = Patch.Plant_GetGrowTime(obj);
      if (limit > 0)
      {
        var value = Patch.Plant_TimeSincePlanted(obj);
        lines.Add(Format.ProgressPercent("Progress", value, limit));
      }
      lines.Add("Radius: " + Format.Meters(obj.m_growRadius));
      lines.Add("Grows: " + Format.Name(obj.m_grownPrefabs));
      if (obj.m_destroyIfCantGrow)
        lines.Add("Destroyed if can't grow");
      return Format.JoinLines(lines);
    }
    public static string Get(EffectArea obj)
    {
      if (Settings.effectAreaLineWidth == 0 || !obj) return "";
      return EffectAreaUtils.GetTypeText(obj.m_type) + " " + Format.Radius(obj.GetRadius());
    }
    public static string Get(PrivateArea obj)
    {
      if (Settings.effectAreaLineWidth == 0 || !obj) return "";
      return "Protection " + Format.Radius(obj.m_radius);
    }

    private static string GetProgressText(Smelter instance)
    {
      var limit = instance.m_secPerProduct;
      if (limit == 0) return "";
      var value = Patch.Smelter_GetBakeTimer(instance);
      return Format.ProgressPercent("Progress", value, limit);
    }
    private static string GetFuelText(Smelter instance)
    {
      var maxFuel = instance.m_maxFuel;
      var secPerFuel = instance.m_secPerProduct / instance.m_fuelPerProduct;
      if (maxFuel == 0) return "";
      var limit = maxFuel * secPerFuel;
      if (limit == 0) return "";
      var value = Patch.Smelter_GetFuel(instance) * secPerFuel;
      return Format.ProgressPercent("Fuel", value, limit);
    }
    private static string GetPowerText(Windmill windmill)
    {
      if (!windmill) return "";
      var cover = Patch.m_cover(windmill);
      var speed = Utils.LerpStep(windmill.m_minWindSpeed, 1f, EnvMan.instance.GetWindIntensity());
      var powerText = "Power: " + Format.Percent(windmill.GetPowerOutput());
      var speedText = Format.Percent(speed) + " speed";
      var coverText = Format.Percent(cover) + " cover";
      return powerText + " from " + speedText + " and " + coverText;
    }
    public static string Get(Smelter obj)
    {
      if (!Settings.structures || !Settings.progress || !obj) return "";
      var lines = new List<string>();
      lines.Add(GetProgressText(obj));
      lines.Add(GetFuelText(obj));
      if (obj.m_windmill)
        lines.Add(GetPowerText(obj.m_windmill));
      else
        lines.Add(GetSmokeLimit());
      return Format.JoinLines(lines);
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
      var lines = new List<string>();
      var body = Patch.m_body(obj);
      var forwardSpeed = obj.GetSpeed();
      var forwardAngle = 90f - Mathf.Atan2(obj.transform.forward.z, obj.transform.forward.x) / Math.PI * 180f;
      if (forwardSpeed < 0)
        lines.Add("Speed: " + Format.Fixed(-forwardSpeed) + " m/s away from " + Format.Int(forwardAngle) + " degrees (backward)");
      else
        lines.Add("Speed: " + Format.Fixed(forwardSpeed) + " m/s towards " + Format.Int(forwardAngle) + " degrees (forward)");
      Vector3 velocity = body.velocity * 1f;
      velocity.y = 0f;
      var angle = 90f - Mathf.Atan2(velocity.z, velocity.x) / Math.PI * 180f;
      var speed = velocity.magnitude;
      lines.Add("Speed: " + Format.Fixed(speed) + " m/s towards " + Format.Int(angle) + " degrees");
      lines.Add(EnvUtils.GetWind());
      lines.Add("Wind power: " + Format.Percent(GetWindPower(obj)) + " from " + Format.Int(GetRelativeAngle(obj)) + " degrees");
      return Format.JoinLines(lines);
    }
    private static string GetCover(Vector3 startPos, double limit, bool checkRoof = true, bool minLimit = true)
    {
      var lines = new List<string>();
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
      lines.Add(text);
      if (checkRoof && !roof)
        lines.Add(Format.String("Not under roof!", "red"));
      if (Math.Abs(percent - (float)hits / total) > 0.01) lines.Add(Format.String("Error with cover calculation (" + percent + ")!", "red"));
      return Format.JoinLines(lines);
    }
    public static string Get(Bed obj)
    {
      if (!obj) return "";
      return GetCover(obj);
    }
    public static string Get(Container obj)
    {
      if (!obj) return "";
      return Get(obj.m_defaultItems, 1);
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

