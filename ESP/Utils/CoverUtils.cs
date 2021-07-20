using UnityEngine;

namespace ESP
{
  public static class CoverUtils
  {
    public static Vector3 GetCoverPoint(CraftingStation obj) => obj.m_roofCheckPoint.position;
    public static Vector3 GetCoverPoint(Fermenter obj) => obj.m_roofCheckPoint.position;
    public static Vector3 GetCoverPoint(Beehive obj) => obj.m_coverPoint.position;
    public static Vector3 GetCoverPoint(Fireplace obj) => obj.transform.position + Vector3.up * obj.m_coverCheckOffset;
    public static Vector3 GetCoverPoint(Bed obj) => obj.GetSpawnPoint();
    public static Vector3 GetCoverPoint(Windmill obj) => obj.m_propeller.transform.position;
    public static bool ChecksCover(Fireplace obj) => obj.m_enabledObjectLow != null && obj.m_enabledObjectHigh != null;
  }
}