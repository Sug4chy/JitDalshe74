using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Repositories;

[Repository]
internal sealed class ConsultationRequestsRepository(PostgresqlDbContext dbContext)
    : BaseRepository<ConsultationRequest>(dbContext), IConsultationRequestsRepository;