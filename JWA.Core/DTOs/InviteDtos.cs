using System;

namespace JWA.Core.DTOs
{
    /// <summary>
    /// User data sent over the network.
    /// </summary>
    public class InviteDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Facility { get; set; }
        public Guid RoleId { get; set; }
        public int? FacilityId { get; set; }
        public int? OrganizationId { get; set; }
    }
}
