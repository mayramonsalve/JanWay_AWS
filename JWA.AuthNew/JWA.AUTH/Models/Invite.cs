using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.AUTH.Models
{
    public class Invite
    {
        public int id { get; set; }
        public string email { get; set; }
        public DateTime creation_date { get; set; }
        public int facility_id { get; set; }
        public int organization_id { get; set; }
        public Guid role_id { get; set; }
    }
}
