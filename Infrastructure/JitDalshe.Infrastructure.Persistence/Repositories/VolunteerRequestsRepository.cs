using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Repositories;


[Repository]
public class VolunteerRequestsRepository(PostgresqlDbContext dbContext)
    : BaseRepository<VolunteerRequest>(dbContext), IVolunteerRequestsRepository;