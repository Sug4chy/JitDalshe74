using JitDalshe.Ui.Admin.Models.Common;

namespace JitDalshe.Ui.Admin.Models;


public sealed record SupportGroupRequest(
    Guid Id,
    string ApplicantName,
    int ApplicantAge,
    string? ApplicantPhoneNumber,
    string? ApplicantEmail,
    RequestStatus Status,
    CommunicationMethod CommunicationMethods,
    DateOnly Date,
    string? Comment
)
{
    public string CommunicationMethodsString
    {
        get
        {
            var methods = new List<string>();
            
            if (CommunicationMethods.HasFlag(CommunicationMethod.CallAPhone)) 
                methods.Add("Звонок");
            
            if (CommunicationMethods.HasFlag(CommunicationMethod.WhatsApp)) 
                methods.Add("WhatsApp");
            
            if (CommunicationMethods.HasFlag(CommunicationMethod.Telegram)) 
                methods.Add("Telegram");
            
            if (CommunicationMethods.HasFlag(CommunicationMethod.Email)) 
                methods.Add("Email");

            return methods.Count > 0 ? string.Join(", ", methods) : "Не выбран";
        }
    }
}