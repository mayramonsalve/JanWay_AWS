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
        private readonly JWAContext context;

        public FacilityRepository(JWAContext context) : base(context)
        {
            this.context = context;
        }

        public Facility GetFacilityById(int id)
        {
            return context.Facilities.Where(x => x.Id == id).FirstOrDefault();
        }

        //public async Task<Facility> GetByEmail(string email)
        //{
        //    return await _entities.FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());
        //}

    }
}
