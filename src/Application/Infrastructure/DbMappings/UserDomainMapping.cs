using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Mappings;

public class UserMapping : IEntityTypeConfiguration<UserDomain>
{
    public void Configure(EntityTypeBuilder<UserDomain> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(u => u.PersonalIdentifier)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.UserName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.NormalizedEmail)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.EmailConfirmed)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(u => u.SecurityStamp)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(u => u.ConcurrencyStamp)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(30)
            .IsRequired(false);

        builder.Property(u => u.PhoneNumberConfirmed)
            .HasDefaultValue(false);

        builder.Property(u => u.TwoFactorEnabled)
            .HasDefaultValue(false);
    }
}