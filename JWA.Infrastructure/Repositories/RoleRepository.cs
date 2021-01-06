using JWA.Core.CustomEntities;
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
    public class RoleRepository : IRoleRepository
    {
        private readonly JWAContext _context;
        protected DbSet<Role> _entities; 

        public RoleRepository(JWAContext context)
        {
            _context = context;
            _entities = _context.Set<Role>();
        }

        public IEnumerable<Role> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public async Task<Role> GetById(Guid id)
        {
            return await _entities.FindAsync(id);
        }
    }
}
