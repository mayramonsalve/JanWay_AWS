using JWA.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IFacilityRepository : IRepository<Facility>
    {
        Task<Facility> GetFacilityByName(string name, int organizationId);
        Task<IEnumerable<Facility>> GetFacilitiesByOrganization(int organizationId);
        Task Activate(int id);
        Task Deactivate(int id);
    }
}
