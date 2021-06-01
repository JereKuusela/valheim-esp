using HarmonyLib;

namespace ESP
{

  [HarmonyPatch(typeof(MessageHud), "UpdateMessage")]
  public class MessageHud_UpdateMessage
  {
    private static string forcedMessage = "";
    public static string ForcedMessage
    {
      get
      {
        return forcedMessage;
      }
      set
      {
        if (MessageHud.instance && MessageHud.instance.m_messageText)
        {
          if (value.Length == 0)
            MessageHud.instance.m_messageText.CrossFadeAlpha(0f, 4f, true);
          else
            MessageHud.instance.m_messageText.CrossFadeAlpha(1f, 0f, true);
        }
        forcedMessage = value;
      }
    }

    public static bool Prefix()
    {
      var hud = MessageHud.instance;
      if (ForcedMessage.Length == 0 || !hud) return true;
      if (!hud.m_messageText)
      {
        hud.ShowMessage(MessageHud.MessageType.TopLeft, ForcedMessage);
        return true;
      }
      hud.m_messageText.text = ForcedMessage;
      return false;
    }
  }
}