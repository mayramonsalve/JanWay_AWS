using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWA.Infrastructure.Data.Configurations
{
    public class InviteConfiguration : IEntityTypeConfiguration<Invite>
    {
        public void Configure(EntityTypeBuilder<Invite> builder)
        {
            builder.ToTable("invites");

            builder.HasIndex(e => e.Email)
                .HasName("invites_email_key")
                .IsUnique();

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.CreationDate).HasColumnName("creation_date");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(100);

            builder.Property(e => e.FacilityId).HasColumnName("facility_id");

            builder.Property(e => e.OrganizationId).HasColumnName("organization_id");

            builder.Property(e => e.RoleId).HasColumnName("role_id");

            builder.HasOne(d => d.Facility)
                .WithMany(p => p.Invites)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("invites_facility_id_fkey");

            builder.HasOne(d => d.Organization)
                .WithMany(p => p.Invites)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("invites_organization_id_fkey");

            /*builder.HasOne(d => d.Role)
                .WithMany(p => p.Invites)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invites_role_id_fkey");*/
        }
    }
}
