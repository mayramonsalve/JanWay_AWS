using JWA.Core.CustomEntities;
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
    public class FlushService : IFlushService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public FlushService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task InsertFlush(Flush flush)
        {   //UPDATE HEALTH AND PERFORMANCE CALCULATION
            flush.Health = 100;
            flush.Performance = 100;
            flush.CreationDate = DateTime.Now;

            await _unitOfWork.FlushRepository.Insert(flush);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteFlush(int id)
        {
            await _unitOfWork.FlushRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Flush> GetFlush(int id)
        {
            return await _unitOfWork.FlushRepository.GetById(id);
        }

        public PagedList<Flush> GetFlushes()//FlushQueryFilter filters)
        {
            //filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            //filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            //var invites = _unitOfWork.InviteRepository.GetAll().ToList();

            //if (!String.IsNullOrEmpty(filters.Email))
            //{
            //    invites = invites.Where(x => x.Email.ToLower().Contains(filters.Email.ToLower())).ToList();
            //}

            //if (!String.IsNullOrEmpty(filters.Role))
            //{
            //    invites = invites.Where(x => x.Role.Name.ToLower().Contains(filters.Role.ToLower())).ToList();
            //}

            //if (!String.IsNullOrEmpty(filters.Organization))
            //{
            //    invites = invites.Where(x => x.Organization.Name.ToLower().Contains(filters.Organization.ToLower())).ToList();
            //}

            //if (!String.IsNullOrEmpty(filters.Facility))
            //{
            //    invites = invites.Where(x => x.Facility.Name.ToLower().Contains(filters.Facility.ToLower())).ToList();
            //}

            //var pagedInvites = PagedList<Invite>.Create(invites, filters.PageNumber, filters.PageSize);

            //return pagedInvites;
            return null;
        }

    }
}
