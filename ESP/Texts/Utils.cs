using UnityEngine;

namespace ESP {
  public partial class Texts {
    public static bool IsValid(MonoBehaviour obj) {
      if (!obj) return false;
      var nView = Patch.Nview(obj);
      if (!nView) return false;
      return nView.IsValid();
    }
  }
}