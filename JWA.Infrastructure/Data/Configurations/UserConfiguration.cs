using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasIndex(e => e.NormalizedEmail)
                .HasName("users_normalized_email_key");

            builder.HasIndex(e => e.NormalizedUserName)
                .HasName("users_normalized_user_name_key")
                .IsUnique();

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(e => e.AccessFailedCount).HasColumnName("access_failed_count");

            builder.Property(e => e.ConcurrencyStamp).HasColumnName("concurrency_stamp");

            builder.Property(e => e.CreationDate)
                .HasColumnName("creation_date")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(256);

            builder.Property(e => e.EmailConfirmed).HasColumnName("email_confirmed");

            builder.Property(e => e.LockoutEnabled).HasColumnName("lockout_enabled");

            builder.Property(e => e.LockoutEnd)
                .HasColumnName("lockout_end")
                .HasColumnType("timestamp with time zone");

            builder.Property(e => e.NormalizedEmail)
                .HasColumnName("normalized_email")
                .HasMaxLength(256);

            builder.Property(e => e.NormalizedUserName)
                .HasColumnName("normalized_user_name")
                .HasMaxLength(256);

            builder.Property(e => e.PasswordHash).HasColumnName("password_hash");

            builder.Property(e => e.PhoneNumber).HasColumnName("phone_number");

            builder.Property(e => e.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");

            builder.Property(e => e.SecurityStamp).HasColumnName("security_stamp");

            builder.Property(e => e.TwoFactorEnabled).HasColumnName("two_factor_enabled");

            builder.Property(e => e.UserName)
                .HasColumnName("user_name")
                .HasMaxLength(256);
        }
    }
}
