using System;
using System.Collections.Generic;
using ESP;
using UnityEngine;
namespace Visualization;

public abstract class BaseRuler : MonoBehaviour
{
  private static LayerMask Mask = LayerMask.GetMask("terrain");
  private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");
  // No direct way for the projector prefab, so it must be acquired from some object.
  private static GameObject? basePrefab;
  private static GameObject BasePrefab => basePrefab ??= GetBasePrefab();
  protected static Vector3 BaseScale => BasePrefab.transform.localScale;
  private static GameObject GetBasePrefab()
  {
    var workbench = ZNetScene.instance.GetPrefab("piece_workbench");
    if (!workbench) throw new InvalidOperationException("Error: Unable to find the workbench object.");
    return workbench.GetComponentInChildren<CircleProjector>().m_prefab;
  }

  public static bool SnapToGround = true;
  public static bool Visible = true;

  public void OnDestroy()
  {
    foreach (GameObject obj in Segments) Destroy(obj);
    Segments.Clear();
  }
  public void Update()
  {
    if (!Visible)
    {
      CreateSegments(0);
      return;
    }
    CreateLines();
    if (SnapToGround)
      Snap();
  }

  protected List<GameObject> Segments = [];
  protected void CreateSegments(int count)
  {
    if (Segments.Count == count) return;
    foreach (GameObject obj in Segments) Destroy(obj);
    Segments.Clear();
    if (count == 0) return;
    for (int i = 0; i < count; i++)
      Segments.Add(Instantiate(BasePrefab, transform));
    ApplyColor();
  }


  internal void ApplyColor()
  {
    var color = Draw.GetColor(Tag.EffectAreaPlayerBase);
    foreach (var segment in Segments)
      ApplyColor(segment, color);
  }

  private static void ApplyColor(GameObject obj, Color color)
  {
    var renderer = obj.GetComponent<Renderer>();
    if (renderer)
    {
      var materials = renderer.materials;
      for (int i = 0; i < materials.Length; i++)
        materials[i].SetColor(ColorPropertyId, color);
    }
  }


  protected abstract void CreateLines();
  private void Snap()
  {
    foreach (var segment in Segments)
      segment.transform.position = Snap(segment.transform.position);
  }
  private Vector3 Snap(Vector3 pos)
  {
    if (Physics.Raycast(pos + Vector3.up * 500f, Vector3.down, out var raycastHit, 1000f, Mask.value))
      pos.y = raycastHit.point.y;
    return pos;
  }


  // Helper functions for the derived classes.
  protected Transform Get(int index) => Segments[index].transform;
  protected void Set(int index, Vector3 pos) => Get(index).localPosition = pos;
  protected void SetRot(int index, Vector3 rot) => Get(index).localRotation = Quaternion.LookRotation(rot, Vector3.up);
}

public class CircleRuler : BaseRuler
{

  public void Awake() => CircleAreaManager.Register(this, transform.position, radius);
  public new void OnDestroy()
  {
    CircleAreaManager.Unregister(this);
    base.OnDestroy();
  }

  [SerializeField]
  private float radius = 20f;
  public float Radius
  {
    get => radius;
    set
    {
      radius = value;
      CircleAreaManager.UpdateRadius(this, radius, transform.position);
    }
  }

  protected override void CreateLines()
  {
    var count = Math.Max(3, (int)(Radius * 4));
    CreateSegments(count);
    UpdateLines();
  }
  private readonly float Speed = 0.1f;
  private void UpdateLines()
  {
    var count = Segments.Count;
    var angle = 6.2831855f / count;
    var time = Time.time * Speed;
    for (int i = 0; i < count; i++)
    {
      var f = i * angle + time;
      var vector = new Vector3(Mathf.Sin(f) * Radius, 0f, Mathf.Cos(f) * Radius);
      Set(i, vector);
    }
    for (int i = 0; i < count; i++)
    {
      var prev = i == 0 ? Segments[Segments.Count - 1] : Segments[i - 1];
      var next = i == Segments.Count - 1 ? Segments[0] : Segments[i + 1];
      Vector3 normalized = (prev.transform.localPosition - next.transform.localPosition).normalized;
      SetRot(i, normalized);
    }

    for (int i = 0; i < count; i++)
      Segments[i].SetActive(!CircleAreaManager.IsInsideOtherCircle(this, Segments[i].transform.position));

  }
}
