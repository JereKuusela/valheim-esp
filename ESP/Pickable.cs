using HarmonyLib;
using UnityEngine;
using System;

namespace ESP
{

  [HarmonyPatch(typeof(Pickable), "Awake")]
  public class Pickabler_Awake
  {
    private static bool IsEnabled(Pickable instance)
    {
      if (!Settings.showPickables) return false;
      var name = instance.m_itemPrefab.name.ToLower();
      var excluded = Settings.excludedPickables.ToLower().Split(',');
      if (Array.Exists(excluded, item => item == name)) return false;
      return true;
    }
    private static Color GetColor(Pickable instance)
    {
      return instance.m_hideWhenPicked && instance.m_respawnTimeMinutes > 0 ? Color.green : Color.blue;
    }
    public static void Postfix(Pickable __instance, ZNetView ___m_nview)
    {
      if (!IsEnabled(__instance))
        return;
      var color = GetColor(__instance);
      Action<GameObject> action = (GameObject obj) =>
        {
          var text = obj.AddComponent<PickableText>();
          text.pickable = __instance;
          text.nview = ___m_nview;
        };
      Drawer.DrawMarkerLine(__instance.gameObject, Vector3.zero, color, Settings.pickableRayWidth, action);
    }
  }

  public class PickableText : MonoBehaviour, Hoverable
  {
    private String GetRespawnTime()
    {
      if (!pickable.m_hideWhenPicked || pickable.m_respawnTimeMinutes == 0) return "Never";
      DateTime time = ZNet.instance.GetTime();
      DateTime d = new DateTime(nview.GetZDO().GetLong("picked_time", 0L));
      var timer = (time - d).TotalMinutes;
      var picked = nview.GetZDO().GetBool("picked", false); ;
      var timerString = picked ? timer.ToString("N0") : "Not picked";
      return timerString + " / " + pickable.m_respawnTimeMinutes.ToString("N0") + " minutes";
    }
    public string GetHoverText()
    {
      var respawn = GetRespawnTime();
      var lines = new string[]{
        TextUtils.String(pickable.m_itemPrefab.name),
        "Respawn: " + TextUtils.String(respawn)
      };
      if (pickable.m_amount > 0)
      {
        lines.AddItem("Amount: " + TextUtils.String(pickable.m_amount.ToString()));
      }
      return lines.Join(null, "\n");
    }
    public string GetHoverName() => pickable.m_itemPrefab.name;

    public Pickable pickable;
    public ZNetView nview;
  }
}