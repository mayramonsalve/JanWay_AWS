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
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> InsertAddress(Address address)
        {
            address.ZipCode = "0000";
            address.Latitude = 0;
            address.Longitude = 0;
            address.CreationDate = DateTime.Now;
            await _unitOfWork.AddressRepository.Insert(address);
            await _unitOfWork.SaveChangesAsync();
            return address.Id;
        }

        public async Task<bool> UpdateAddress(Address address)
        {
            var existingAddress = await _unitOfWork.AddressRepository.GetById(address.Id);

            if (!String.IsNullOrEmpty(address.Description))
                existingAddress.Description = address.Description;

            if (!String.IsNullOrEmpty(address.City))
                existingAddress.City = address.City;

            existingAddress.StateId = address.StateId;
            
            _unitOfWork.AddressRepository.Update(existingAddress);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAddress(int id)
        {
            await _unitOfWork.AddressRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Address> GetAddress(int id)
        {
            return await _unitOfWork.AddressRepository.GetById(id);
        }
    }
}
