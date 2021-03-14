using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class Invite : BaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid RoleId { get; set; }
        public int? OrganizationId { get; set; }
        public int? FacilityId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Facility Facility { get; set; }
    }
}
