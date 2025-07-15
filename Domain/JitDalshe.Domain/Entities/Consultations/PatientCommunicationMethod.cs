namespace JitDalshe.Domain.Entities.Consultations;

[Flags]
public enum PatientCommunicationMethod
{
    CallAPhone = 1,
    WhatsApp = 2,
    Telegram = 4,
    Email = 8
}