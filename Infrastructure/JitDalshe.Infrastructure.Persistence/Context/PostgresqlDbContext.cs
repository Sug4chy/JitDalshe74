using JitDalshe.Application.Entities;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.Entities.SupportGroups;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Converters;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Context;

public sealed class PostgresqlDbContext(DbContextOptions<PostgresqlDbContext> options) : DbContext(options)
{
    public DbSet<News> News { get; init; }
    public DbSet<NewsImage> NewsImages { get; init; }
    public DbSet<NewsPrimaryImage>  NewsPrimaryImages { get; init; }
    public DbSet<Event> Events { get; init; }
    public DbSet<EventImage> EventImages { get; init; }
    public DbSet<Banner> Banners { get; init; }
    public DbSet<BannerImage> BannerImages { get; init; }
    public DbSet<BannerMobileImage> BannerMobileImages { get; init; }
    public DbSet<Review> Reviews { get; init; }
    public DbSet<TelegramChat> TelegramChats { get; init; }
    public DbSet<ConsultationRequest> ConsultationRequests { get; init; }
    public DbSet<SupportGroupRequest> SupportGroupRequests { get; init; }
    public DbSet<VolunteerRequest> VolunteerRequests { get; init; }
    public DbSet<AdminUser> AdminUsers { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            
            if (entityType.FindProperty("Id") is not null)
            {
                modelBuilder.Entity(clrType).HasKey("Id");

                modelBuilder.Entity(clrType)
                    .Property("Id")
                    .ValueGeneratedNever()
                    .IsRequired()
                    .HasColumnName("id");
            }
            
            if (typeof(IAuditableEntity).IsAssignableFrom(clrType))
            {
                modelBuilder.Entity(clrType)
                    .Property(nameof(IAuditableEntity.CreatedAt))
                    .HasColumnName(nameof(IAuditableEntity.CreatedAt).ToSnakeCase());
                
                modelBuilder.Entity(clrType)
                    .Property(nameof(IAuditableEntity.UpdatedAt))
                    .HasColumnName(nameof(IAuditableEntity.UpdatedAt).ToSnakeCase());
            }
        }
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<IdOf<News>>().HaveConversion<IdOfValueConverter<News>>();
        configurationBuilder.Properties<IdOf<NewsImage>>().HaveConversion<IdOfValueConverter<NewsImage>>();
        configurationBuilder.Properties<IdOf<Event>>().HaveConversion<IdOfValueConverter<Event>>();
        configurationBuilder.Properties<IdOf<EventImage>>().HaveConversion<IdOfValueConverter<EventImage>>();
        configurationBuilder.Properties<IdOf<Banner>>().HaveConversion<IdOfValueConverter<Banner>>();
        configurationBuilder.Properties<IdOf<BannerImage>>().HaveConversion<IdOfValueConverter<BannerImage>>();
        configurationBuilder.Properties<IdOf<BannerMobileImage>>().HaveConversion<IdOfValueConverter<BannerMobileImage>>();
        configurationBuilder.Properties<IdOf<Review>>().HaveConversion<IdOfValueConverter<Review>>();
        configurationBuilder.Properties<IdOf<ConsultationRequest>>().HaveConversion<IdOfValueConverter<ConsultationRequest>>();
        configurationBuilder.Properties<IdOf<SupportGroupRequest>>().HaveConversion<IdOfValueConverter<SupportGroupRequest>>();
        configurationBuilder.Properties<IdOf<VolunteerRequest>>().HaveConversion<IdOfValueConverter<VolunteerRequest>>();
        configurationBuilder.Properties<IdOf<TelegramChat>>().HaveConversion<IdOfValueConverter<TelegramChat>>();
        configurationBuilder.Properties<IdOf<AdminUser>>().HaveConversion<IdOfValueConverter<AdminUser>>();
    }
}