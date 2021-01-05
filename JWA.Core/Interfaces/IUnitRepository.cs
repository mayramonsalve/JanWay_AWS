using JWA.Core.Entities;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IUnitRepository : IRepository<Unit>
    {
        //Task<Unit> GetByEmail(string email);
    }
}
