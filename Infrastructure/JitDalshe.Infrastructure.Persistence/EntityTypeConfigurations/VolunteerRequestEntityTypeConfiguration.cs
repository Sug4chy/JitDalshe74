using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public class VolunteerRequestEntityTypeConfiguration : IEntityTypeConfiguration<VolunteerRequest>
{
    public void Configure(EntityTypeBuilder<VolunteerRequest> entity)
    {
        entity.ToTable(nameof(VolunteerRequest).ToSnakeCase());

        entity.HasId();

        entity.Property(x => x.ApplicantName)
            .IsRequired()
            .HasColumnName(nameof(VolunteerRequest.ApplicantName).ToSnakeCase());

        entity.Property(x => x.ApplicantAge)
            .IsRequired()
            .HasColumnName(nameof(VolunteerRequest.ApplicantAge).ToSnakeCase());

        entity.Property(x => x.ApplicantPhoneNumber)
            .HasColumnName(nameof(VolunteerRequest.ApplicantPhoneNumber).ToSnakeCase());

        entity.Property(x => x.ApplicantEmail)
            .HasColumnName(nameof(VolunteerRequest.ApplicantEmail).ToSnakeCase());

        entity.Property(x => x.Status)
            .IsRequired()
            .HasColumnName(nameof(VolunteerRequest.Status).ToSnakeCase());

        entity.Property(x => x.CommunicationMethods)
            .HasColumnName(nameof(VolunteerRequest.CommunicationMethods).ToSnakeCase());
        
        entity.Property(x => x.Comment)
            .HasColumnName(nameof(VolunteerRequest.Comment).ToSnakeCase());
        
        entity.HasAudits();
    }
}