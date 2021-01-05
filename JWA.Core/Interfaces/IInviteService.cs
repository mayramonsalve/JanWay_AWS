using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IInviteService
    {
        PagedList<Invite> GetInvites(InviteQueryFilter filters);
        Task<Invite> GetInvite(int id);
        Task InsertInvite(Invite user, ClaimsPrincipal User);
        Task<bool> DeleteInvite(int id);
    }
}