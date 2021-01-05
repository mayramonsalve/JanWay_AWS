using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("units");

            builder.HasIndex(e => e.IpAddress)
                .HasName("units_ip_address_key")
                .IsUnique();

            builder.HasIndex(e => e.MacAddress)
                .HasName("units_mac_address_key")
                .IsUnique();

            builder.HasIndex(e => e.Suin)
                .HasName("units_suin_key")
                .IsUnique();

            builder.HasIndex(e => e.Uin)
                .HasName("units_uin_key")
                .IsUnique();

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.CreationDate)
                .HasColumnName("creation_date")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.FacilityId).HasColumnName("facility_id");

            builder.Property(e => e.IpAddress)
                .IsRequired()
                .HasColumnName("ip_address")
                .HasMaxLength(20);

            builder.Property(e => e.IsActive).HasColumnName("is_active");

            builder.Property(e => e.MacAddress)
                .IsRequired()
                .HasColumnName("mac_address")
                .HasMaxLength(20);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(50);

            builder.Property(e => e.Suin).HasColumnName("suin");

            builder.Property(e => e.Uin).HasColumnName("uin");

            builder.HasOne(d => d.Facility)
                .WithMany(p => p.Units)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("units_facility_id_fkey");
        }
    }
}
