using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("userclaims");

            builder.HasIndex(e => e.UserId)
                .HasName("user_claims_user_id_key");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.ClaimType).HasColumnName("claim_type");

            builder.Property(e => e.ClaimValue).HasColumnName("claim_value");

            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.UserClaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_claims_user_id_fkey");
        }
    }
}
