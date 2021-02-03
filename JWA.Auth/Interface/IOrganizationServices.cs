using JWA.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.Auth.Interface
{
    public interface IOrganizationServices
    {
        Organization GetOrganization(int id);
    }
}
