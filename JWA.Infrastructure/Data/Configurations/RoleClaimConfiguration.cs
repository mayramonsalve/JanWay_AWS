using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("roleclaims");

            builder.HasIndex(e => e.RoleId)
                .HasName("role_claims_role_id_key");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.ClaimType).HasColumnName("claim_type");

            builder.Property(e => e.ClaimValue).HasColumnName("claim_value");

            builder.Property(e => e.RoleId).HasColumnName("role_id");

            builder.HasOne(d => d.Role)
                .WithMany(p => p.RoleClaims)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("role_claims_role_id_fkey");
        }
    }
}
