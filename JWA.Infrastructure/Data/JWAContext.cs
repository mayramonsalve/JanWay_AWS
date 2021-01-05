using JWA.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JWA.Infrastructure.Data
{
    public partial class JWAContext : DbContext
    {
        public JWAContext()
        {
        }

        public JWAContext(DbContextOptions<JWAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<Flush> Flushes { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<RoleClaim> RoleClaims { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Supervisor> Supervisors { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //modelBuilder.ApplyConfiguration(new AddressConfiguration());

            //modelBuilder.ApplyConfiguration(new FacilityConfiguration());

            //modelBuilder.ApplyConfiguration(new FlushConfiguration());

            //modelBuilder.ApplyConfiguration(new InviteConfiguration());

            //modelBuilder.ApplyConfiguration(new OrganizationConfiguration());

            //modelBuilder.ApplyConfiguration(new RoleConfiguration());

            //modelBuilder.ApplyConfiguration(new StateConfiguration());

            //modelBuilder.ApplyConfiguration(new SupervisorConfiguration());

            //modelBuilder.ApplyConfiguration(new UnitConfiguration());

            //modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
