using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class Unit : BaseEntity
    {
        public Unit()
        {
            Flushes = new HashSet<Flush>();
        }

        public int Suin { get; set; }
        public int Uin { get; set; }
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? FacilityId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual ICollection<Flush> Flushes { get; set; }
    }
}
