using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class Supervisor : BaseEntity
    {
        public bool IsOwner { get; set; }
        public Guid UserId { get; set; }
        public int? FacilityId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }
    }
}
