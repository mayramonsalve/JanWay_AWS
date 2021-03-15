using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface ISystemStatusService
    {
        PagedList<SystemStatus> GetSystemStatus();//FlushQueryFilter filters);
        Task<SystemStatus> GetSystemStatus(int id);
        Task InsertSystemStatus(SystemStatus systemStatus);
        Task<bool> DeleteSystemStatus(int id);
    }
}