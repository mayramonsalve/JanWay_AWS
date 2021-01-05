using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IUnitService
    {
        PagedList<Unit> GetUnits();//UnitQueryFilter filters);
        Task<Unit> GetUnit(int id);
        Task InsertUnit(Unit unit);
        Task<bool> DeleteUnit(int id);
    }
}