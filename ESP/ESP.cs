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
      Visibility.AddTagToGroup(Group.Creature, Tag.TrackedCreature);
      Visibility.AddTagToGroup(Group.Creature, Tag.CreatureAlertRange);
      Visibility.AddTagToGroup(Group.Creature, Tag.CreatureBreedingPartnerRange);
      Visibility.AddTagToGroup(Group.Creature, Tag.CreatureBreedingTotalRange);
      Visibility.AddTagToGroup(Group.Creature, Tag.CreatureEatingRange);
      Visibility.AddTagToGroup(Group.Creature, Tag.CreatureFireRange);
      Visibility.AddTagToGroup(Group.Creature, Tag.CreatureFoodSearchRange);
      Visibility.AddTagToGroup(Group.Creature, Tag.CreatureHearRange);
      Visibility.AddTagToGroup(Group.Creature, Tag.CreatureNoise);
      Visibility.AddTagToGroup(Group.Creature, Tag.CreatureViewRange);
      Visibility.AddTagToGroup(Group.Other, Tag.Chest);
      Visibility.AddTagToGroup(Group.Other, Tag.StructureCover);
      Visibility.AddTagToGroup(Group.Other, Tag.SpawnPoint);
      Visibility.AddTagToGroup(Group.Other, Tag.Destructible);
      Visibility.AddTagToGroup(Group.Other, Tag.EffectArea);
      Visibility.AddTagToGroup(Group.Other, Tag.Location);
      Visibility.AddTagToGroup(Group.Other, Tag.Ore);
      Visibility.AddTagToGroup(Group.Other, Tag.Pickable);
      Visibility.AddTagToGroup(Group.Zone, Tag.RandomEventSystem);
      Visibility.AddTagToGroup(Group.Other, Tag.Smoke);
      Visibility.AddTagToGroup(Group.Other, Tag.Spawner);
      Visibility.AddTagToGroup(Group.Zone, Tag.SpawnZone);
      Visibility.AddTagToGroup(Group.Other, Tag.StructureSupport);
      Visibility.AddTagToGroup(Group.Other, Tag.Tree);
      Visibility.AddTagToGroup(Group.Zone, Tag.ZoneCorner);
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
