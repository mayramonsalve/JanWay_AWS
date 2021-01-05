using AutoMapper;
using JWA.Api.Response;
using JWA.Core.CustomEntities;
using JWA.Core.DTOs;
using JWA.Core.Entities;
using JWA.Core.Interfaces;
using JWA.Core.QueryFilters;
using JWA.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWA.Api.Controllers
{
    //[Authorize(Roles = typeof(RoleType.Administrator), typeof(RoleType.Manager))]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvitesController : ControllerBase
    {
        private readonly IInviteService _inviteService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IPasswordService _passwordService;
    
        public InvitesController(IInviteService inviteService, IMapper mapper, IUriService uriService, IPasswordService passwordService)
        {
            _inviteService = inviteService;
            _mapper = mapper;
            _uriService = uriService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Retrieve all invites depending on user role and organization.
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [HttpGet(Name = "[controller][action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<InviteDtos>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetAll([FromQuery]InviteQueryFilter filters)
        {
            var invites = _inviteService.GetInvites(filters);
            var invitesDto = _mapper.Map<IEnumerable<InviteDtos>>(invites);

            var metadata = new Metadata
            {
                TotalCount = invites.TotalCount,
                PageSize = invites.PageSize,
                CurrentPage = invites.CurrentPage,
                TotalPages = invites.TotalPages,
                HasNextPage = invites.HasNextPage,
                HasPreviousPage = invites.HasPreviousPage,
                NextPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAll))).ToString(),
                PreviousPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAll))).ToString()
            };

            var response = new ApiResponse<IEnumerable<InviteDtos>>(invitesDto)
            {
                Meta = metadata
            };

            return Ok(response);
        }


        /// <summary>
        /// Retrieve invite information.
        /// </summary>
        /// <param name="id">Invite id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var invite = await _inviteService.GetInvite(id);
            var inviteDto = _mapper.Map<InviteDtos>(invite);
            var response = new ApiResponse<InviteDtos>(inviteDto);
            return Ok(response);
        }

        /// <summary>
        /// Send invite.
        /// </summary>
        /// <param name="inviteDto">Invite information</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SendInvite(InviteDtos inviteDto)
        {
            //Not able to send invite. Followed by errors.
            var invite = _mapper.Map<Invite>(inviteDto);

            await _inviteService.InsertInvite(invite, User);
            //SEND EMAIL
            inviteDto = _mapper.Map<InviteDtos>(invite);

            var response = new ApiResponse<InviteDtos>(inviteDto);
            return Ok(response);
        }

        /// <summary>
        /// Cancel invite.
        /// </summary>
        /// <param name="id">Invite id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _inviteService.DeleteInvite(id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }
    }
}
