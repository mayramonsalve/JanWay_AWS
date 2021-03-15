using JWA.Core.CustomEntities;
using JWA.Core.DTOs;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IUnitService
    {
        PagedList<Unit> GetUnits(UnitQueryFilter filters);
        Task<Unit> GetUnit(int id);
        Task<UnitsProfileDto> GetUnitProfile(int id);
        Task InsertUnit(Unit unit);
        Task InsertUnitsRange(List<Unit> units);
        Task<bool> DeleteUnit(int id);
        Task<Unit> GetByMacAddress(string macAddress);
        Task<bool> AssignUnit(Unit unit);
        Task<bool> UpdateUnit(Unit unit);
        Task<bool> DetachUnit(int id);
        Task<bool> RebootUnit(int id);
        Task<PagedList<Unit>> GetUnassignedUnits(BaseQueryFilter filters);
        Task<PagedList<Flush>> GetFlushesHistory(FlushQueryFilter filters);
    }
}