using JWA.Core.Entities;
using JWA.Core.Interfaces;
using JWA.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWA.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JWAContext _context;

        //This is for all repositories
        //private readonly IRepository<Role> _roleRepository; //This is used when repository is not updated, it's the same as BaseRepository

        private readonly IFacilityRepository _facilityRepository;

        private readonly IInviteRepository _inviteRepository;

        private readonly IOrganizationRepository _organizationRepository;

        private readonly IRoleRepository _roleRepository;

        private readonly ISupervisorRepository _supervisorRepository;

        private readonly IUnitRepository _unitRepository;

        private readonly IUserRepository _userRepository;
        public UnitOfWork(JWAContext context)
        {
            _context = context;
        }

        //Instances of all the repositories
        public IFacilityRepository FacilityRepository => _facilityRepository ?? new FacilityRepository(_context);
        public IInviteRepository InviteRepository => _inviteRepository ?? new InviteRepository(_context);
        public IOrganizationRepository OrganizationRepository => _organizationRepository ?? new OrganizationRepository(_context);
        public IRoleRepository RoleRepository => _roleRepository ?? new RoleRepository(_context);
        public ISupervisorRepository SupervisorRepository => _supervisorRepository ?? new SupervisorRepository(_context);
        public IUnitRepository UnitRepository => _unitRepository ?? new UnitRepository(_context);
        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
