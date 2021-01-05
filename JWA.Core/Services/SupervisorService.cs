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
using System.Threading.Tasks;

namespace JWA.Core.Services
{
    public class SupervisorService : ISupervisorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public SupervisorService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public Task<bool> DeleteSupervisor(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Supervisor> GetSupervisor(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Supervisor> GetSupervisorByUser(Guid userId)
        {
            return await _unitOfWork.SupervisorRepository.GetSupervisorByUser(userId);
        }

        public PagedList<Supervisor> GetSupervisors(UserQueryFilter filters)
        {
            throw new NotImplementedException();
        }

        public async Task InsertSupervisor(Supervisor supervisor)
        {
            var invite = await _unitOfWork.InviteRepository.GetByEmail(supervisor.User.Email);

            if (invite.OrganizationId != supervisor.OrganizationId)
            {
                await _unitOfWork.UserRepository.Delete(supervisor.UserId);
                throw new BusinessException("Organization sent doesn't match the invite sent.");
            }

            await SetDefaultValuesAsync(supervisor);
            await _unitOfWork.SupervisorRepository.Insert(supervisor);
            await _unitOfWork.InviteRepository.Delete(invite.Id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateSupervisor(Supervisor supervisor)
        {
            var existingSupervisor = await _unitOfWork.SupervisorRepository.GetById(supervisor.Id);

            existingSupervisor.FacilityId = supervisor.FacilityId;

            _unitOfWork.SupervisorRepository.Update(existingSupervisor);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserDto>> AddSupervisorData(IEnumerable<UserDto> users)
        {
            var usersId = users.Select(u => u.Id);
            var supervisors = await _unitOfWork.SupervisorRepository.GetSupervisorsByUsers(usersId);

            if (supervisors != null)
            {
                users.ToList().ForEach(async u => await AddSupervisorData(u));
            }

            return users;
        }

        public async Task<UserDto> AddSupervisorData(UserDto user)
        {
            var supervisor = await _unitOfWork.SupervisorRepository.GetSupervisorByUser(user.Id);

            if (supervisor != null)
            {
                user.OrganizationId = supervisor.OrganizationId;
                user.FacilityId = supervisor.FacilityId;
                user.SupervisorId = supervisor.Id;
            }

            return user;
        }

        private async Task SetDefaultValuesAsync(Supervisor supervisor)
        {
            var supervisors = await _unitOfWork.SupervisorRepository.GetSupervisorsByOrganization(supervisor.OrganizationId);
            if(supervisors == null)
            {
                supervisor.IsOwner = true;
            }
        }
    }
}
