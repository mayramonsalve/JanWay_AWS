using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");

            builder.HasIndex(e => e.NormalizedName)
                .HasName("roles_normalized_name_key")
                .IsUnique();

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(e => e.ConcurrencyStamp).HasColumnName("concurrency_stamp");

            builder.Property(e => e.IsInternal).HasColumnName("is_internal");

            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(256);

            builder.Property(e => e.NormalizedName)
                .HasColumnName("normalized_name")
                .HasMaxLength(256);
        }
    }
}
