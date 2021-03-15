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
    public class UnitRepository : BaseRepository<Unit>, IUnitRepository
    {
        public UnitRepository(JWAContext context) : base(context)
        { }

        public async Task<Unit> GetByMacAddress(string macAddress)
        {
            return await _entities.FirstOrDefaultAsync(e => e.MacAddress.ToLower() == macAddress.ToLower());
        }
        public async Task<Unit> GetBySuinNumber(int suin)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Suin == suin);
        }
        public async Task<IEnumerable<Unit>> GetUnassignedUnits()
        {
            return await _entities.Where(e => !e.FacilityId.HasValue).ToListAsync();
        }
    }
}
