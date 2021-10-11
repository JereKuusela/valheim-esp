using Authorization;
using BepInEx;
using HarmonyLib;
using Visualization;

namespace ESP {
  [BepInPlugin("valheim.jerekuusela.esp", "ESP", "1.5.0.0")]
  public class ESP : BaseUnityPlugin {
    public void Awake() {
      Settings.Init(Config);
      var harmony = new Harmony("valheim.jerekuusela.esp");
      harmony.PatchAll();
      Admin.Instance = new EspAdmin();
      SetupTagGroups();
    }

    private void SetupTagGroups() {
      Visibility.AddTag(Group.Creature, Tag.TrackedCreature);
      Visibility.AddTag(Group.Creature, Tag.CreatureAlertRange);
      Visibility.AddTag(Group.Creature, Tag.CreatureBreedingPartnerRange);
      Visibility.AddTag(Group.Creature, Tag.CreatureBreedingTotalRange);
      Visibility.AddTag(Group.Creature, Tag.CreatureEatingRange);
      Visibility.AddTag(Group.Creature, Tag.CreatureFireRange);
      Visibility.AddTag(Group.Creature, Tag.CreatureFoodSearchRange);
      Visibility.AddTag(Group.Creature, Tag.CreatureHearRange);
      Visibility.AddTag(Group.Creature, Tag.CreatureNoise);
      Visibility.AddTag(Group.Creature, Tag.CreatureViewRange);
      Visibility.AddTag(Group.Other, Tag.Chest);
      Visibility.AddTag(Group.Other, Tag.StructureCover);
      Visibility.AddTag(Group.Other, Tag.SpawnPoint);
      Visibility.AddTag(Group.Other, Tag.Destructible);
      Visibility.AddTag(Group.Other, Tag.EffectArea);
      Visibility.AddTag(Group.Other, Tag.Location);
      Visibility.AddTag(Group.Other, Tag.Ore);
      Visibility.AddTag(Group.Other, Tag.Pickable);
      Visibility.AddTag(Group.Zone, Tag.RandomEventSystem);
      Visibility.AddTag(Group.Other, Tag.Smoke);
      Visibility.AddTag(Group.Other, Tag.Spawner);
      Visibility.AddTag(Group.Zone, Tag.SpawnZone);
      Visibility.AddTag(Group.Other, Tag.StructureSupport);
      Visibility.AddTag(Group.Other, Tag.Tree);
      Visibility.AddTag(Group.Zone, Tag.ZoneCorner);
    }
    public void LateUpdate() {
      if (Player.m_localPlayer)
        Texts.UpdateAverageSpeed(Ship.GetLocalShip());
    }
  }

  [HarmonyPatch(typeof(Chat), "Awake")]
  public class ChatBind {
    public static void Postfix(Console __instance) {
      if (!Settings.configFirstRun.Value) return;
      Settings.configFirstRun.Value = false;
      var binds = Patch.BindList(__instance);
      if (binds == null) UnityEngine.Debug.LogError("No binds!");
      while (true) {
        var index = binds.FindIndex(item => item.Contains("esp_"));
        if (index == -1) break;
        binds.RemoveAt(index);
      }
      __instance.TryRunCommand("bind y esp_toggle " + Group.Zone);
      __instance.TryRunCommand("bind u esp_toggle " + Group.Creature);
      __instance.TryRunCommand("bind i esp_toggle " + Group.Other);
      __instance.TryRunCommand("bind p esp_toggle " + Tool.DPS);
      __instance.TryRunCommand("bind p esp_toggle " + Tool.Experience);
      __instance.TryRunCommand("bind o esp_toggle " + Tool.ExtraInfo);
      __instance.TryRunCommand("bind j esp_toggle " + Tool.Ruler);
    }
  }
}
