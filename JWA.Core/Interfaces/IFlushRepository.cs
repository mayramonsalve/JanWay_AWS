using JWA.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IFlushRepository : IRepository<Flush>
    {
        Task<IEnumerable<Flush>> GetByUnit(int unitId);
    }
}
