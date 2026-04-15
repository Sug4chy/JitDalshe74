using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface IConsultationRequestsRepository
{
    Task<ConsultationRequest[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<ConsultationRequest, bool>>? filteringExpression = null,
        Expression<Func<ConsultationRequest, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default);
    
    Task<Maybe<ConsultationRequest>> FindByIdAsync(IdOf<ConsultationRequest> id, CancellationToken ct = default);
    
    Task AddAsync(ConsultationRequest request, CancellationToken ct = default);
    
    Task EditAsync(ConsultationRequest request, CancellationToken ct = default);
}