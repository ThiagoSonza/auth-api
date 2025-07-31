using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Mappings;

public class RoleMapping : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(r => r.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.NormalizedName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.ConcurrencyStamp)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.HasMany<IdentityRoleClaim<string>>()
            .WithOne()
            .HasForeignKey(rc => rc.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
