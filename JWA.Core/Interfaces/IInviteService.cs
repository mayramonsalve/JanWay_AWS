using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IInviteService
    {
        PagedList<Invite> GetInvites(InviteQueryFilter filters);
        Invite GetInvite(int id);
        Task InsertInvite(Invite user, ClaimsPrincipal User);
        Task InsertInvitesRange(List<Invite> invites);
        Task<bool> DeleteInvite(int id);
        Task<bool> RemoveInvite(int id);
        Invite GetInviteByEmailId(string email);
        Task<Invite> InsertInvite(Invite invite);
    }
}