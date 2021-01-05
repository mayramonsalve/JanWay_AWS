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
    public class FacilityService : IFacilityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public FacilityService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task InsertFacility(Facility facility)
        {
            
            await _unitOfWork.FacilityRepository.Insert(facility);
            await _unitOfWork.SaveChangesAsync();
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

        public PagedList<Facility> GetFacilities()//InviteQueryFilter filters)
        {
            //filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            //filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            //var invites = _unitOfWork.InviteRepository.GetAll();

            //if (!String.IsNullOrEmpty(filters.Email))
            //{
            //    invites = invites.Where(x => x.Email.ToLower().Contains(filters.Email.ToLower()));
            //}

            //if (!String.IsNullOrEmpty(filters.Role))
            //{
            //    invites = invites.Where(x => x.Role.Name.ToLower().Contains(filters.Role.ToLower()));
            //}

            //if (!String.IsNullOrEmpty(filters.Organization))
            //{
            //    invites = invites.Where(x => x.Organization.Name.ToLower().Contains(filters.Organization.ToLower()));
            //}

            //if (!String.IsNullOrEmpty(filters.Facility))
            //{
            //    invites = invites.Where(x => x.Facility.Name.ToLower().Contains(filters.Facility.ToLower()));
            //}

            //var pagedInvites = PagedList<Invite>.Create(invites, filters.PageNumber, filters.PageSize);

            //return pagedInvites;
            return null;
        }

    }
}