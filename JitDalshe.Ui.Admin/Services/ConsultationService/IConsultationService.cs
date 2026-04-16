using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.ConsultationService;

public interface IConsultationService
{
    Task<ConsultationRequest[]> ListAsync();
    Task ToggleStatusAsync(Guid id, Func<Task>? onSuccess = null);
}