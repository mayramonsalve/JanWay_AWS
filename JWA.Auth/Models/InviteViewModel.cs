using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.Auth.Models
{
    public class InviteViewModel
    {
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public int FacilityId { get; set; }
        public int organization_id { get; set; }
    }
}
