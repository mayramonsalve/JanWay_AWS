using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class SystemStatusConfiguration : IEntityTypeConfiguration<SystemStatus>
    {
        public void Configure(EntityTypeBuilder<SystemStatus> builder)
        {
            builder.ToTable("systemstatus");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.BatteryLevel).HasColumnName("battery_level");

            builder.Property(e => e.CreationDate)
                .HasColumnName("creation_date")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.Date)
                .HasColumnName("date")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.Health).HasColumnName("health");

            builder.Property(e => e.Performance).HasColumnName("performance");

            builder.Property(e => e.SelenoidTemperature).HasColumnName("selenoid_temperature");

            builder.Property(e => e.UnitId).HasColumnName("unit_id");

            builder.HasOne(d => d.Unit)
                .WithMany(p => p.SystemStatus)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("systemstatus_unit_id_fkey");

        }
    }
}
