using JWA.Core.CustomEntities;
using JWA.Core.DTOs;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IOrganizationService
    {
        PagedList<Organization> GetOrganizations(OrganizationQueryFilter filters);
        Task<Organization> GetOrganization(int id);
        Task<OrganizationProfileDto> GetOrganizationProfile(int id);
        Task SetupOrganization(Address address, Organization organization, List<Facility> facilities, List<Invite> invites, List<Unit> units);
        Task<int> InsertOrganization(Organization organization);
        Task<bool> UpdateOrganization(Organization organization, Address address);
        Task<bool> DeleteOrganization(int id);
        Task<bool> ExistsOrganization(string name);
        Task<bool> ActivateOrganization(int id);
        Task<bool> DeactivateOrganization(int id);
    }
}