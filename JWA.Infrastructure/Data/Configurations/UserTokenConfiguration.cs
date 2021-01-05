using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                .HasName("user_tokens_pkey");

            builder.ToTable("usertokens");

            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.Property(e => e.LoginProvider).HasColumnName("login_provider");

            builder.Property(e => e.Name).HasColumnName("name");

            builder.Property(e => e.Value).HasColumnName("value");

            builder.HasOne(d => d.User)
                .WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_tokens_user_id_fkey");
        }
    }
}
