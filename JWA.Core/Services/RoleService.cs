using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.Exceptions;
using JWA.Core.Interfaces;
using JWA.Core.QueryFilters;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public RoleService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task<Role> GetRole(Guid id)
        {
            return await _unitOfWork.RoleRepository.GetById(id);
        }

        public PagedList<Role> GetRoles(RoleQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            var roles = _unitOfWork.RoleRepository.GetAll();

            if (filters.IsInternal.HasValue)
            {
                roles = roles.Where(x => x.IsInternal == filters.IsInternal.Value);
            }

            var pagedRoles = PagedList<Role>.Create(roles, filters.PageNumber, filters.PageSize);

            return pagedRoles;
        }

    }
}
