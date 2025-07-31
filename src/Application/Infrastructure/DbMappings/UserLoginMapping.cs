using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Mappings;

public class UserLoginMapping : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable("UserLogins");

        builder.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });

        builder.Property(ul => ul.LoginProvider)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ul => ul.ProviderKey)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ul => ul.ProviderDisplayName)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(ul => ul.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
