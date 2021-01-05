using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IFacilityService
    {
        PagedList<Facility> GetFacilities();// FacilityQueryFilter filters);
        Task<Facility> GetFacility(int id);
        Task InsertFacility(Facility facility);
        Task<bool> DeleteFacility(int id);
    }
}