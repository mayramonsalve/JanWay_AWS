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
using System;
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
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IPasswordService _passwordService;
        public UnitsController(IUnitService unitService, IMapper mapper, IUriService uriService, IPasswordService passwordService)
        {
            _unitService = unitService;
            _mapper = mapper;
            _uriService = uriService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Register a unit.
        /// </summary>
        /// <param name="registerUnitDto">Unit's MacAddress</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUnitDto registerUnitDto)
        {
            try
            {
                string message;
                Unit unitExists = await _unitService.GetByMacAddress(registerUnitDto.MacAddress);
                if (unitExists == null)
                {
                    var unit = _mapper.Map<Unit>(registerUnitDto);
                    var registered = _unitService.InsertUnit(unit);

                    message = "The unit has been registered.";
                    var response = new DeviceApiResponse<bool>(message);
                    return Ok(response);
                }
                else
                {
                    message = "The Mac Address already exists in the database.";
                    var response = new DeviceApiResponse<bool>(message);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                string message = "ERROR";
                var response = new ApiResponse<Exception>(ex, message);
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Assign unit to a facility.
        /// </summary>
        /// <param name="assignUnitDto">Unit information</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AssignUnit(AssignUnitDto assignUnitDto)
        {
            //#MISSING: ASSIGN FACILITY DEPENDING ON USER LOGGED
            //VALIDATE THAT UNIT BELONGS TO USER LOGGED'S FACILITY / ORGANIZATION
            var unit = _mapper.Map<Unit>(assignUnitDto);
            var result = await _unitService.UpdateUnit(unit);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Update unit's name and/or facility.
        /// </summary>
        /// <param name="editUnitDto">Unit information</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> EditUnit(EditUnitDto editUnitDto)
        {
            var unit = _mapper.Map<Unit>(editUnitDto);
            //#MISSING: VALIDATE THAT UNIT BELONGS TO USER LOOGGED'S FACILITY/ORGANIZATION
            var result = await _unitService.UpdateUnit(unit);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Detach unit from facility.
        /// </summary>
        /// <param name="detachUnitDto">Unit id</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> DetachUnit(DetachUnitDto detachUnitDto)
        {
            //#MISSING: VALIDATE THAT UNIT BELONGS TO USER LOGGED'S FACILITY/ORGANIZATION
            var result = await _unitService.DeleteUnit(detachUnitDto.Id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Reboot unit.
        /// </summary>
        /// <param name="rebootUnitDto">Unit id</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> RebootUnit(RebootUnitDto rebootUnitDto)
        {
            var result = await _unitService.DeleteUnit(rebootUnitDto.Id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Retrieve all units.
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UnitsHubDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetAll([FromQuery] UnitQueryFilter filters)
        {
            var units = _unitService.GetUnits(filters);
            var unitsDto = _mapper.Map<IEnumerable<UnitsHubDto>>(units);

            var metadata = new Metadata
            {
                TotalCount = units.TotalCount,
                PageSize = units.PageSize,
                CurrentPage = units.CurrentPage,
                TotalPages = units.TotalPages,
                HasNextPage = units.HasNextPage,
                HasPreviousPage = units.HasPreviousPage,
                NextPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAll))).ToString(),
                PreviousPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAll))).ToString()
            };

            var response = new ApiResponse<IEnumerable<UnitsHubDto>>(unitsDto)
            {
                Meta = metadata
            };

            return Ok(response);
        }

        /// <summary>
        /// Retrieve unit profile information.
        /// </summary>
        /// <param name="id">Unit id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitProfile(int id)
        {
            //#MISSING: VALIDATE THAT UNIT BELONGS TO USER LOGGED'S FACILITY/ORGANIZATION
            var unitProfile = await _unitService.GetUnitProfile(id);
            var response = new ApiResponse<UnitsProfileDto>(unitProfile);
            return Ok(response);
        }

        /// <summary>
        /// Retrieve flushes history.
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<FlushesHistoryDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetFlushesHistory([FromQuery] FlushQueryFilter filters)
        {
            var flushesHistory = await _unitService.GetFlushesHistory(filters);
            var flushesHistoryDto = _mapper.Map<IEnumerable<FlushesHistoryDto>>(flushesHistory);

            var metadata = new Metadata
            {
                TotalCount = flushesHistory.TotalCount,
                PageSize = flushesHistory.PageSize,
                CurrentPage = flushesHistory.CurrentPage,
                TotalPages = flushesHistory.TotalPages,
                HasNextPage = flushesHistory.HasNextPage,
                HasPreviousPage = flushesHistory.HasPreviousPage,
                NextPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetFlushesHistory))).ToString(),
                PreviousPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetFlushesHistory))).ToString()
            };

            var response = new ApiResponse<IEnumerable<FlushesHistoryDto>>(flushesHistoryDto)
            {
                Meta = metadata
            };

            return Ok(response);
        }

        /// <summary>
        /// Retrieve all unassigned units.
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [Route("[action]")]
        //[HttpGet(Name = "[controller][action]")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UnassignedUnitsDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllUnassigned([FromQuery] BaseQueryFilter filters)
        {
            var unassignedUnits = await _unitService.GetUnassignedUnits(filters);
            var unassignedUnitsDto = _mapper.Map<IEnumerable<UnassignedUnitsDto>>(unassignedUnits);

            var metadata = new Metadata
            {
                TotalCount = unassignedUnits.TotalCount,
                PageSize = unassignedUnits.PageSize,
                CurrentPage = unassignedUnits.CurrentPage,
                TotalPages = unassignedUnits.TotalPages,
                HasNextPage = unassignedUnits.HasNextPage,
                HasPreviousPage = unassignedUnits.HasPreviousPage,
                NextPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAllUnassigned))).ToString(),
                PreviousPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAllUnassigned))).ToString()
            };

            var response = new ApiResponse<IEnumerable<UnassignedUnitsDto>>(unassignedUnitsDto)
            {
                Meta = metadata
            };

            return Ok(response);
        }
    }
}
