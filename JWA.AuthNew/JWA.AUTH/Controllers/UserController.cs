using JWA.AUTH.Interface;
using JWA.AUTH.Models;
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

namespace JWA.AUTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IInviteService invite;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IGeneralServices generalServices;

        public UserController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager
                                , IConfiguration configuration, IInviteService invite,RoleManager<IdentityRole> roleManager
                                , IGeneralServices generalServices)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.invite = invite;
            this.roleManager = roleManager;
            this.generalServices = generalServices;
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //var ResultConfirmed = await userManager.ConfirmEmailAsync(user, model.token);
                    user = await userManager.FindByEmailAsync(user.Email);
                    user.EmailConfirmed = true;
                    await userManager.UpdateAsync(user);
                    if (user.EmailConfirmed)
                    {
                        var InvitedUser = invite.GetInviteByEmailId(model.Email);

                        var role = await roleManager.FindByIdAsync(InvitedUser.role_id.ToString());

                        var roleresult = await userManager.AddToRoleAsync(user, role.Name);
                        if (roleresult.Succeeded)
                        {
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
            return BadRequest();
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangeUserViewModel changeUserViewModel)
        {
            var user = await userManager.FindByEmailAsync(changeUserViewModel.Email);
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            if (user == null)
            {
                return BadRequest("User Does Not Exist");
            }
            var result = await userManager.ResetPasswordAsync(user, token, changeUserViewModel.NewPassword);
            return Ok(result);
        }
        [HttpPost]
        [Route("ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                string baseUrl = string.Empty;
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                baseUrl = Url.Action("RecoverPassword", "User",
                        new { userId = user.Id, token = token }, Request.Scheme);
                var API_Key = AppConstants.SendGridKey;
                var emailBody = string.Format(AppConstants.InviteEmailTemplate, baseUrl);
                var emailsubjects = string.Format(AppConstants.InviteEmailSubject, "Test");

                var result = await generalServices.send_email_sendgrid(API_Key, user.Email, emailsubjects, emailBody);
                if (result.StatusCode.ToString() == "Accepted")
                {
                    return Ok(result );
                }
            }
            
            return Ok();
        }
        [HttpPost]
        [Route("RecoverPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoverPassword(string Token,string Email,string NewPassword)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                var result = await userManager.ResetPasswordAsync(user, Token, NewPassword);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            return Ok();
        }
        [HttpPost]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editProfileViewModel)
        {
            var user = await userManager.FindByEmailAsync(editProfileViewModel.Email);            
            if (user != null)
            {
                user.PhoneNumber = editProfileViewModel.Phone;
                var result = await userManager.UpdateAsync(user);
            }
            return Ok();
        }
        [HttpPost]
        [Route("SignIn")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignIn signIn)
        {
            var user = await userManager.FindByEmailAsync(signIn.Email);
            if(user!=null && await userManager.CheckPasswordAsync(user, signIn.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            else
            {
                return BadRequest("username/password is incorrect");
            }
        }
        [HttpPost]
        [Route("DeleteUser")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                //var userRoles = await userManager.GetRolesAsync(user);
                //foreach (var userRole in userRoles)
                //{
                //    await userManager.RemoveFromRoleAsync(user, userRole);
                //}                
            }
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                await userManager.RemoveFromRoleAsync(user, userRole);
            }
            return Ok();
        }
    }
}
