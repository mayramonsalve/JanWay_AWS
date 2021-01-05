using JWA.Core.Entities;
using JWA.Core.Interfaces;
using JWA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.Infrastructure.Repositories
{
    public class SupervisorRepository : BaseRepository<Supervisor>, ISupervisorRepository
    {
        public SupervisorRepository(JWAContext context) : base(context)
        { }

        public async Task<Supervisor> GetSupervisorByUser(Guid userId)
        {
            return await _entities.FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task<IEnumerable<Supervisor>> GetSupervisorsByUsers(IEnumerable<Guid> usersId)
        {
            return await _entities.Where(e => usersId.Contains(e.UserId)).ToListAsync();
        }

        public async Task<IEnumerable<Supervisor>> GetSupervisorsByOrganization(int organizationId)
        {
            return await _entities.Where(e => e.OrganizationId == organizationId).ToListAsync();
        }
    }
}
