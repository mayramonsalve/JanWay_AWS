using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.AUTH.Models
{
    public class Roles1 : IdentityRole
    {
        public bool IsInternal { get; set; }
    }
}
