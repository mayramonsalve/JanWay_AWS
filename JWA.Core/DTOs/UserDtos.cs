using System;

namespace JWA.Core.DTOs
{
    /// <summary>
    /// User data sent over the network.
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public int? OrganizationId { get; set; }
        public string Organization { get; set; }
        public int? FacilityId { get; set; }
        public string Facility { get; set; }
        public int? SupervisorId { get; set; }
    }

    public class ProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class ConfirmAccountRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? OrganizationId { get; set; }
        public int? FacilityId { get; set; }
    }

    public class ConfirmAccountResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int? OrganizationId { get; set; }
        public int? FacilityId { get; set; }
    }

    public class UpdateRoleDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
    }

    public class UpdatePasswordDto
    {
        public int Id { get; set; }
        public string Password { get; set; }
    }

}
