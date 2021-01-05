using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IUserService
    {
        PagedList<User> GetUsers(UserQueryFilter filters);
        Task<User> GetUser(Guid id);
        Task<Guid> InsertUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> UpdateRole(User user);
        Task<bool> UpdatePassword(User user);
        Task<bool> DeleteUser(Guid id);
        Task<User> GetUserByCredentials(SignIn signIn);
    }
}