using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.Interfaces;
using JWA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.Infrastructure.Repositories
{
    public class FacilityRepository : BaseRepository<Facility>, IFacilityRepository
    {
        public FacilityRepository(JWAContext context) : base(context)
        { }

        public async Task<Facility> GetFacilityByName(string name, int organizationId)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Name.ToLower() == name.ToLower() && e.OrganizationId == organizationId);
        }

        public async Task<IEnumerable<Facility>> GetFacilitiesByOrganization(int organizationId)
        {
            return await _entities.Where(e => e.OrganizationId == organizationId).ToListAsync();
        }

        public async Task Activate(int id)
        {
            Facility entity = await GetById(id);
            entity.IsActive = true;
            _entities.Update(entity);
        }

        public async Task Deactivate(int id)
        {
            Facility entity = await GetById(id);
            entity.IsActive = false;
            _entities.Update(entity);
        }
    }
}
