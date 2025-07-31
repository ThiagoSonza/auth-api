using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Mappings;

public class RoleClaimMapping : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        builder.ToTable("RoleClaims");

        builder.HasKey(rc => rc.Id);

        builder.Property(rc => rc.Id)
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(rc => rc.RoleId)
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(rc => rc.ClaimType)
            .HasMaxLength(30)
            .IsRequired(false);

        builder.Property(rc => rc.ClaimValue)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.HasOne<IdentityRole>()
            .WithMany()
            .HasForeignKey(rc => rc.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}