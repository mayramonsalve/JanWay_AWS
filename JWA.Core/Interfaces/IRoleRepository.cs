using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAll();
        Task<Role> GetById(int id);
    }
}
