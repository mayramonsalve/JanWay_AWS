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
    public class FlushRepository : BaseRepository<Flush>, IFlushRepository
    {
        public FlushRepository(JWAContext context) : base(context)
        { }

        public async Task<IEnumerable<Flush>> GetByUnit(int unitId)
        {
            return await _entities.Where(e => e.UnitId == unitId).OrderByDescending(e => e.Date).ToListAsync();
        }

    }
}
