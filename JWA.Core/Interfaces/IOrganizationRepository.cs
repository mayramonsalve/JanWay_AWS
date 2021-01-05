using JWA.Core.Entities;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        //Task<Organization> Get(string email);
    }
}
