using JWA.Core.Entities;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface ISystemStatusRepository : IRepository<SystemStatus>
    {
        //Task<Unit> GetByMacAddress(string macAddress);
    }
}
