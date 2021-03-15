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
    public class FacilitiesController : ControllerBase
    {
        private readonly IFacilityService _facilityService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IPasswordService _passwordService;
        public FacilitiesController(IFacilityService facilityService, IMapper mapper, IUriService uriService, IPasswordService passwordService)
        {
            _facilityService = facilityService;
            _mapper = mapper;
            _uriService = uriService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Add a facility.
        /// </summary>
        /// <param name="addFacilityDto">Facility information</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AddFacility(AddFacilityDto addFacilityDto)
        {
            //#MISSING: ADD ORGANIZATIONID TO FACILITY DEPENDING ON USER LOGGED
            try
            {
                string message;
                bool facilityExists = await _facilityService.ExistsFacility(addFacilityDto.Facility.Name, addFacilityDto.Facility.OrganizationId);
                if (!facilityExists)
                {
                    var address = _mapper.Map<Address>(addFacilityDto);
                    var facility = _mapper.Map<Facility>(addFacilityDto);
                    var invites = _mapper.Map<List<Invite>>(addFacilityDto);
                    var units = _mapper.Map<List<Unit>>(addFacilityDto);
                    var registered = _facilityService.AddFacility(address, facility, invites, units);

                    message = "The facility has been registered.";
                    var response = new DeviceApiResponse<bool>(message);
                    return Ok(response);
                }
                else
                {
                    message = "The Facility name already exists in the Organization.";
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
        /// Update facility information.
        /// </summary>
        /// <param name="editFacilityDto">Facility information</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> EditFacility(EditFacilityDto editFacilityDto)
        {
            var facility = _mapper.Map<Facility>(editFacilityDto.Facility);
            var address = _mapper.Map<Address>(editFacilityDto.Address);
            //#MISSING: VALIDATE THAT USER LOGGED BELONGS TO FACILITY/ORGANIZATION
            var result = await _facilityService.UpdateFacility(facility, address);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Delete facility from organization.
        /// </summary>
        /// <param name="deleteFacilityDto">Facility id</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> DeleteFacility(DeleteFacilityDto deleteFacilityDto)
        {
            //#MISSING: VALIDATE THAT USSER LOGGED BELONGS TO USER FACILITY/ORGANIZATION
            var result = await _facilityService.DeleteFacility(deleteFacilityDto.Id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Deactivate facility.
        /// </summary>
        /// <param name="deactivateFacilityDto">Facility id</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> DeactivateFacility(DeactivateFacilityDto deactivateFacilityDto)
        {
            //#MISSING: VALIDATE THAT USSER LOGGED BELONGS TO USER FACILITY/ORGANIZATION
            var result = await _facilityService.DeactivateFacility(deactivateFacilityDto.Id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Activate facility.
        /// </summary>
        /// <param name="activateFacilityDto">Facility id</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> ActivateFacility(ActivateFacilityDto activateFacilityDto)
        {
            //#MISSING: VALIDATE THAT USSER LOGGED BELONGS TO USER FACILITY/ORGANIZATION
            var result = await _facilityService.ActivateFacility(activateFacilityDto.Id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Retrieve all facilities.
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<FacilitiesListDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] FacilityQueryFilter filters)
        { 
            // GET FACILITIES DEPENDING ON USER LOGGED
            var facilities = await _facilityService.GetFacilities(filters);
            var facilitiesDto = _mapper.Map<IEnumerable<FacilitiesListDto>>(facilities);

            var metadata = new Metadata
            {
                TotalCount = facilities.TotalCount,
                PageSize = facilities.PageSize,
                CurrentPage = facilities.CurrentPage,
                TotalPages = facilities.TotalPages,
                HasNextPage = facilities.HasNextPage,
                HasPreviousPage = facilities.HasPreviousPage,
                NextPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAll))).ToString(),
                PreviousPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAll))).ToString()
            };

            var response = new ApiResponse<IEnumerable<FacilitiesListDto>>(facilitiesDto)
            {
                Meta = metadata
            };

            return Ok(response);
        }

        /// <summary>
        /// Retrieve facility profile information.
        /// </summary>
        /// <param name="id">Facility id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacilityProfile(int id)
        {
            //#MISSING: VALIDATE THAT USER LOGGED BELONGS TO FACILITY
            var facility = await _facilityService.GetFacility(id);
            var facilityInfoDto = _mapper.Map<FacilityInfoDto>(facility);
            var addressInfoDto = _mapper.Map<AddressInfoDto>(facility.Address);
            var facilityProfile = _mapper.Map<FacilityProfileDto>(facility);
            facilityProfile.Facility = facilityInfoDto;
            facilityProfile.Address = addressInfoDto;
            var response = new ApiResponse<FacilityProfileDto>(facilityProfile);
            return Ok(response);
        }

    }
}
