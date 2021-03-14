using JWA.Auth.Interface;
using JWA.Auth.Models;
using CoreInterface = JWA.Core.Interfaces;
using EntitiesModel = JWA.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWA.Core.Interfaces;
using JWA.Core.DTOs;

namespace JWA.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InviteController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IOrganizationServices organizationServices;
        private readonly CoreInterface.ISendEmailService _sendEmailService;
        private readonly CoreInterface.IInviteService inviteService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IOrganizationService organizationService;

        public InviteController(UserManager<IdentityUser> userManager,IOrganizationServices organizationServices,
                                        CoreInterface.ISendEmailService sendEmailService, CoreInterface.IInviteService inviteService
                                        , RoleManager<IdentityRole> roleManager, CoreInterface.IOrganizationService organizationService)
        {
            this.userManager = userManager;
            this.organizationServices = organizationServices;
            _sendEmailService = sendEmailService;
            this.inviteService = inviteService;
            this.roleManager = roleManager;
            this.organizationService = organizationService;
        }

        [HttpPost]
        [Route("InviteUser")]
        public async Task<IActionResult> InviteUser(InviteDtos model)
        {
            try
            {
                var ExistingUser = inviteService.GetInviteByEmailId(model.Email);
                var role = await roleManager.FindByIdAsync(model.RoleId.ToString());
                if (role == null)
                {
                    return BadRequest("Role id does not exists");
                }
                if (ExistingUser != null)
                {
                    return BadRequest("User is already register");
                }
                EntitiesModel.Invite invite = new EntitiesModel.Invite();
                invite.Email = model.Email;
                invite.CreationDate = DateTime.Now;
                invite.FacilityId = model.FacilityId;
                invite.OrganizationId = model.OrganizationId;
                invite.RoleId = model.RoleId;
                var organization = await organizationService.GetOrganization((int)model.OrganizationId);
                if (organization == null)
                {
                    return BadRequest("Organisation Id does not exists");
                }
                string token = string.Empty;
                string baseUrl = string.Empty;
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var IdentityUser = await userManager.CreateAsync(user);
                if (!IdentityUser.Succeeded)
                {
                    return BadRequest("User already exists");
                }
                var InvitedUser = await inviteService.InsertInvite(invite);
                if (InvitedUser.Id > 0)
                {
                    token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    baseUrl = Url.Action("ConfirmEmail", "User",
                            new { userId = InvitedUser.Id, token = token }, Request.Scheme);
                    var API_Key = AppConstants.SendGridKey;
                    var emailBody = string.Format(AppConstants.InviteEmailTemplate, baseUrl);
                    var emailsubjects = string.Format(AppConstants.InviteEmailSubject, organization.Name);

                    var result = await _sendEmailService.send_email_sendgrid(API_Key, invite.Email, emailsubjects, emailBody);
                }
                return Ok(new { token, baseUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("GetInvitedUser")]
        public IActionResult GetInvitedUser(EmailDto Params)
        {
            var invite = inviteService.GetInviteByEmailId(Params.Email);
            return Ok(invite);
        }

        [HttpGet]
        [Route("DeleteInvitedUser")]
        public async Task<IActionResult> DeleteInvitedUser(EmailDto Params)
        {
            var Isdeleted = inviteService.RemoveInvite(Params.Email);
            var user = await userManager.FindByEmailAsync(Params.Email);
            if (!user.EmailConfirmed)
            {
                await userManager.DeleteAsync(user);
            }
            if (Isdeleted)
            {
                return Ok("Invitation deleted Successfully");
            }
            return BadRequest("Invitation was not deletd either invite does not exists or input was not in correct fomat");
        }
    }
}
