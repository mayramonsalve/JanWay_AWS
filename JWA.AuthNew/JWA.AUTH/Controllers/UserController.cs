using JWA.Auth.Interface;
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
    [Authorize(Roles = "admin")]
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
                        
                        var ResultConfirmed = await userManager.ConfirmEmailAsync(user, model.token);
                        var token = await userManager.GeneratePasswordResetTokenAsync(user);
                        var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                        //var result = await userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
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
                        return BadRequest(result);
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
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize(Roles = "FacilityManager,FacilityAdministrator,OrganizationManager,OrganizationAdministrator")]
        public async Task<IActionResult> ChangePassword(ChangeUserPasswordDto changeUserViewModel)
        {
            try
            {
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
                return BadRequest(ex.ToString());
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
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    baseUrl = Url.Action("RecoverPassword", "User",
                            new { token = token, userId = user.Id}, Request.Scheme);
                    var API_Key = AppConstants.SendGridKey;
                    var emailBody = string.Format(AppConstants.RecoverPasswordEmailTemplate, token);
                    var emailsubjects = AppConstants.RecoverEmailSubject;

                    var result = await sendEmailService.send_email_sendgrid(API_Key, user.Email, emailsubjects, emailBody);
                    if (result.StatusCode.ToString() == "Accepted")
                    {
                        return Ok(result);
                    }
                }
                return BadRequest("user does not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }           
        }
        [HttpPost]
        [Route("RecoverPassword")]
        [AllowAnonymous]
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
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost]
        [Route("EditProfile")]
        [AllowAnonymous]
        public async Task<IActionResult> EditProfile(EditProfileDto editProfileViewModel)
        {
            try
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
                }
                return Ok("User does not exists");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
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
                        expires: DateTime.Now.AddHours(24),
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
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost]
        [Route("SignOut")]
        [AllowAnonymous]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return Ok();
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
                    //var userRoles = await userManager.GetRolesAsync(user);
                    //foreach (var userRole in userRoles)
                    //{
                    //    await userManager.RemoveFromRoleAsync(user, userRole);
                    //}                
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
                return BadRequest(ex.ToString());
            }
        }
    }
}
