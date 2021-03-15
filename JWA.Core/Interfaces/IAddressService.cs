using JWA.Core.CustomEntities;
using JWA.Core.DTOs;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IAddressService
    {
        Task<Address> GetAddress(int id);
        Task<int> InsertAddress(Address address);
        Task<bool> UpdateAddress(Address address);
        Task<bool> DeleteAddress(int id);
    }
}