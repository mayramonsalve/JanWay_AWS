using JWA.Core.Entities;
using System;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //All repositories of the app
        IInviteRepository InviteRepository { get; }
        IFacilityRepository FacilityRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IRoleRepository RoleRepository { get; } // When repository is the same as BaseRepository
        ISupervisorRepository SupervisorRepository { get; }
        IUnitRepository UnitRepository { get; }
        IUserRepository UserRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
