using Authorization;
using UnityEngine;

namespace ESP {
  public partial class Settings {
    public const string OTHER = "ESP_Other";
    public const string ZONE = "ESP_Zone";
    public const string CREATURE = "ESP_Creature";
    ///<summary>Setting for other visual visibility. Forced to off for non-admins on servers.</summary>
    public static bool showOthers {
      get => Settings.ShowOthers && Admin.Enabled;
      set {
        if (value)
          Admin.Check();
        Settings.configShowOthers.Value = value;
      }
    }
    ///<summary>Setting for creature visual visibility. Forced to off for non-admins on servers.</summary>
    public static bool showCreatures {
      get => Settings.ShowCreatures && Admin.Enabled;
      set {
        if (value)
          Admin.Check();
        Settings.configShowCreatures.Value = value;
      }
    }
    ///<summary>Setting for zone visual visibility. Forced to off for non-admins on servers.</summary>
    public static bool showZones {
      get => Settings.ShowZones && Admin.Enabled;
      set {
        if (value)
          Admin.Check();
        Settings.configShowZones.Value = value;
      }
    }
    ///<summary>Toggles visibility of other visuals.</summary>
    public static void ToggleOtherVisibility() {
      showOthers = !showOthers;
      CheckVisibility(OTHER);
    }
    ///<summary>Toggles visibility of zone visuals.</summary>
    public static void ToggleZoneVisibility() {
      showZones = !showZones;
      CheckVisibility(ZONE);
    }
    ///<summary>Toggles visibility of creature visuals.</summary>
    public static void ToggleCreatureVisibility() {
      showCreatures = !showCreatures;
      CheckVisibility(CREATURE);
    }
    ///<summary>Checks visibility of all visuals.</summary>
    public static void CheckVisibility() {
      CheckVisibility(OTHER);
      CheckVisibility(CREATURE);
      CheckVisibility(ZONE);
    }

    ///<summary>Checks visibility of a given visual type.</summary>
    private static void CheckVisibility(string name) {
      var activate = IsShown(name);
      foreach (var gameObj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
        if (gameObj.name == name && gameObj.activeSelf != activate)
          gameObj.SetActive(activate);
      }
    }
    ///<summary>Returns whether a given visual type is shown.</summary>
    private static bool IsShown(string name) {
      if (name == ZONE) return showZones;
      if (name == CREATURE) return showCreatures;
      return showOthers;
    }
  }
}