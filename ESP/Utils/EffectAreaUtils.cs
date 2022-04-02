using System;
using System.Collections.Generic;
using Service;
namespace ESP;
public class EffectAreaUtils {

  public static String GetTypeText(EffectArea.Type type) {
    List<string> types = new();
    if ((type & EffectArea.Type.Burning) != 0) types.Add("Burning");
    if ((type & EffectArea.Type.Heat) != 0) types.Add("Heat");
    if ((type & EffectArea.Type.Fire) != 0) types.Add("Fire");
    if ((type & EffectArea.Type.NoMonsters) != 0) types.Add("No monsters");
    if ((type & EffectArea.Type.Teleport) != 0) types.Add("Teleport");
    if ((type & EffectArea.Type.PlayerBase) != 0) types.Add("Base");
    if ((type & EffectArea.Type.WarmCozyArea) != 0) types.Add("Warm and cozy");
    return Format.JoinRow(types);
  }
}
