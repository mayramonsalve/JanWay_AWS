using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IOrganizationService
    {
        PagedList<Organization> GetOrganizations();//OrganizationQueryFilter filters);
        Task<Organization> GetOrganization(int id);
        Task InsertOrganization(Organization organization);
        Task<bool> DeleteOrganization(int id);
    }
}