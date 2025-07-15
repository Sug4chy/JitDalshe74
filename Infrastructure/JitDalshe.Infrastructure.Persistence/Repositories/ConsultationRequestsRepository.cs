using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;

namespace JitDalshe.Infrastructure.Persistence.Repositories;

[Repository]
internal sealed class ConsultationRequestsRepository : IConsultationRequestsRepository
{
    private readonly PostgresqlDbContext _db;

    public ConsultationRequestsRepository(PostgresqlDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(ConsultationRequest request, CancellationToken ct = default)
    {
        _db.ConsultationRequests.Add(request);
        await _db.SaveChangesAsync(ct);
    }
}