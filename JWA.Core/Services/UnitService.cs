using JWA.Core.CustomEntities;
using JWA.Core.DTOs;
using JWA.Core.Entities;
using JWA.Core.Exceptions;
using JWA.Core.Interfaces;
using JWA.Core.QueryFilters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public UnitService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task InsertUnit(Unit unit)
        {
            unit.Suin = 0;
            unit.Uin = 0;
            unit.IpAddress = "127.0.0.1";
            unit.Name = "";
            unit.IsActive = false;
            unit.CreationDate = DateTime.Now;
            await _unitOfWork.UnitRepository.Insert(unit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task InsertUnitsRange(List<Unit> units)
        {
            units.ForEach(e => e.Uin = e.Suin);
            units.ForEach(e => e.MacAddress = e.Name.Replace(" ",""));
            units.ForEach(e => e.IpAddress = DateTime.Now.Ticks.ToString());
            units.ForEach(e => e.IsActive = true);
            units.ForEach(e => e.CreationDate = DateTime.Now);
            await _unitOfWork.UnitRepository.InsertRange(units);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteUnit(int id)
        {
            await _unitOfWork.UnitRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Unit> GetUnit(int id)
        {
            return await _unitOfWork.UnitRepository.GetById(id);
        }

        public async Task<UnitsProfileDto> GetUnitProfile(int id)
        {
            var unit = await _unitOfWork.UnitRepository.GetById(id);
            var flushes = await _unitOfWork.FlushRepository.GetByUnit(id);
            var lastFlush = flushes.Take(1);
            var flushesHistory = flushes.Skip(1);

            UnitsProfileDto unitsProfileDto = new UnitsProfileDto();
            unitsProfileDto.Id = unit.Id;
            unitsProfileDto.Name = unit.Name;
            unitsProfileDto.IpAddress = unit.IpAddress;
            unitsProfileDto.PressureOnFilters = "Good"; //#MISSING
            unitsProfileDto.BatteryLevel = 100;
            unitsProfileDto.FlushesNumber = flushes.Count();

            unitsProfileDto.LastFlush = null;

            unitsProfileDto.FlushesHistory = null;

            return unitsProfileDto;
        }

        public PagedList<Unit> GetUnits(UnitQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            var units = _unitOfWork.UnitRepository.GetAll().ToList();

            if (!String.IsNullOrEmpty(filters.Name))
            {
                units = units.Where(x => x.Name.ToLower().Contains(filters.Name.ToLower())).ToList();
            }

            if (filters.OrganizationId.HasValue)
            {
                units = units.Where(x => x.Facility.OrganizationId == filters.OrganizationId.Value).ToList();
            }

            if (filters.FacilityId.HasValue)
            {
                units = units.Where(x => x.FacilityId == filters.FacilityId).ToList();
            }

            if (filters.SuinNumber.HasValue)
            {
                units = units.Where(x => x.Suin == filters.SuinNumber.Value).ToList();
            }

            if (filters.Health.HasValue)
            {
                units = units.Where(x => x.SystemStatus.OrderByDescending(y => y.Date).FirstOrDefault().Health == filters.Health.Value).ToList();
            }

            var pagedUnits = PagedList<Unit>.Create(units, filters.PageNumber, filters.PageSize);

            return pagedUnits;
        }

        public async Task<Unit> GetByMacAddress(string macAddress)
        {
            return await _unitOfWork.UnitRepository.GetByMacAddress(macAddress);
        }

        public async Task<bool> AssignUnit(Unit unit)
        {
            var existingUnit = await _unitOfWork.UnitRepository.GetBySuinNumber(unit.Suin);

            if (existingUnit == null)
                throw new BusinessException("Suin Number doesn't exist.");
            
            existingUnit.Name = unit.Name;

            existingUnit.FacilityId = unit.FacilityId;

            existingUnit.IsActive = true;

            _unitOfWork.UnitRepository.Update(existingUnit);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUnit(Unit unit)
        {
            var existingUnit = await _unitOfWork.UnitRepository.GetBySuinNumber(unit.Suin);

            if (!String.IsNullOrEmpty(unit.Name))
                existingUnit.Name = unit.Name;

            if (unit.FacilityId.HasValue)
                existingUnit.FacilityId = unit.FacilityId;

            _unitOfWork.UnitRepository.Update(existingUnit);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DetachUnit(int id)
        {
            var existingUnit = await _unitOfWork.UnitRepository.GetById(id);

            existingUnit.IsActive = false;
            existingUnit.FacilityId = null;

            _unitOfWork.UnitRepository.Update(existingUnit);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RebootUnit(int id)
        {
            var existingUnit = await _unitOfWork.UnitRepository.GetById(id);

            existingUnit.Name = "";
            existingUnit.IpAddress = "";
            existingUnit.Suin = 0;
            existingUnit.FacilityId = null;
            existingUnit.IsActive = false;

            _unitOfWork.UnitRepository.Update(existingUnit);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PagedList<Unit>> GetUnassignedUnits(BaseQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;
            
            var unassignedUnits = await _unitOfWork.UnitRepository.GetUnassignedUnits();

            var pagedUnassignedUnits = PagedList<Unit>.Create(unassignedUnits, filters.PageNumber, filters.PageSize);

            return pagedUnassignedUnits;
        }
        
        public async Task<PagedList<Flush>> GetFlushesHistory(FlushQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            var flushesHistory = await _unitOfWork.FlushRepository.GetByUnit(filters.UnitId);

            if (!String.IsNullOrEmpty(filters.PressureOnFilters))
            { //#MISSING: UPDATE PRESSURE ON FILTERS FILTER
                flushesHistory = flushesHistory.Where(x => x.Performance > 0).ToList();
            }

            if (filters.Health.HasValue)
            {
                flushesHistory = flushesHistory.Where(x => x.Health == filters.Health.Value).ToList();
            }

            if (filters.Health.HasValue)
            {//#MISSING: UPDATE SELENOID TEMPERATURE FILTER
                flushesHistory = flushesHistory.Where(x => x.Performance > 0).ToList();
            }

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            if (filters.Date > dtDateTime)
            {//#MISSING: UPDATE SELENOID TEMPERATURE FILTER
                flushesHistory = flushesHistory.Where(x => x.Date == filters.Date).ToList();
            }

            var pagedFlushesHistory = PagedList<Flush>.Create(flushesHistory, filters.PageNumber, filters.PageSize);

            return pagedFlushesHistory;
        }
    }
}
