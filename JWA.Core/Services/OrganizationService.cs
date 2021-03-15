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
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public OrganizationService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task SetupOrganization(Address address, Organization organization, List<Facility> facilities, List<Invite> invites, List<Unit> units)
        {
            int addressId = 1;

            try
            {
                address.Latitude = 0;
                address.Longitude = 0;
                address.ZipCode = "0000";
                address.CreationDate = DateTime.Now;
                await _unitOfWork.AddressRepository.Insert(address);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                organization.AddressId = address.Id == 0 ? addressId : address.Id;
                organization.IsActive = true;
                organization.CreationDate = DateTime.Now;
                await _unitOfWork.OrganizationRepository.Insert(organization);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                facilities.ForEach(e => e.AddressId = (address.Id == 0 ? addressId : address.Id));
                facilities.ForEach(e => e.OrganizationId = organization.Id);
                facilities.ForEach(e => e.IsActive = true);
                facilities.ForEach(e => e.CreationDate = DateTime.Now);
                await _unitOfWork.FacilityRepository.InsertRange(facilities);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                //#MISSING: SEND EMAILS
                invites.ForEach(e => e.OrganizationId = organization.Id);
                invites.ForEach(e => e.CreationDate = DateTime.Now);
                await _unitOfWork.InviteRepository.InsertRange(invites);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                units.ForEach(e => e.CreationDate = DateTime.Now);
                units.ForEach(e => e.IpAddress = "127.0.0.1");
                units.ForEach(e => e.IsActive = true);
                units.ForEach(e => e.MacAddress = "MAC-ADDRESS");
                units.ForEach(e => e.Uin = 123);
                await _unitOfWork.UnitRepository.InsertRange(units);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<int> InsertOrganization(Organization organization)
        {
            organization.IsActive = true;
            organization.CreationDate = DateTime.Now;
            await _unitOfWork.OrganizationRepository.Insert(organization);
            await _unitOfWork.SaveChangesAsync();
            return organization.Id;
        }

        public async Task<bool> UpdateOrganization(Organization organization, Address address)
        {
            var existingOrganization = await _unitOfWork.OrganizationRepository.GetById(organization.Id);

            var existingAddress = await _unitOfWork.AddressRepository.GetById(existingOrganization.AddressId);

            if (!String.IsNullOrEmpty(organization.Name))
                existingOrganization.Name = organization.Name;

            if (!String.IsNullOrEmpty(organization.PhoneNumber))
                existingOrganization.PhoneNumber = organization.PhoneNumber;

            if (!String.IsNullOrEmpty(address.Description))
                existingAddress.Description = address.Description;

            if (!String.IsNullOrEmpty(address.City))
                existingAddress.City = address.City;

            existingAddress.StateId = address.StateId;

            _unitOfWork.OrganizationRepository.Update(existingOrganization);
            _unitOfWork.AddressRepository.Update(existingAddress);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrganization(int id)
        {
            await _unitOfWork.OrganizationRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Organization> GetOrganization(int id)
        {
            return await _unitOfWork.OrganizationRepository.GetById(id);
        }

        public async Task<OrganizationProfileDto> GetOrganizationProfile(int id)
        {
            Organization organization = await _unitOfWork.OrganizationRepository.GetById(id);
            OrganizationProfileDto organizationProfileDto = new OrganizationProfileDto();
            return null;
        }

        public PagedList<Organization> GetOrganizations(OrganizationQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            var organizations = _unitOfWork.OrganizationRepository.GetAll().ToList();

            if (!String.IsNullOrEmpty(filters.Name))
            {
                organizations = organizations.Where(x => x.Name.ToLower().Contains(filters.Name.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(filters.City))
            {
                organizations = organizations.Where(x => x.Address.City.ToLower().Contains(filters.City.ToLower())).ToList();
            }

            if (filters.StateId.HasValue)
            {
                organizations = organizations.Where(x => x.Address.StateId == filters.StateId.Value).ToList();
            }

            organizations = organizations.Where(x => x.IsActive == filters.Status).ToList();

            var pagedOrganizations = PagedList<Organization>.Create(organizations, filters.PageNumber, filters.PageSize);

            return pagedOrganizations;
        }

        public async Task<bool> ExistsOrganization(string name)
        {
            Organization existingOrganization = await _unitOfWork.OrganizationRepository.GetOrganizationByName(name);
            return existingOrganization != null;
        }

        public async Task<bool> ActivateOrganization(int id)
        {
            await _unitOfWork.OrganizationRepository.Activate(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateOrganization(int id)
        {
            await _unitOfWork.OrganizationRepository.Deactivate(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
