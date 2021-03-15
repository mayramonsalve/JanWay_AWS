using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IFlushService
    {
        PagedList<Flush> GetFlushes();//FlushQueryFilter filters);
        Task<Flush> GetFlush(int id);
        Task InsertFlush(Flush flush);
        Task<bool> DeleteFlush(int id);
    }
}