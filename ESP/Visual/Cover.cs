using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{
  public partial class Visual
  {
    private static void DrawCover(MonoBehaviour obj, Vector3 startPos, string name, string text)
    {
      var delta = startPos - obj.transform.position;
      var start = Constants.CoverRaycastStart;
      var cover = new Cover();
      foreach (var vector in Patch.m_coverRays(cover))
      {
        var color = Color.green;
        if (Physics.Raycast(startPos + vector * start, vector, out var raycastHit, Constants.CoverRayCastLength - start, Patch.m_coverRayMask(cover)))
          color = Color.red;
        var line = Drawer.DrawLineWithFixedRotation(obj, delta + vector * start, delta + vector * Constants.CoverRayCastLength, color, Settings.coverRayWidth, Drawer.OTHER);
        Drawer.AddText(line, name, text);
        Drawer.AddTag(line, Constants.CoverTag);
      }
    }
    public static void Draw(CraftingStation obj)
    {
      if (!obj || Settings.coverRayWidth == 0) return;
      DrawCover(obj, CoverUtils.GetCoverPoint(obj), Format.Name(obj), Texts.GetCover(obj));
    }
    public static void Draw(Beehive obj)
    {
      if (!obj || Settings.coverRayWidth == 0) return;
      DrawCover(obj, CoverUtils.GetCoverPoint(obj), Format.Name(obj), Texts.GetCover(obj));
    }
    public static void Draw(Fireplace obj)
    {
      if (!obj || Settings.coverRayWidth == 0 || !CoverUtils.ChecksCover(obj)) return;
      DrawCover(obj, CoverUtils.GetCoverPoint(obj), Format.Name(obj), Texts.GetCover(obj));
    }
    public static void Draw(Bed obj)
    {
      if (!obj || Settings.coverRayWidth == 0) return;
      DrawCover(obj, CoverUtils.GetCoverPoint(obj), Format.Name(obj), Texts.GetCover(obj));
    }
    public static void Draw(Fermenter obj)
    {
      if (!obj || Settings.coverRayWidth == 0) return;
      DrawCover(obj, CoverUtils.GetCoverPoint(obj), Format.Name(obj), Texts.GetCover(obj));
    }
    public static void Draw(Windmill obj)
    {
      if (!obj || Settings.coverRayWidth == 0) return;
      DrawCover(obj, CoverUtils.GetCoverPoint(obj), Format.Name(obj), Texts.GetCover(obj));
    }
    private static void UpdateCover(MonoBehaviour obj, Vector3 startPos, string text)
    {
      var renderers = Drawer.GetRenderers(obj, Constants.CoverTag);
      var cover = new Cover();
      var vectors = Patch.m_coverRays(cover);
      if (renderers.Length != vectors.Length) return;
      var start = Constants.CoverRaycastStart;
      var index = 0;
      foreach (var vector in vectors)
      {
        RaycastHit raycastHit;
        var color = Color.green;
        if (Physics.Raycast(startPos + vector * start, vector, out raycastHit, Constants.CoverRayCastLength - start, Patch.m_coverRayMask(cover)))
          color = Color.red;
        var renderer = renderers[index];
        renderer.material.SetColor("_Color", color);
        var staticText = renderer.GetComponent<StaticText>();
        if (staticText) staticText.text = text;
        index++;
      }
    }
    public static void Update(CraftingStation obj)
    {
      if (!obj || Settings.coverRayWidth == 0) return;
      UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
    }
    public static void Update(Beehive obj)
    {
      if (!obj || Settings.coverRayWidth == 0) return;
      UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
    }
    public static void Update(Fireplace obj)
    {
      if (!obj || Settings.coverRayWidth == 0) return;
      UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
    }
    public static void Update(Bed obj)
    {
      if (!obj || Settings.coverRayWidth == 0) return;
      UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
    }
    public static void Update(Fermenter obj)
    {
      if (!obj || Settings.coverRayWidth == 0) return;
      UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
    }
    public static void Update(Windmill obj)
    {
      if (!obj || Settings.coverRayWidth == 0) return;
      UpdateCover(obj, CoverUtils.GetCoverPoint(obj), Texts.GetCover(obj));
    }
  }
}