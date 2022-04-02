using Service;
using Visualization;
namespace ESP;
public class EspAdmin : DefaultAdmin {
  public override bool Enabled {
    get => base.Enabled;
    set {
      base.Enabled = value;
      Visibility.Set(value);
      SupportUtils.UpdateVisibility();
    }
  }
}
