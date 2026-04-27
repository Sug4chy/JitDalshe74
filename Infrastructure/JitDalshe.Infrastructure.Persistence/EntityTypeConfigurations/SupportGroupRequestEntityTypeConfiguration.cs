using JitDalshe.Domain.Entities.SupportGroups;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public class SupportGroupRequestEntityTypeConfiguration : IEntityTypeConfiguration<SupportGroupRequest>
{
    public void Configure(EntityTypeBuilder<SupportGroupRequest> entity)
    {
        entity.ToTable(nameof(SupportGroupRequest).ToSnakeCase());

        entity.HasId();

        entity.Property(x => x.ApplicantName)
            .IsRequired()
            .HasColumnName(nameof(SupportGroupRequest.ApplicantName).ToSnakeCase());

        entity.Property(x => x.ApplicantAge)
            .IsRequired()
            .HasColumnName(nameof(SupportGroupRequest.ApplicantAge).ToSnakeCase());

        entity.Property(x => x.ApplicantPhoneNumber)
            .HasColumnName(nameof(SupportGroupRequest.ApplicantPhoneNumber).ToSnakeCase());

        entity.Property(x => x.ApplicantEmail)
            .HasColumnName(nameof(SupportGroupRequest.ApplicantEmail).ToSnakeCase());

        entity.Property(x => x.Status)
            .IsRequired()
            .HasColumnName(nameof(SupportGroupRequest.Status).ToSnakeCase());

        entity.Property(x => x.CommunicationMethods)
            .HasColumnName(nameof(SupportGroupRequest.CommunicationMethods).ToSnakeCase());
        
        entity.Property(x => x.Comment)
            .HasColumnName(nameof(SupportGroupRequest.Comment).ToSnakeCase());
        
        entity.HasAudits();
    }
}