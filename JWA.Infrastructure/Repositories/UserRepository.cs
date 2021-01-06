using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.Interfaces;
using JWA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace JWA.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly JWAContext _context;
        protected DbSet<User> _entities;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        public UserRepository(JWAContext context, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            _context = context;
            userManager = userManager;
            _entities = _context.Set<User>();
        }

        public async Task<User> GetUserByCredentials(SignIn signIn)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Email == signIn.Email);
        }
        public IEnumerable<User> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public async Task<User> GetById(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Insert(User entity)
        {
            var user = new IdentityUser { UserName = entity.UserName, Email = entity.Email };
            entity.ConcurrencyStamp = user.ConcurrencyStamp;
            entity.SecurityStamp = user.SecurityStamp;
            await _entities.AddAsync(entity);
        }

        public void Update(User entity)
        {
            _entities.Update(entity);
        }

        public async Task Delete(Guid id)
        {
            User entity = await GetById(id);
            _entities.Remove(entity);
        }
        public User GetUserByEmail(string email)
        {
            return _entities.Where(e => e.Email == email).FirstOrDefault();
        }
        public User GetUserByUserName(string username)
        {
            return _entities.Where(e => e.UserName == username).FirstOrDefault();
        }
    }
}
