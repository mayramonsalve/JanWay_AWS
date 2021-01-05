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
using System.Net;
using System.Threading.Tasks;

namespace JWA.Api.Controllers
{
    //[Authorize(Roles = "SystemAdministrator"]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisorsController : ControllerBase
    {
        private readonly ISupervisorService _supervisorService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IPasswordService _passwordService;
        public SupervisorsController(ISupervisorService supervisorService, IMapper mapper, IUriService uriService, IPasswordService passwordService)
        {
            _supervisorService = supervisorService;
            _mapper = mapper;
            _uriService = uriService;
            _passwordService = passwordService;
        }

        ///// <summary>
        ///// Retrieve all supervisors depending on supervisor role and organization.
        ///// </summary>
        ///// <param name="filters">Filters to apply</param>
        ///// <returns></returns>
        //[HttpGet(Name = "[controller][action]")]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<SupervisorDto>>))]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //public IActionResult GetAll([FromQuery]SupervisorQueryFilter filters)
        //{
        //    var supervisors = _supervisorService.GetSupervisors(filters);
        //    var supervisorsDto = _mapper.Map<IEnumerable<SupervisorDto>>(supervisors);

        //    var metadata = new Metadata
        //    {
        //        TotalCount = supervisors.TotalCount,
        //        PageSize = supervisors.PageSize,
        //        CurrentPage = supervisors.CurrentPage,
        //        TotalPages = supervisors.TotalPages,
        //        HasNextPage = supervisors.HasNextPage,
        //        HasPreviousPage = supervisors.HasPreviousPage,
        //        NextPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAll))).ToString(),
        //        PreviousPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAll))).ToString()
        //    };

        //    var response = new ApiResponse<IEnumerable<SupervisorDto>>(supervisorsDto)
        //    {
        //        Meta = metadata
        //    };

        //    return Ok(response);
        //}

        ///// <summary>
        ///// Retrieve supervisor information.
        ///// </summary>
        ///// <param name="id">Supervisor id</param>
        ///// <returns></returns>
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    var supervisor = await _supervisorService.GetSupervisor(id);
        //    var supervisorDto = _mapper.Map<SupervisorDto>(supervisor);
        //    var response = new ApiResponse<SupervisorDto>(supervisorDto);
        //    return Ok(response);
        //}

        /// <summary>
        /// Relocate supervisor to another facility.
        /// </summary>
        /// <param name="relocateDto">Relocation information</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Relocate(RelocateDto relocateDto)
        {
            var supervisor = _mapper.Map<Supervisor>(relocateDto);

            var result = await _supervisorService.UpdateSupervisor(supervisor);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        [HttpGet]
        public List<string> GetTest()
        {
            return new List<string>() {
            "Nancy Davolio",
            "Andrew Fuller",
            "Janet Leverling"
        };
        }
    }
}
