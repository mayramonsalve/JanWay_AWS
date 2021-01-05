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
    public class OrganizationRepository : BaseRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(JWAContext context) : base(context)
        { }

        //public async Task<Organization> GetByEmail(string email)
        //{
        //    return await _entities.FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());
        //}

    }
}
