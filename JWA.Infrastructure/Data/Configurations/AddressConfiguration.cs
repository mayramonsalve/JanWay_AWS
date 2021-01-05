using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("addresses");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('adresses_id_seq'::regclass)");

            builder.Property(e => e.City)
                .IsRequired()
                .HasColumnName("city")
                .HasMaxLength(50);

            builder.Property(e => e.CreationDate)
                .HasColumnName("creation_date")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.Latitude).HasColumnName("latitude");

            builder.Property(e => e.Longitude).HasColumnName("longitude");

            builder.Property(e => e.StateId).HasColumnName("state_id");

            builder.Property(e => e.Street)
                .IsRequired()
                .HasColumnName("street")
                .HasMaxLength(100);

            builder.Property(e => e.ZipCode)
                .IsRequired()
                .HasColumnName("zip_code")
                .HasMaxLength(10);

            builder.HasOne(d => d.State)
                .WithMany(p => p.Addresses)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("addresses_states_id_fkey");
        }
    }
}
