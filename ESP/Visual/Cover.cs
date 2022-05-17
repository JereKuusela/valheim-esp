using Service;
using UnityEngine;
using Visualization;
namespace ESP;
public partial class Visual {
  private static void DrawCover(MonoBehaviour obj, Vector3 startPos, string name, string text, bool isPlayer = false) {
    var delta = startPos - obj.transform.position;
    var start = Constants.CoverRaycastStart;
    if (Cover.m_coverRays == null) Cover.Setup();
    if (Cover.m_coverRays == null) return;
    foreach (var vector in Cover.m_coverRays) {
      var tag = isPlayer ? Tag.PlayerCover : Tag.StructureCover;
      if (Physics.Raycast(startPos + vector * start, vector, out var raycastHit, Constants.CoverRayCastLength - start, Cover.m_coverRayMask))
        tag = isPlayer ? Tag.PlayerCoverBlocked : Tag.StructureCoverBlocked;
      var line = Visualization.Draw.DrawLineWithFixedRotation(tag, obj, delta + vector * start, delta + vector * Constants.CoverRayCastLength);
      Visualization.Draw.AddText(line, name, text);
    }
  }
  public static void DrawCover(CraftingStation obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover)) return;
    DrawCover(obj, CoverUtils.GetCoverPoint(obj), Translate.Name(obj), Texts.GetCover(obj));
  }
  public static void Draw(Beehive obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover)) return;
    DrawCover(obj, CoverUtils.GetCoverPoint(obj), Translate.Name(obj), Texts.GetCover(obj));
  }
  public static void Draw(Fireplace obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover) || !CoverUtils.ChecksCover(obj)) return;
    DrawCover(obj, CoverUtils.GetCoverPoint(obj), Translate.Name(obj), Texts.GetCover(obj));
  }
  public static void Draw(Bed obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover)) return;
    DrawCover(obj, CoverUtils.GetCoverPoint(obj), Translate.Name(obj), Texts.GetCover(obj));
  }
  public static void Draw(Fermenter obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover)) return;
    DrawCover(obj, CoverUtils.GetCoverPoint(obj), Translate.Name(obj), Texts.GetCover(obj));
  }
  public static void Draw(Windmill obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover)) return;
    DrawCover(obj, CoverUtils.GetCoverPoint(obj), Translate.Name(obj), Texts.GetCover(obj));
  }
  public static void DrawCover(Player obj) {
    if (!Helper.IsValid(obj) || Settings.IsDisabled(Tag.PlayerCover)) return;
    DrawCover(obj, CoverUtils.GetCoverPoint(obj), Translate.Name(obj), Texts.GetCover(obj), true);
  }
  private static void UpdateCover(MonoBehaviour obj, Vector3 startPos, string text, bool isPlayer = false) {
    var tags = isPlayer ? new string[] { Tag.PlayerCover, Tag.PlayerCoverBlocked } : new string[] { Tag.StructureCover, Tag.StructureCoverBlocked };
    var renderers = Visualization.Draw.GetRenderers(obj, tags);
    var vectors = Cover.m_coverRays;
    if (renderers.Length != vectors.Length) return;
    var start = Constants.CoverRaycastStart;
    var index = 0;
    foreach (var vector in vectors) {
      var tag = isPlayer ? Tag.PlayerCover : Tag.StructureCover;
      RaycastHit raycastHit;
      if (Physics.Raycast(startPos + vector * start, vector, out raycastHit, Constants.CoverRayCastLength - start, Cover.m_coverRayMask))
        tag = isPlayer ? Tag.PlayerCoverBlocked : Tag.StructureCoverBlocked;
      var renderer = renderers[index];
      var color = Visualization.Draw.GetColor(tag);
      renderer.material.SetColor("_Color", color);
      var staticText = renderer.GetComponent<StaticText>();
      if (staticText) staticText.text = text;
      index++;
    }
  }
  public static void Update(CraftingStation obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover)) return;
    UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
  }
  public static void Update(Beehive obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover)) return;
    UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
  }
  public static void Update(Fireplace obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover)) return;
    UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
  }
  public static void Update(Bed obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover)) return;
    UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
  }
  public static void Update(Fermenter obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover)) return;
    UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
  }
  public static void Update(Windmill obj) {
    if (!obj || Settings.IsDisabled(Tag.StructureCover)) return;
    UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
  }
  public static void Update(Player obj) {
    if (!Helper.IsValid(obj) || Settings.IsDisabled(Tag.PlayerCover)) return;
    UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj), true);
  }
}
