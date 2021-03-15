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
    public class SystemStatusRepository : BaseRepository<SystemStatus>, ISystemStatusRepository
    {
        public SystemStatusRepository(JWAContext context) : base(context)
        { }

        //public async Task<Unit> GetByMacAddress(string macAddress)
        //{
        //    return await _entities.FirstOrDefaultAsync(e => e.MacAddress.ToLower() == macAddress.ToLower());
        //}

    }
}
