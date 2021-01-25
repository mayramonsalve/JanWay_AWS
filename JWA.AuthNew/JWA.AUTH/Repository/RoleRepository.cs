using JWA.AUTH.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.AUTH.Repository
{
    public class RoleRepository : IRoleService
    {
        public IdentityRole GetRoleById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
