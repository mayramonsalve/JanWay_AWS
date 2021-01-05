using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class FlushConfiguration : IEntityTypeConfiguration<Flush>
    {
        public void Configure(EntityTypeBuilder<Flush> builder)
        {
            builder.ToTable("flushes");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.BatteryLevel).HasColumnName("battery_level");

            builder.Property(e => e.CreationDate)
                .HasColumnName("creation_date")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.Date)
                .HasColumnName("date")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.Filter1).HasColumnName("filter_1");

            builder.Property(e => e.Filter2).HasColumnName("filter_2");

            builder.Property(e => e.Filter3).HasColumnName("filter_3");

            builder.Property(e => e.Filter4).HasColumnName("filter_4");

            builder.Property(e => e.Health).HasColumnName("health");

            builder.Property(e => e.Performance).HasColumnName("performance");

            builder.Property(e => e.SelenoidTemperature).HasColumnName("selenoid_temperature");

            builder.Property(e => e.UnitId).HasColumnName("unit_id");

            builder.HasOne(d => d.Unit)
                .WithMany(p => p.Flushes)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("flushes_unit_id_fkey");
        }
    }
}
