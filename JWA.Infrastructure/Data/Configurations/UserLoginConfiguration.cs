using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                .HasName("user_logins_pkey");

            builder.ToTable("userlogins");

            builder.HasIndex(e => e.UserId)
                .HasName("user_logins_user_id_key");

            builder.Property(e => e.LoginProvider).HasColumnName("login_provider");

            builder.Property(e => e.ProviderKey).HasColumnName("provider_key");

            builder.Property(e => e.ProviderDisplayName).HasColumnName("provider_display_name");

            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_logins_user_id_fkey");
        }
    }
}
