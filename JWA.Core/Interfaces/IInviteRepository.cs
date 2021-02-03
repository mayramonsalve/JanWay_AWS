using JWA.Core.Entities;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IInviteRepository : IRepository<Invite>
    {
        Invite GetByEmail(string email);
        Invite GetByEmailId(string email);
        Invite GetByInviteId(int id);
        void SaveChanges();
        bool RemoveInvite(string email);
    }
}
