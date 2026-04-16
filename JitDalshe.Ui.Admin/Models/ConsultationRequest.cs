namespace JitDalshe.Ui.Admin.Models;

public sealed record ConsultationRequest(
    Guid Id,
    string PatientName,
    int PatientAge,
    string? PatientPhoneNumber,
    string? PatientEmail,
    bool IsHandled,
    PatientCommunicationMethod CommunicationMethods,
    DateOnly Date
)
{
    public string StatusString => IsHandled ? "Обработана" : "Новая";
    
    public string CommunicationMethodsString
    {
        get
        {
            var methods = new List<string>();
            
            if (CommunicationMethods.HasFlag(PatientCommunicationMethod.CallAPhone)) 
                methods.Add("Звонок");
            
            if (CommunicationMethods.HasFlag(PatientCommunicationMethod.WhatsApp)) 
                methods.Add("WhatsApp");
            
            if (CommunicationMethods.HasFlag(PatientCommunicationMethod.Telegram)) 
                methods.Add("Telegram");
            
            if (CommunicationMethods.HasFlag(PatientCommunicationMethod.Email)) 
                methods.Add("Email");

            return methods.Count > 0 ? string.Join(", ", methods) : "Не выбран";
        }
    }
}

[Flags]
public enum PatientCommunicationMethod
{
    CallAPhone = 1,
    WhatsApp = 2,
    Telegram = 4,
    Email = 8
}
