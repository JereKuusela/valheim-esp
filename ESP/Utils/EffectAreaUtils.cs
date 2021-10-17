using System;
using System.Collections.Generic;
using Service;
using UnityEngine;

namespace ESP {
  public class EffectAreaUtils {

    public static Color GetEffectColor(EffectArea.Type type) {
      if ((type & EffectArea.Type.Burning) != 0) return Settings.EffectAreaBurningColor;
      if ((type & EffectArea.Type.Heat) != 0) return Settings.EffectAreaHeatColor;
      if ((type & EffectArea.Type.Fire) != 0) return Settings.EffectAreaFireColor;
      if ((type & EffectArea.Type.NoMonsters) != 0) return Settings.EffectAreaNoMonstersColor;
      if ((type & EffectArea.Type.Teleport) != 0) return Settings.EffectAreaTeleportColor;
      if ((type & EffectArea.Type.PlayerBase) != 0) return Settings.EffectAreaPlayerBaseColor;
      return Settings.EffectAreaOtherColor;
    }
    public static String GetTypeText(EffectArea.Type type) {
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

