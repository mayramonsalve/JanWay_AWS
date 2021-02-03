﻿using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.Exceptions;
using JWA.Core.Interfaces;
using JWA.Core.QueryFilters;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Core.Services
{
    public class InviteService : IInviteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInviteRepository inviteRepository;
        private readonly PaginationOptions _paginationOptions;

        public InviteService(IOptions<PaginationOptions> options, IInviteRepository inviteRepository)
        {
            this.inviteRepository = inviteRepository;
            _paginationOptions = options.Value;
        }

        public async Task InsertInvite(Invite invite, ClaimsPrincipal User)
        {
            var existingInvite = GetInviteByEmail(invite.Email);
            if(existingInvite != null)
                throw new BusinessException("Email already exists.");
            var role = await _unitOfWork.RoleRepository.GetById(invite.RoleId);
            if (role == null)
                throw new BusinessException("Role doesn't exist.");
            else
            {
                int userRoleId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value);
                if (role.IsInternal)
                {
                    ///if(role.Id < userRoleId)
                    //    throw new BusinessException($"User doesn't have permission to invite a {role.Name} user.");
                }
                else { 
                    if(role.Name.Contains("Organization") && !invite.OrganizationId.HasValue)
                    {
                        throw new BusinessException($"Organization is required for {role.Name} role.");
                    }
                    if (role.Name.Contains("Facility") && !invite.FacilityId.HasValue)
                    {
                        throw new BusinessException($"Facility is required for {role.Name} role.");
                    }
                }
            }
            await _unitOfWork.InviteRepository.Insert(invite);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteInvite(int id)
        {
            await _unitOfWork.InviteRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public Invite GetInvite(int id)
        {
            return _unitOfWork.InviteRepository.GetByInviteId(id);
        }

        public Invite GetInviteByEmail(string email)
        {
            return _unitOfWork.InviteRepository.GetByEmail(email);
        }

        public PagedList<Invite> GetInvites(InviteQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            var invites = _unitOfWork.InviteRepository.GetAll();

            if (!String.IsNullOrEmpty(filters.Email))
            {
                invites = invites.Where(x => x.Email.ToLower().Contains(filters.Email.ToLower()));
            }

            if (!String.IsNullOrEmpty(filters.Role))
            {
                invites = invites.Where(x => x.Role.Name.ToLower().Contains(filters.Role.ToLower()));
            }

            if (!String.IsNullOrEmpty(filters.Organization))
            {
                invites = invites.Where(x => x.Organization.Name.ToLower().Contains(filters.Organization.ToLower()));
            }

            if (!String.IsNullOrEmpty(filters.Facility))
            {
                invites = invites.Where(x => x.Facility.Name.ToLower().Contains(filters.Facility.ToLower()));
            }

            var pagedInvites = PagedList<Invite>.Create(invites, filters.PageNumber, filters.PageSize);

            return pagedInvites;
        }
        public async Task<Invite> InsertInvite(Invite invite)
        {
            try
            {
                var existingInvite = GetInviteByEmailId(invite.Email);
                if (existingInvite != null)
                {
                    throw new BusinessException("Email already exists.");
                }
                await inviteRepository.Insert(invite);
                //await GetInviteByEmailId.SaveChangesAsync();
                inviteRepository.SaveChanges();
                return invite;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemoveInvite(string email)
        {
            return inviteRepository.RemoveInvite(email);
        }

        public Invite GetInviteByEmailId(string email)
        {
            return inviteRepository.GetByEmail(email);
        }
    }
}
