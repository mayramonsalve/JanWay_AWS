using AutoMapper;
using JWA.Api.Response;
using JWA.Core.CustomEntities;
using JWA.Core.DTOs;
using JWA.Core.Entities;
using JWA.Core.Interfaces;
using JWA.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWA.Api.Controllers
{
    //[AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ISupervisorService _supervisorService;
        private readonly IPasswordService _passwordService;

        public AccountController(IConfiguration configuration, IMapper mapper, IUserService userService, ISupervisorService supervisorService, IPasswordService passwordService)
        {
            _configuration = configuration;
            _userService = userService;
            _mapper = mapper;
            _supervisorService = supervisorService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Sign in to the system.
        /// </summary>
        /// <param name="signIn">Credentials information</param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> SignIn(SignIn signIn)
        //{
        //    var validation = await IsValidUser(signIn);
        //    if (validation.Item1)
        //    {
        //        string token = await GenerateToken(validation.Item2);
        //        return Ok(new { token });
        //    }
        //    return NotFound();
        //}


        /// <summary>
        /// Confirm user account.
        /// </summary>
        /// <param name="request">User account information</param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> ConfirmAccount(ConfirmAccountRequest request)
        //{
        //    var user = _mapper.Map<User>(request);
        //    //user.Password = _passwordService.Hash(request.Password);

        //    var id = await _userService.InsertUser(user);

        //    if (request.OrganizationId.HasValue)
        //    {
        //        var supervisor = _mapper.Map<Supervisor>(request);
        //        supervisor.UserId = id;
        //        await _supervisorService.InsertSupervisor(supervisor);
        //    }

        //    var result = _mapper.Map<ConfirmAccountResponse>(request);

        //    var response = new ApiResponse<ConfirmAccountResponse>(result);
        //    return Ok(response);
        //}

        //private async Task<(bool, User)> IsValidUser(SignIn signIn)
        //{
        //    var user = await _userService.GetUserByCredentials(signIn);

        //    var isValid = true;// _passwordService.Check(user.Password, signIn.Password);

        //    return (isValid, user);
        //}

        //private async Task<string> GenerateToken(User user)
        //{
        //    //Header
        //    var _symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
        //    var signingCredentials = new SigningCredentials(_symetricSecurityKey, SecurityAlgorithms.Sha256);
        //    var header = new JwtHeader(signingCredentials);

        //    //Claims
        //    var claims = new[]
        //    {
        //        //new Claim(ClaimTypes.Name, user.Name),
        //        new Claim(ClaimTypes.Email, user.Email),
        //        //new Claim(ClaimTypes.Role, user.RoleId.ToString()),
        //        new Claim("Organization", await GetOrganization(user))
        //    };

        //    //Payload
        //    var payload = new JwtPayload
        //    (
        //        _configuration["Authentication:Issuer"],
        //        _configuration["Authentication:Audience"],
        //        claims,
        //        DateTime.Now,
        //        DateTime.UtcNow.AddHours(8)
        //    );

        //    var token = new JwtSecurityToken(header, payload);
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        //private async Task<string> GetOrganization(User user) {
        //    if(user.Supervisors == null)
        //        return "";
        //    else
        //    {
        //        var supervisor = await _supervisorService.GetSupervisorByUser(user.Id);
        //        return supervisor.OrganizationId.ToString();
        //    }
        //}

    }
}
