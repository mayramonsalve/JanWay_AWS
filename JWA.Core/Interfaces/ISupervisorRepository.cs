using JWA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface ISupervisorRepository : IRepository<Supervisor>
    {
        Task<Supervisor> GetSupervisorByUser(Guid userId);
        Task<IEnumerable<Supervisor>> GetSupervisorsByUsers(IEnumerable<Guid> usersId);
        Task<IEnumerable<Supervisor>> GetSupervisorsByOrganization(int organizationId);
    }
}
