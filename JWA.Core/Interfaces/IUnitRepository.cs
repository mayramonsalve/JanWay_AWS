using JWA.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IUnitRepository : IRepository<Unit>
    {
        Task<Unit> GetByMacAddress(string macAddress);
        Task<Unit> GetBySuinNumber(int suin);
        Task<IEnumerable<Unit>> GetUnassignedUnits();
    }
}
