using UnityEngine;
using System;
using System.Collections.Generic;

namespace ESP
{
  public class EffectAreaUtils
  {

    public static Color GetEffectColor(EffectArea.Type type)
    {
      if ((type & EffectArea.Type.Burning) != 0) return Color.yellow;
      if ((type & EffectArea.Type.Heat) != 0) return Color.magenta;
      if ((type & EffectArea.Type.Fire) != 0) return Color.red;
      if ((type & EffectArea.Type.NoMonsters) != 0) return Color.green;
      if ((type & EffectArea.Type.Teleport) != 0) return Color.blue;
      if ((type & EffectArea.Type.PlayerBase) != 0) return Color.white;
      return Color.black;
    }
    public static String GetTypeText(EffectArea.Type type)
    {
      var types = new List<string>();
      if ((type & EffectArea.Type.Burning) != 0) types.Add("Burning");
      if ((type & EffectArea.Type.Heat) != 0) types.Add("Heat");
      if ((type & EffectArea.Type.Fire) != 0) types.Add("Fire");
      if ((type & EffectArea.Type.NoMonsters) != 0) types.Add("No monsters");
      if ((type & EffectArea.Type.Teleport) != 0) types.Add("Teleport");
      if ((type & EffectArea.Type.PlayerBase) != 0) types.Add("Base");
      return string.Join(", ", types);
    }
  }
}

