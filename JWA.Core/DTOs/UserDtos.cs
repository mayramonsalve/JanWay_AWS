using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;

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
    public class EmailDto
    {
        public string Email { get; set; }
    }
    public class ChangeUserPasswordDto
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ConfirmEmailDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
    public class EditProfileDto
    {
        public string Email { get; set; }
        public string Phone { get; set; }
    }
    public class RecoverPasswordDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
    public class SignInDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class LoginViewModel
    {
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }
    }
}
