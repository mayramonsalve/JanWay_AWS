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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public UserService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task<Guid> InsertUser(User user)
        {
            var invite = await _unitOfWork.InviteRepository.GetByEmail(user.Email);
            if (invite == null)
                throw new BusinessException("Invite doesn't exist.");
            else
            {
                //if(invite.RoleId != user.RoleId)
                    //throw new BusinessException("Role sent doesn't match the invite sent.");
            }
            SetDefaultValues(user, invite);
            await _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            await _unitOfWork.UserRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUser(Guid id)
        {
            return await _unitOfWork.UserRepository.GetById(id);
        }

        public PagedList<User> GetUsers(UserQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            var users = _unitOfWork.UserRepository.GetAll();

            /*if (!String.IsNullOrEmpty(filters.Name))
            {
                users = users.Where(x => x.Name.ToLower().Contains(filters.Name.ToLower()));
            }*/

            /*if (!String.IsNullOrEmpty(filters.Role))
            {
                users = users.Where(x => x.Role.Name.ToLower().Contains(filters.Role.ToLower()));
            }*/

            if (!String.IsNullOrEmpty(filters.Email))
            {
                users = users.Where(x => x.Email.ToLower().Contains(filters.Email.ToLower()));
            }

            var pagedUsers = PagedList<User>.Create(users, filters.PageNumber, filters.PageSize);

            return pagedUsers;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var existingUser = await _unitOfWork.UserRepository.GetById(user.Id);

            //existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;

            _unitOfWork.UserRepository.Update(existingUser);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRole(User user)
        {
            var existingUser = await _unitOfWork.UserRepository.GetById(user.Id);
            //existingUser.RoleId = user.RoleId;

            _unitOfWork.UserRepository.Update(existingUser);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePassword(User user)
        {
            var existingUser = await _unitOfWork.UserRepository.GetById(user.Id);
            //existingUser.Password = user.Password;

            _unitOfWork.UserRepository.Update(existingUser);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserByCredentials(SignIn signIn)
        {
            return await _unitOfWork.UserRepository.GetUserByCredentials(signIn);
        }

        private static void SetDefaultValues(User user, Invite invite)
        {
            //user.RoleId = invite.RoleId;
        }

    }
}
