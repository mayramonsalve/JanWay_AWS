using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IRoleService
    {
        PagedList<Role> GetRoles(RoleQueryFilter filters);
        Task<Role> GetRole(Guid id);
    }
}