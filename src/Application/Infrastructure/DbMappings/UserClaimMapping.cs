using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Mappings;

public class UserClaimMapping : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        builder.ToTable("UserClaims");

        builder.HasKey(uc => uc.Id);

        builder.Property(uc => uc.UserId)
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(uc => uc.ClaimType)
            .HasMaxLength(30)
            .IsRequired(false);

        builder.Property(uc => uc.ClaimValue)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.HasOne<UserDomain>()
            .WithMany()
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}