using AutoMapper;
using JWA.Api.Response;
using JWA.Core.CustomEntities;
using JWA.Core.DTOs;
using JWA.Core.Entities;
using JWA.Core.Interfaces;
using JWA.Core.QueryFilters;
using JWA.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace JWA.Api.Controllers
{
    //[Authorize(Roles = "System Administrator, Support Manager, Customer Support, Organization Administrator, Organization Manager, Facility Administrator")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISupervisorService _supervisorService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, IMapper mapper, ISupervisorService supervisorService,
                                IUriService uriService, IPasswordService passwordService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _supervisorService = supervisorService;
            _mapper = mapper;
            _uriService = uriService;
            _passwordService = passwordService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve all users depending on user role and organization.
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [HttpGet(Name = "[controller][action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery]UserQueryFilter filters)
        {
            try
            {
                _logger.LogInformation("-------GETALLASYNC-------");

                var users = _userService.GetUsers(filters);
                var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
                usersDto = await _supervisorService.AddSupervisorData(usersDto);

                var metadata = new Metadata
                {
                    TotalCount = users.TotalCount,
                    PageSize = users.PageSize,
                    CurrentPage = users.CurrentPage,
                    TotalPages = users.TotalPages,
                    HasNextPage = users.HasNextPage,
                    HasPreviousPage = users.HasPreviousPage,
                    NextPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAllAsync))).ToString(),
                    PreviousPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAllAsync))).ToString()
                };

                var response = new ApiResponse<IEnumerable<UserDto>>(usersDto)
                {
                    Meta = metadata
                };

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Exception Caught");
                return BadRequest();
            }
        }

        /// <summary>
        /// Retrieve user information.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userService.GetUser(id);
            var userDto = _mapper.Map<UserDto>(user);
            var response = new ApiResponse<UserDto>(userDto);
            return Ok(response);
        }

        /// <summary>
        /// Edit user profile.
        /// </summary>
        /// <param name="profileDto">User profile information</param>
        /// <returns></returns>
        [Route("EditProfile")]
        [HttpPut]
        public async Task<IActionResult> EditProfile(ProfileDto profileDto)
        {
            var user = _mapper.Map<User>(profileDto);
            
            var result = await _userService.UpdateUser(user);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Update role.
        /// </summary>
        /// <param name="updateRoleDto">New role information</param>
        /// <returns></returns>
        [Route("UpdateRole")]
        [HttpPut]
        public async Task<IActionResult> UpdateRole(UpdateRoleDto updateRoleDto)
        {
            var user = _mapper.Map<User>(updateRoleDto);

            var result = await _userService.UpdateRole(user);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Update role.
        /// </summary>
        /// <param name="updatePasswordDto">New password information</param>
        /// <returns></returns>
        [Route("UpdatePassword")]
        [HttpPut]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var user = _mapper.Map<User>(updatePasswordDto);
            user.PasswordHash = _passwordService.Hash(updatePasswordDto.Password);

            var result = await _userService.UpdatePassword(user);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Delete user account.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteUser(id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

    }
}
