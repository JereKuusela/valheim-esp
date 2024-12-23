using System.Linq;
using Data;
using Service;
namespace ESP;
public partial class Texts
{
  public static string Get(string id, ZDO zdo)
  {
    var lines = FormatLoading.Get(Settings.Custom, zdo.m_prefab);
    if (lines == null || lines.Count == 0) return "";
    ObjectParameters pars = new ObjectParameters(id, "", zdo);
    var formatted = lines.Select(pars.Replace);
    return Format.JoinLines(formatted);
  }
}