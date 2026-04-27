namespace JitDalshe.Domain.Common;

[Flags]
public enum CommunicationMethod
{
    CallAPhone = 1,
    WhatsApp = 2,
    Telegram = 4,
    Email = 8
}