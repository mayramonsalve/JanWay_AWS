using JWA.Core.CustomEntities;
using JWA.Core.DTOs;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface ISupervisorService
    {
        PagedList<Supervisor> GetSupervisors(UserQueryFilter filters);
        Task<Supervisor> GetSupervisor(int id);
        Task InsertSupervisor(Supervisor supervisor);
        Task<bool> UpdateSupervisor(Supervisor supervisor);
        Task<bool> DeleteSupervisor(int id);
        Task<Supervisor> GetSupervisorByUser(Guid userId);
        Task<UserDto> AddSupervisorData(UserDto users);
        Task<IEnumerable<UserDto>> AddSupervisorData(IEnumerable<UserDto> users);
    }
}