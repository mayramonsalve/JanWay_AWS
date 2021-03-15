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
    public class FacilityService : IFacilityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public FacilityService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task AddFacility(Address address, Facility facility, List<Invite> invites, List<Unit> units)
        {
            await _unitOfWork.AddressRepository.Insert(address);
            await _unitOfWork.SaveChangesAsync();

            //throw new BusinessException("Organization sent doesn't match the invite sent.");
            facility.AddressId = address.Id;
            await _unitOfWork.FacilityRepository.Insert(facility);
            await _unitOfWork.SaveChangesAsync();

            //#MISSING: SEND EMAILS
            invites.ForEach(e => e.FacilityId = facility.Id);
            await _unitOfWork.InviteRepository.InsertRange(invites);
            await _unitOfWork.SaveChangesAsync();

            units.ForEach(e => e.FacilityId = facility.Id);
            await _unitOfWork.UnitRepository.InsertRange(units);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> InsertFacility(Facility facility)
        {
            facility.IsActive = true;
            facility.CreationDate = DateTime.Now;
            await _unitOfWork.FacilityRepository.Insert(facility);
            await _unitOfWork.SaveChangesAsync();
            return facility.Id;
        }

        public async Task<Dictionary<string, int>> InsertFacilitiesRange(List<Facility> facilities)
        {
            facilities.ForEach(e => e.IsActive = true);
            facilities.ForEach(e => e.CreationDate = DateTime.Now);
            await _unitOfWork.FacilityRepository.InsertRange(facilities);
            await _unitOfWork.SaveChangesAsync();
            var facilitiesDic = facilities.ToDictionary(t => t.Name, t => t.Id);
            return facilitiesDic;
    }

        public async Task<bool> UpdateFacility(Facility facility, Address address)
        {
            var existingFacility = await _unitOfWork.FacilityRepository.GetById(facility.Id);

            var existingAddress = await _unitOfWork.AddressRepository.GetById(existingFacility.AddressId.Value);

            if (!String.IsNullOrEmpty(facility.Name))
                existingFacility.Name = facility.Name;

            if (!String.IsNullOrEmpty(facility.PhoneNumber))
                existingFacility.PhoneNumber = facility.PhoneNumber;

            existingFacility.OrganizationId = facility.OrganizationId;

            if (!String.IsNullOrEmpty(address.Description))
                existingAddress.Description = address.Description;

            if (!String.IsNullOrEmpty(address.City))
                existingAddress.City = address.City;

            existingAddress.StateId = address.StateId;
            
            _unitOfWork.FacilityRepository.Update(existingFacility);
            _unitOfWork.AddressRepository.Update(existingAddress);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFacility(int id)
        {
            await _unitOfWork.FacilityRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Facility> GetFacility(int id)
        {
            return await _unitOfWork.FacilityRepository.GetById(id);
        }

        public async Task<FacilityProfileDto> GetFacilityProfile(int id)
        {
            Facility facility = await _unitOfWork.FacilityRepository.GetById(id);
            FacilityProfileDto facilityProfileDto = new FacilityProfileDto();
            return null;
        }
        
        public async Task<PagedList<Facility>> GetFacilities(FacilityQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            var facilities = filters.OrganizationId.HasValue ?
                await _unitOfWork.FacilityRepository.GetFacilitiesByOrganization(filters.OrganizationId.Value) :
                _unitOfWork.FacilityRepository.GetAll().ToList();

            if (!String.IsNullOrEmpty(filters.Name))
            {
                facilities = facilities.Where(x => x.Name.ToLower().Contains(filters.Name.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(filters.City))
            {
                facilities = facilities.Where(x => x.Address.City.ToLower().Contains(filters.City.ToLower())).ToList();
            }

            if (filters.StateId.HasValue)
            {
                facilities = facilities.Where(x => x.Address.StateId == filters.StateId.Value).ToList();
            }

            facilities = facilities.Where(x => x.IsActive == filters.Status).ToList();

            var pagedFacilities = PagedList<Facility>.Create(facilities, filters.PageNumber, filters.PageSize);

            return pagedFacilities;
        }

        public async Task<bool> ExistsFacility(string name, int organizationId)
        {
            Facility existingFacility = await _unitOfWork.FacilityRepository.GetFacilityByName(name, organizationId);
            return existingFacility != null;
        }

        public async Task<bool> ActivateFacility(int id)
        {
            await _unitOfWork.FacilityRepository.Activate(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateFacility(int id)
        {
            await _unitOfWork.FacilityRepository.Deactivate(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
