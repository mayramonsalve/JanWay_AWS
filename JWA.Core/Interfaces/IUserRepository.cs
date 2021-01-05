using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        Task<User> GetById(Guid id);
        Task Insert(User entity);
        void Update(User entity);
        Task Delete(Guid id);
        Task<User> GetUserByCredentials(SignIn signIn);
    }
}
