using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Mappings;

public class UserTokenMapping : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.ToTable("UserTokens");

        builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

        builder.Property(ut => ut.UserId)
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(ut => ut.LoginProvider)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ut => ut.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ut => ut.Value)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
