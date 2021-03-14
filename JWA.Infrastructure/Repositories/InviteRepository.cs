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
    public class InviteRepository : BaseRepository<Invite>, IInviteRepository
    {
        private readonly JWAContext context;

        public InviteRepository(JWAContext context) : base(context)
        {
            this.context = context;
        }

        public Invite GetByEmail(string email)
        {
            return _entities.Where(x => x.Email == email).FirstOrDefault();
        }
        public Invite GetByEmailId(string email)
        {
            return _entities.Where(e => e.Email.ToLower() == email.ToLower()).FirstOrDefault();
        }

        public Invite GetByInviteId(int id)
        {
            return _entities.Where(e => e.Id == id).FirstOrDefault();
        }
        public bool RemoveInvite(string email)
        {
            try
            {
                var invite = GetByEmailId(email);
                var result = context.Remove(invite);
                if (result.State.ToString() == "Deleted")
                {
                    SaveChanges();
                    return true;
                }
                return false;
            }
            catch (System.Exception)
            {
                return false;
            }
            
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
