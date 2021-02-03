using JWA.Auth.Models;
using JWA.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreInterface = JWA.Core.Interfaces;
namespace JWA.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly CoreInterface.ISendEmailService sendEmailService;
        private readonly CoreInterface.IInviteService inviteService;

        public UserController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager
                                , IConfiguration configuration, RoleManager<IdentityRole> roleManager
                                , CoreInterface.ISendEmailService sendEmailService, 
                                    CoreInterface.IInviteService inviteService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.sendEmailService = sendEmailService;
            this.inviteService = inviteService;
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await userManager.FindByEmailAsync(model.Email);                        
                        var ResultConfirmed = await userManager.ConfirmEmailAsync(user, model.Token);
                        if (user.EmailConfirmed)
                        {
                            return BadRequest("Email is already confirmed");
                        }
                        if (ResultConfirmed.Succeeded)
                        {
                            var token = await userManager.GeneratePasswordResetTokenAsync(user);
                            var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                            if (user.EmailConfirmed)
                            {
                                var inviteduser = inviteService.GetInviteByEmailId(model.Email);

                                var role = await roleManager.FindByIdAsync(inviteduser.RoleId.ToString());

                                var roleresult = await userManager.AddToRoleAsync(user, role.Name);
                                if (roleresult.Succeeded)
                                {
                                    inviteService.RemoveInvite(model.Email);
                                    return Ok("User confirmed successfully");
                                }
                                else
                                {
                                    await userManager.DeleteAsync(user);
                                }
                            }
                            else
                            {
                                await userManager.DeleteAsync(user);
                            }
                            return BadRequest("User confirmation failed");
                        }
                        return BadRequest(ResultConfirmed);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.ToString());
                    }
                }
                return BadRequest("Model state is not valid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize(Roles = "FacilityManager,FacilityAdministrator,OrganizationManager,OrganizationAdministrator")]
        public async Task<IActionResult> ChangePassword(ChangeUserPasswordDto changeUserViewModel)
        {
            try
            {
                if (!Request.Headers.TryGetValue("Cookie", out var Cookie))
                {
                    return BadRequest("User must be signed in first");
                }
                    var user = await userManager.FindByEmailAsync(changeUserViewModel.Email);
                if (user != null && await userManager.CheckPasswordAsync(user, changeUserViewModel.OldPassword))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    if (user == null)
                    {
                        return BadRequest("User Does Not Exist");
                    }
                    var result = await userManager.ResetPasswordAsync(user, token, changeUserViewModel.NewPassword);
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Incorrect Current Password or User does not exist");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [HttpPost]
        [Route("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(EmailDto Params)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(Params.Email);
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    string baseUrl = string.Empty;
                    var PasswordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                    baseUrl = Url.Action("RecoverPassword", "User",
                            new { token = PasswordResetToken, userId = user.Id}, Request.Scheme);
                    var API_Key = AppConstants.SendGridKey;
                    var emailBody = string.Format(AppConstants.RecoverPasswordEmailTemplate, PasswordResetToken);
                    var emailsubjects = AppConstants.RecoverEmailSubject;

                    var result = await sendEmailService.send_email_sendgrid(API_Key, user.Email, emailsubjects, emailBody);
                    if (result.StatusCode.ToString() == "Accepted")
                    {
                        return Ok(new { result.IsSuccessStatusCode, result.StatusCode });
                    }
                }
                return BadRequest("Either user does not exist or is not confirmed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }           
        }
        [HttpPost]
        [Route("RecoverPassword")]
        [Authorize]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordDto Model)
        {
            try
            {
                if (Model.Email != null && Model.NewPassword != null)
                {
                    var user = await userManager.FindByEmailAsync(Model.Email);
                    if (user != null)
                    {
                        var result = await userManager.ResetPasswordAsync(user, Model.Token, Model.NewPassword);
                        if (result.Succeeded)
                        {
                            return Ok("Password changed successfully");
                        }
                    }
                    return Ok("User does not exist"); 
                }
                else
                {
                    return BadRequest("Email or Password can not be null");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [HttpPost]
        [Route("EditProfile")]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileDto editProfileViewModel)
        {
            try
            {
                string token = string.Empty;
                var IsHeader = Request.Headers.TryGetValue("Authorization", out var traceValue);
                if (IsHeader)
                {
                    token = traceValue[0].Split(' ')[2];
                }
                var _token = new JwtSecurityTokenHandler();
                var TokenS = _token.ReadJwtToken(token) as JwtSecurityToken;
                var TokenMail = TokenS.Claims.FirstOrDefault().Value;
                if (TokenMail == editProfileViewModel.Email)
                {
                    var user = await userManager.FindByEmailAsync(editProfileViewModel.Email);
                    if (user != null)
                    {
                        user.PhoneNumber = editProfileViewModel.Phone;
                        var result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return Ok("Profile Updated successfully");
                        }
                        return Ok("Profile updation failed");
                    }
                    return BadRequest("User does not exists");
                }
                return BadRequest("Invalid Token");
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [HttpPost]
        [Route("SignIn")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInDto signIn)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(signIn.Email);
                if (user != null && await userManager.CheckPasswordAsync(user, signIn.Password))
                {
                    var result = await signInManager.PasswordSignInAsync(signIn.Email, signIn.Password, signIn.RememberMe, false);
                    var userRoles = await userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                {                                                                         // claims to be added here
                    new Claim(ClaimTypes.Name, user.UserName),                            //
                    new Claim(ClaimTypes.Email,user.Email),                               //
                    new Claim(ClaimTypes.NameIdentifier,user.Id),                         //
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),    //
                };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: configuration["JWT:ValidIssuer"],
                        audience: configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddDays(30),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        result = result
                    });
                }
                else
                {
                    return BadRequest("username/password is incorrect");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [HttpGet]
        [Route("SignOut")]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            try
            {
                if (Request.Headers.TryGetValue("Cookie", out var Cookie))
                {
                    await signInManager.SignOutAsync();
                    return Ok("User signed out");
                }
                else
                {
                    return BadRequest("User is not Signed In");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("DeleteUser")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(EmailDto Params)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(Params.Email);
                if (user != null)
                {
                    await userManager.DeleteAsync(user);              
                }
                else
                {
                    return Ok("User does not exists");
                }
                var userRoles = await userManager.GetRolesAsync(user);
                foreach (var userRole in userRoles)
                {
                    await userManager.RemoveFromRoleAsync(user, userRole);
                }
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
