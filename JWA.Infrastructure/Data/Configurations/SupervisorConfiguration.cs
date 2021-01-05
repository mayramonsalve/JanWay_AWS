using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class SupervisorConfiguration : IEntityTypeConfiguration<Supervisor>
    {
        public void Configure(EntityTypeBuilder<Supervisor> builder)
        {
            builder.ToTable("supervisors");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.CreationDate)
                .HasColumnName("creation_date")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.FacilityId).HasColumnName("facility_id");

            builder.Property(e => e.IsOwner).HasColumnName("is_owner");

            builder.Property(e => e.OrganizationId).HasColumnName("organization_id");

            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Supervisors)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("supervisors_user_id_fkey");

            builder.HasOne(d => d.Facility)
                .WithMany(p => p.Supervisors)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("supervisors_facility_id_fkey");

            builder.HasOne(d => d.Organization)
                .WithMany(p => p.Supervisors)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("supervisors_organization_id_fkey");
        }
    }
}
