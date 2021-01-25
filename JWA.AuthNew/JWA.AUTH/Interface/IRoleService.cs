using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.AUTH.Interface
{
    public interface IRoleService
    {
        IdentityRole GetRoleById(string id);

    }
}
