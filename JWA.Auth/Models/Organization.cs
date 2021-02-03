using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.Auth.Models
{
    public class Organization
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool is_active { get; set; }
        public int address_id { get; set; }
        public DateTime creation_date { get; set; }
    }
}
