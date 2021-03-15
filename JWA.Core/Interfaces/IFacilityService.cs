using JWA.Core.CustomEntities;
using JWA.Core.DTOs;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IFacilityService
    {
        Task<PagedList<Facility>> GetFacilities(FacilityQueryFilter filters);
        Task<Facility> GetFacility(int id);
        Task<FacilityProfileDto> GetFacilityProfile(int id);
        Task AddFacility(Address address, Facility facility, List<Invite> invites, List<Unit> units);
        Task<int> InsertFacility(Facility facility);
        Task<Dictionary<string,int>> InsertFacilitiesRange(List<Facility> facilities);
        Task<bool> UpdateFacility(Facility facility, Address address);
        Task<bool> DeleteFacility(int id);
        Task<bool> ExistsFacility(string name, int organizationId);
        Task<bool> ActivateFacility(int id);
        Task<bool> DeactivateFacility(int id);
    }
}