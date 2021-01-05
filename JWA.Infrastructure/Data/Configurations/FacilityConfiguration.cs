using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.ToTable("facilities");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.AddressId).HasColumnName("address_id");

            builder.Property(e => e.CreationDate)
                .HasColumnName("creation_date")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.IsActive).HasColumnName("is_active");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(50);

            builder.Property(e => e.OrganizationId).HasColumnName("organization_id");

            builder.HasOne(d => d.Address)
                .WithMany(p => p.Facilities)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("facilities_address_id_fkey");

            builder.HasOne(d => d.Organization)
                .WithMany(p => p.Facilities)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("facilities_organization_id_fkey");
        }
    }
}
