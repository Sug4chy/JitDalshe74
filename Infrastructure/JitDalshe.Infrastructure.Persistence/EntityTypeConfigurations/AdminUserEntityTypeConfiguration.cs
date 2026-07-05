using JitDalshe.Domain.Entities.Users;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public class AdminUserEntityTypeConfiguration : IEntityTypeConfiguration<AdminUser>
{
    public void Configure(EntityTypeBuilder<AdminUser> entity)
    {
        entity.ToTable(nameof(AdminUser).ToSnakeCase());
        
        entity.Property(x => x.Email)
            .IsRequired()
            .HasColumnName(nameof(AdminUser.Email).ToSnakeCase());
        
        entity.HasIndex(x => x.Email)
            .IsUnique();

        entity.Property(x => x.PasswordHash)
            .IsRequired()
            .HasColumnName(nameof(AdminUser.PasswordHash).ToSnakeCase());

        entity.Property(x => x.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnName(nameof(AdminUser.Role).ToSnakeCase());

        entity.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnName(nameof(AdminUser.IsActive).ToSnakeCase());
    }
}