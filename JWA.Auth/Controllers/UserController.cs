using JWA.Auth.Models;
using JWA.Core.CustomEntities;
using JWA.Core.Entities;
using JWA.Core.Interfaces;
using JWA.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IInviteService _inviteservice;
        private readonly IOrganizationService _organizationService;
        private readonly ISendEmailService _sendEmailService;
        private readonly IUserRolesService _userRolesService;
        public UserController(IUserService userService, IPasswordService passwordService,
                                IInviteService inviteservice, IOrganizationService organizationService,
                                ISendEmailService sendEmailService, IUserRolesService userRolesService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _inviteservice = inviteservice;
            _organizationService = organizationService;
            _sendEmailService = sendEmailService;
            _userRolesService = userRolesService;
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel confirmEmailViewModel)
        {
            try
            {
                var ExistingUserName = _userService.GetUserByUserName(confirmEmailViewModel.Email);
                if (confirmEmailViewModel.Password != confirmEmailViewModel.ConfirmPassword)
                {
                    return BadRequest("Email is not confirmed");
                }
                else if (ExistingUserName != null)
                {
                    return BadRequest("UserName already exists");
                }
                else
                {
                    try
                    {
                        User user = new User();
                        user.Id = Guid.NewGuid();
                        user.Email = confirmEmailViewModel.Email;
                        user.UserName = confirmEmailViewModel.Email;
                        user.NormalizedUserName = confirmEmailViewModel.Email;
                        user.NormalizedEmail = confirmEmailViewModel.Email;
                        user.EmailConfirmed = true;
                        user.PasswordHash = _passwordService.Hash(confirmEmailViewModel.Password);
                        user.PhoneNumberConfirmed = false;
                        user.TwoFactorEnabled = false;
                        user.LockoutEnabled = false;
                        user.AccessFailedCount = 0;
                        user.CreationDate = DateTime.Now;

                        var NewUserId = await _userService.InsertUser(user);
                        var invite = _inviteservice.GetInviteByEmailId(confirmEmailViewModel.Email);
                        UserRole userRole = new UserRole();
                        userRole.RoleId = invite.RoleId;
                        userRole.UserId = NewUserId;
                        await _userRolesService.InsertRoleUser(userRole);
                        var invitedelete = _inviteservice.DeleteInvite(invite.Id);
                        return Ok(user);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("InviteUser")]
        public async Task<IActionResult> InviteUser(InviteViewModel inviteViewModel)
        {
            Invite invite = new Invite();
            invite.Email = inviteViewModel.Email;
            invite.FacilityId = inviteViewModel.FacilityId;
            invite.OrganizationId = inviteViewModel.organization_id;
            invite.RoleId = inviteViewModel.RoleId;
            invite.CreationDate = DateTime.Now;
            var InvitedUser = await _inviteservice.InsertInvite(invite);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
                                {
                                   new Claim(ClaimTypes.Name, InvitedUser.Email),
                                   new Claim(ClaimTypes.NameIdentifier, InvitedUser.Id.ToString()),
                                 }, "Identity.Application");

            var token = _passwordService.CreateAccessToken(_passwordService.CreateJwtClaims(claimsIdentity));

            var baseUrl = string.Format("{0}://{1}{2}/", Request.Scheme, Request.Host, Request.PathBase) + "api/User/GetInvitedUser?token=" + token;
            //api / User / GetInvitedUser ? id = 4
            var organisation = await _organizationService.GetOrganization(inviteViewModel.organization_id);
            var emailBody = string.Format(AppConstants.InviteEmailTemplate, baseUrl);
            var emailsubjects = string.Format(AppConstants.InviteEmailSubject, organisation.Name);
            var apiKey = AppConstants.SendGridKey;
            var result = await _sendEmailService.send_email_sendgrid(apiKey, invite.Email, emailsubjects, emailBody);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetInvitedUser")]
        public async Task<IActionResult> GetInvitedUser(string token)
        {
            try
            {
                var UserDetail = _passwordService.ReadToken(token);
                var userEmail = UserDetail.Claims.FirstOrDefault().Value;

                var user = _inviteservice.GetInviteByEmailId(userEmail);

                var organisation = await _organizationService.GetOrganization((int)user.OrganizationId);

                return Ok(new { user.Email, organisation.Name });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Authorize(Roles= "TestTRole")]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                var User = _userService.GetUserByUserName(changePasswordViewModel.Email);
                if (User == null)
                {
                    return BadRequest(changePasswordViewModel.Email + "is not registered. Or it is not confirmed");
                }
                else
                {
                    var is_password_true = _passwordService.Check(User.PasswordHash, changePasswordViewModel.Password);
                    if (is_password_true)
                    {
                        User.PasswordHash = _passwordService.Hash(changePasswordViewModel.NewPassword);
                        await _userService.UpdateUser(User);
                        return Ok("user password is updated successfully");
                    }
                    else
                    {
                        return BadRequest("Old password is in correct");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Recover Password
        [HttpPost]
        [Route("ForgetPassword")]
        public void ForgetPassword(string email)
        {
            try
            {
                var User = _userService.GetUserByEmail(email);
                var claimsIdentity = new ClaimsIdentity(new List<Claim>()
                                {
                                   new Claim(ClaimTypes.Email, User.Email),
                                   new Claim(ClaimTypes.NameIdentifier, User.Email),
                                 }, "Identity.Application");

                var token = _passwordService.CreateAccessToken(_passwordService.CreateJwtClaims(claimsIdentity));

                if (User == null)
                {
                    throw new Exception(email + "is not registered. Or it is not confirmed");
                }
                else
                {
                    var apiKey = AppConstants.SendGridKey;
                    var EmailBody = string.Format(AppConstants.ForgetPasswordEmailBody, token);
                    _sendEmailService.send_email_sendgrid(apiKey, email, AppConstants.ForgetPasswordEmailSubject, EmailBody);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel recoverPasswordViewModel)
        {
            try
            {
                var TokenData = _passwordService.ReadToken(recoverPasswordViewModel.Token);
                string email = TokenData.Claims.FirstOrDefault().Value;
                var User = _userService.GetUserByEmail(email);

                if (User == null)
                {
                    return BadRequest(email + "is not registered. Or it is not confirmed");
                }
                else
                {
                    User.PasswordHash = _passwordService.Hash(recoverPasswordViewModel.Password);
                    await _userService.UpdateUser(User);
                    return Ok(User);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        [HttpPost]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile(EditProfileViewModel user)
        {
            try
            {
                #region get user details
                var ExistingUser = _userService.GetUserByEmail(user.Email);
                if (ExistingUser == null)
                {
                    return BadRequest("user does not exists");
                }
                #endregion

                #region update user info
                ExistingUser.Email = user.Email;
                ExistingUser.PhoneNumber = user.Phone == null ? ExistingUser.PhoneNumber : user.Phone;
                var result = await _userService.UpdateUser(ExistingUser);
                #endregion

                if (result)
                {
                    return Ok(ExistingUser);
                }
                else
                {
                    return BadRequest("user not updated successsfully");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost]
        [Route("UserExists")]
        public bool UserExists(string token)
        {
            bool IsUserExists = false;
            var TokenData = _passwordService.ReadToken(token);
            var CheckUserExists = _userService.GetUserByEmail(TokenData.Claims.FirstOrDefault().Value);
            if (CheckUserExists != null)
            {
                IsUserExists = true;
            }
            return IsUserExists;
        }

        [HttpPost]
        [Route("SignIn")]
        public IActionResult SignIn(SignIn signIn)
        {
            var CheckUserExists = _userService.GetUserByEmail(signIn.Email);
            if (CheckUserExists != null)
            {
                var IsAuthenticated = _passwordService.Check(CheckUserExists.PasswordHash, signIn.Password);
                if (IsAuthenticated)
                {
                    var claimsIdentity = new ClaimsIdentity(new List<Claim>()
                                {
                                   new Claim(ClaimTypes.Name, CheckUserExists.Email),
                                   new Claim(ClaimTypes.SerialNumber, CheckUserExists.Id.ToString()),
                                   new Claim(ClaimTypes.NameIdentifier, CheckUserExists.UserName),
                                   new Claim(ClaimTypes.Hash, CheckUserExists.PasswordHash),
                                }, "Identity.Application");

                    var token = _passwordService.CreateAccessToken(_passwordService.CreateJwtClaims(claimsIdentity));

                    return Ok(new { CheckUserExists.UserName, CheckUserExists.Email, token });
                }
            }
            else
            {
                return BadRequest("user does not exists");
            }
            return Ok();
        }

        [HttpGet]
        [Route("ExternalLoginGetRequest")]
        public IActionResult ExternalLoginGetRequest()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginPostRequest") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet]
        [Route("ExternalLoginPostRequest")]
        public async Task<IActionResult> ExternalLoginPostRequest()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault()
                .Claims.Select(Claim => new
                {
                    Claim.Issuer,
                    Claim.OriginalIssuer,
                    Claim.Type,
                    Claim.Value
                });

            return Ok(claims);
        }
    }
}
