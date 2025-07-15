using JitDalshe.Domain.Entities.Consultations;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface IConsultationRequestsRepository
{
    Task AddAsync(ConsultationRequest request, CancellationToken ct = default);
}