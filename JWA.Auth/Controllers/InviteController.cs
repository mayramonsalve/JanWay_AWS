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
using JWA.Core.Entities;

namespace JWA.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InviteController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        private readonly CoreInterface.ISendEmailService _sendEmailService;
        private readonly CoreInterface.IInviteService inviteService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IOrganizationService organizationService;
        private readonly IFacilityService facilityService;

        public InviteController(UserManager<IdentityUser> userManager,CoreInterface.ISendEmailService sendEmailService,
										 CoreInterface.IInviteService inviteService
                                        , RoleManager<IdentityRole> roleManager, CoreInterface.IOrganizationService organizationService
                                        , IFacilityService facilityService)
        {
            this.userManager = userManager;

            _sendEmailService = sendEmailService;
            this.inviteService = inviteService;
            this.roleManager = roleManager;
            this.organizationService = organizationService;
            this.facilityService = facilityService;
        }

        [HttpPost]
        [Route("InviteUser")]
        public async Task<IActionResult> InviteUser(InviteDtos model)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(model.RoleId.ToString());
                var ExistingUser = inviteService.GetInviteByEmailId(model.Email);
                Facility facility = new Facility();
                Organization organization = new Organization();

                if (model.OrganizationId != null)
                {
                    organization = await organizationService.GetOrganization((int)model.OrganizationId);
                }
                else if (model.OrganizationId == null && model.FacilityId != null)
                {
                    return BadRequest("Facility is not associated to any of the organisation");
                }
                if (model.FacilityId != null)
                {
                    facility = facilityService.GetFacilityById((int)model.FacilityId);
                }
                if (facility == null)
                {
                    return BadRequest("Facility does not exists");
                }
                if (facility.OrganizationId == 0 || facility.OrganizationId != model.OrganizationId)
                {
                    return BadRequest("Facility does not belongs to " + organization.Name + " organisation");
                }
                if (role == null)
                {
                    return BadRequest("Role id does not exists");
                }
                if (ExistingUser != null)
                {
                    return BadRequest("User is already invited to re-invite the user please delete the previous invite and then send the invitaion again");
                }
                if (organization == null)
                {
                    return BadRequest("Organisation Id does not exists");
                }
                EntitiesModel.Invite invite = new EntitiesModel.Invite();
                invite.Email = model.Email;
                invite.CreationDate = DateTime.Now;
                invite.FacilityId = model.FacilityId;
                invite.OrganizationId = model.OrganizationId;
                invite.RoleId = model.RoleId;
                organization = await organizationService.GetOrganization((int)model.OrganizationId);
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
                            new {token = token }, Request.Scheme);
                    var API_Key = AppConstants.SendGridKey;
                    var emailBody = string.Format(AppConstants.InviteEmailTemplate, baseUrl);
                    var emailsubjects = string.Format(AppConstants.InviteEmailSubject, organization.Name);

                    var result = await _sendEmailService.send_email_sendgrid(API_Key, invite.Email, emailsubjects, emailBody);
                }
                return Ok(new { token, baseUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [HttpGet]
        [Route("GetInvitedUser")]
        public IActionResult GetInvitedUser(EmailDto Params)
        {
            try
            {
                var invite = inviteService.GetInviteByEmailId(Params.Email);
                if (invite == null)
                {
                    return BadRequest("user is not invited");
                }
                return Ok(invite);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost]
        [Route("DeleteInvitedUser")]
        public async Task<IActionResult> DeleteInvitedUser(EmailDto Params)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(Params.Email);
                if (!user.EmailConfirmed)
                {
                    await userManager.DeleteAsync(user);
                }
                var Invite = inviteService.GetInviteByEmailId(Params.Email);
                if (Invite == null)
                {
                    return BadRequest("User is not invited");
                }
                var Isdeleted = inviteService.RemoveInvite(Params.Email);
                if (Isdeleted)
                {
                    return Ok("Invitation deleted Successfully");
                }
                return BadRequest("Invitation was not deleted either invite does not exists or input was not in correct fomat");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
