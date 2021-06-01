using HarmonyLib;

namespace ESP
{

  [HarmonyPatch(typeof(MessageHud), "UpdateMessage")]
  public class MessageHud_UpdateMessage
  {
    private static string forcedMessage;
    public static string ForcedMessage
    {
      get
      {
        return forcedMessage;
      }
      set
      {
        if (value.Length == 0)
          MessageHud.instance.m_messageText.CrossFadeAlpha(0f, 4f, true);
        else
          MessageHud.instance.m_messageText.CrossFadeAlpha(1f, 0f, true);
        forcedMessage = value;
      }
    }

    public static bool Prefix()
    {
      if (ForcedMessage.Length == 0) return true;
      MessageHud.instance.m_messageText.text = ForcedMessage;
      return false;
    }
  }
}