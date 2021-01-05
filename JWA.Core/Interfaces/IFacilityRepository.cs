using JWA.Core.Entities;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IFacilityRepository : IRepository<Facility>
    {
        //Task<Facility> GetByEmail(string email);
    }
}
