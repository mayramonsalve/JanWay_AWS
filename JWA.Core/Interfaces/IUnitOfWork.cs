using JWA.Core.Entities;
using System;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //All repositories of the app
        IAddressRepository AddressRepository { get; }
        IFacilityRepository FacilityRepository { get; }
        IFlushRepository FlushRepository { get; }
        IInviteRepository InviteRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IRoleRepository RoleRepository { get; }
        ISupervisorRepository SupervisorRepository { get; }
        ISystemStatusRepository SystemStatusRepository { get; }
        IUnitRepository UnitRepository { get; }
        IUserRepository UserRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
        //IUserRolesRepository UserRolesRepository { get; }
    }
}
