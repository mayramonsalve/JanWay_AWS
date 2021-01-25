using JWA.AUTH.Interface;
using JWA.AUTH.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.AUTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InviteController : ControllerBase
    {
        private readonly IInviteService _invite;
        private readonly IGeneralServices _generalServices;
        private readonly UserManager<IdentityUser> userManager;

        public InviteController(IInviteService invite, IGeneralServices generalServices, 
                                        UserManager<IdentityUser> userManager)
        {
            _invite = invite;
            _generalServices = generalServices;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("InviteUser")]
        public async Task<IActionResult> InviteUser(InviteViewModel model)
        {
            var ExistingUser = _invite.GetInviteByEmailId(model.Email);
            if (ExistingUser != null)
            {
                return BadRequest("User is already register");
            }
            Invite invite = new Invite();
            invite.email = model.Email;
            invite.creation_date = DateTime.Now;
            invite.facility_id = model.FacilityId;
            invite.organization_id = model.organization_id;
            invite.role_id = model.RoleId;

            string token = string.Empty;
            string baseUrl = string.Empty;
            var InvitedUser = _invite.InsertInvite(invite);
            if (InvitedUser.id > 0)
            {
                var user = new IdentityUser { UserName = InvitedUser.email, Email = InvitedUser.email };
                token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                baseUrl= Url.Action("ConfirmEmail", "User",
                        new { userId = InvitedUser.id, token = token }, Request.Scheme);
                var API_Key = AppConstants.SendGridKey;
                var emailBody = string.Format(AppConstants.InviteEmailTemplate, baseUrl);
                var emailsubjects = string.Format(AppConstants.InviteEmailSubject, "Test");

                var result = await _generalServices.send_email_sendgrid(API_Key, InvitedUser.email, emailsubjects, emailBody);
            }            
            return Ok(new { InvitedUser, token, baseUrl});
        }
        [HttpGet]
        [Route("GetInvitedUser")]
        public IActionResult GetInvitedUser(string email)
        {
            var invite = _invite.GetInviteByEmailId(email);
            return Ok(invite);
        }
        [HttpGet]
        [Route("DeleteInvitedUser")]
        public IActionResult DeleteInvitedUser(string email)
        {
            var invite = _invite.GetInviteByEmailId(email);
            if (invite == null)
            {
                return BadRequest("Invite does not exist");
            }
            var DeletedInvite = _invite.DeleteInvite(invite);
            return Ok(DeletedInvite);
        }
    }
}
