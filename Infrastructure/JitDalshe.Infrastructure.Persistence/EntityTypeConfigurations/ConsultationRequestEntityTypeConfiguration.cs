using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class ConsultationRequestEntityTypeConfiguration : IEntityTypeConfiguration<ConsultationRequest>
{
    public void Configure(EntityTypeBuilder<ConsultationRequest> entity)
    {
        entity.ToTable(nameof(ConsultationRequest).ToSnakeCase());

        entity.HasId();

        entity.Property(x => x.PatientName)
            .IsRequired()
            .HasColumnName(nameof(ConsultationRequest.PatientName).ToSnakeCase());

        entity.Property(x => x.PatientAge)
            .IsRequired()
            .HasColumnName(nameof(ConsultationRequest.PatientAge).ToSnakeCase());

        entity.Property(x => x.PatientPhoneNumber)
            .HasColumnName(nameof(ConsultationRequest.PatientPhoneNumber).ToSnakeCase());

        entity.Property(x => x.PatientEmail)
            .HasColumnName(nameof(ConsultationRequest.PatientEmail).ToSnakeCase());

        entity.Property(x => x.ConsultationRequestStatus)
            .IsRequired()
            .HasColumnName(nameof(ConsultationRequest.ConsultationRequestStatus).ToSnakeCase());

        entity.Property(x => x.CommunicationMethods)
            .HasColumnName(nameof(ConsultationRequest.CommunicationMethods).ToSnakeCase());
        
        entity.Property(x => x.Comment)
            .HasColumnName(nameof(ConsultationRequest.Comment).ToSnakeCase());
        
        entity.HasAudits();
    }
}