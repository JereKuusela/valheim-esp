using UnityEngine;
using System;
using System.Collections.Generic;

namespace ESP
{
  public class EffectAreaUtils
  {

    public static Color GetEffectColor(EffectArea.Type type)
    {
      if ((type & EffectArea.Type.Burning) != 0) return Settings.effectAreaBurningColor;
      if ((type & EffectArea.Type.Heat) != 0) return Settings.effectAreaHeatColor;
      if ((type & EffectArea.Type.Fire) != 0) return Settings.effectAreaFireColor;
      if ((type & EffectArea.Type.NoMonsters) != 0) return Settings.effectAreaNoMonstersColor;
      if ((type & EffectArea.Type.Teleport) != 0) return Settings.effectAreaTeleportColor;
      if ((type & EffectArea.Type.PlayerBase) != 0) return Settings.effectAreaPlayerBaseColor;
      return Settings.effectAreaOtherColor;
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
      return Format.JoinRow(types);
    }
  }
}

