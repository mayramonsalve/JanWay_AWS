using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class Facility : BaseEntity
    {
        public Facility()
        {
            Units = new HashSet<Unit>();
            Supervisors = new HashSet<Supervisor>();
            Invites = new HashSet<Invite>();
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int OrganizationId { get; set; }
        public int AddressId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Address Address { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ICollection<Supervisor> Supervisors { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
    }
}
