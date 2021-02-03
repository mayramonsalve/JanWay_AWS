using System;

namespace JWA.Core.DTOs
{
    /// <summary>
    /// User data sent over the network.
    /// </summary>
    public class InviteDtos
    {
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public int? OrganizationId { get; set; }
        public int? FacilityId { get; set; }
    }
}
