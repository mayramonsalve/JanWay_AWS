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
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IAddressService _addressService;
        private readonly IFacilityService _facilityService;
        private readonly IInviteService _inviteService;
        private readonly IUnitService _unitService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IPasswordService _passwordService;
        public OrganizationsController(IOrganizationService organizationService, IAddressService addressService,
                                        IFacilityService facilityService, IInviteService inviteService, IUnitService unitService,
                                        IMapper mapper, IUriService uriService, IPasswordService passwordService)
        {
            _organizationService = organizationService;
            _addressService = addressService;
            _facilityService = facilityService;
            _inviteService = inviteService;
            _unitService = unitService;
            _mapper = mapper;
            _uriService = uriService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Setup an organization.
        /// </summary>
        /// <param name="setupOrganizationDto">Organization information</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SetupOrganization(SetupOrganizationDto setupOrganizationDto)
        {
            //#MISSING: ADD ORGANIZATIONID TO ORGANIZATION DEPENDING ON USER LOGGED
            //ASSIGN DATA AND THEN VALIDATE THE WHOLE OBJECT
            try
            {
                string message;
                bool organizationExists = await _organizationService.ExistsOrganization(setupOrganizationDto.Organization.Name);
                if (!organizationExists)
                {
                    var address = _mapper.Map<Address>(setupOrganizationDto.Address);
                    var addressId = await _addressService.InsertAddress(address);

                    var organization = _mapper.Map<Organization>(setupOrganizationDto.Organization);
                    organization.AddressId = addressId;
                    var organizationId = await _organizationService.InsertOrganization(organization);

                    var facilities = _mapper.Map<List<Facility>>(setupOrganizationDto.Facilities);
                    facilities.ForEach(e => e.OrganizationId = organizationId);
                    var facilitiesIdDic = await _facilityService.InsertFacilitiesRange(facilities);

                    var invites = _mapper.Map<List<Invite>>(setupOrganizationDto.Users);
                    invites.ForEach(e => e.OrganizationId = organizationId);
                    await _inviteService.InsertInvitesRange(invites);

                    setupOrganizationDto.Units.ForEach(e => e.FacilityId = facilitiesIdDic[e.Facility]);
                    var units = _mapper.Map<List<Unit>>(setupOrganizationDto.Units);
                    await _unitService.InsertUnitsRange(units);

                    message = "The organization has been registered.";
                    var response = new DeviceApiResponse<bool>(message);
                    return Ok(response);
                }
                else
                {
                    message = "The Organization name already exists in the Organization.";
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
        /// Update organization information.
        /// </summary>
        /// <param name="editOrganizationDto">Organization information</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> EditOrganization(EditOrganizationDto editOrganizationDto)
        {
            var organization = _mapper.Map<Organization>(editOrganizationDto.Organization);
            var address = _mapper.Map<Address>(editOrganizationDto.Address);
            //#MISSING: VALIDATE THAT USER LOGGED BELONGS TO ORGANIZATION
            var result = await _organizationService.UpdateOrganization(organization, address);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Delete organization from organization.
        /// </summary>
        /// <param name="deleteOrganizationDto">Organization id</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> DeleteOrganization(DeleteOrganizationDto deleteOrganizationDto)
        {
            //#MISSING: VALIDATE THAT USSER LOGGED BELONGS TO USER ORGANIZATION
            var result = await _organizationService.DeleteOrganization(deleteOrganizationDto.Id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Deactivate organization.
        /// </summary>
        /// <param name="deactivateOrganizationDto">Organization id</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> DeactivateOrganization(DeactivateOrganizationDto deactivateOrganizationDto)
        {
            //#MISSING: VALIDATE THAT USSER LOGGED BELONGS TO USER ORGANIZATION
            var result = await _organizationService.DeactivateOrganization(deactivateOrganizationDto.Id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Activate organization.
        /// </summary>
        /// <param name="activateOrganizationDto">Organization id</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> ActivateOrganization(ActivateOrganizationDto activateOrganizationDto)
        {
            //#MISSING: VALIDATE THAT USSER LOGGED BELONGS TO USER ORGANIZATION
            var result = await _organizationService.ActivateOrganization(activateOrganizationDto.Id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Retrieve all organizations.
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<OrganizationsListDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetAll([FromQuery] OrganizationQueryFilter filters)
        {
            // GET ORGANIZATIONS DEPENDING ON USER LOGGED
            var organizations = _organizationService.GetOrganizations(filters);
            var organizationsDto = _mapper.Map<IEnumerable<OrganizationsListDto>>(organizations);

            var metadata = new Metadata
            {
                TotalCount = organizations.TotalCount,
                PageSize = organizations.PageSize,
                CurrentPage = organizations.CurrentPage,
                TotalPages = organizations.TotalPages,
                HasNextPage = organizations.HasNextPage,
                HasPreviousPage = organizations.HasPreviousPage,
                NextPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAll))).ToString(),
                PreviousPageUrl = _uriService.GetPaginationUri(Url.RouteUrl(nameof(GetAll))).ToString()
            };

            var response = new ApiResponse<IEnumerable<OrganizationsListDto>>(organizationsDto)
            {
                Meta = metadata
            };

            return Ok(response);
        }

        /// <summary>
        /// Retrieve organization profile information.
        /// </summary>
        /// <param name="id">Organization id</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetOrganizationProfile(int id)
        {
            //#MISSING: VALIDATE THAT USER LOGGED BELONGS TO ORGANIZATION
            var organization = await _organizationService.GetOrganization(id);

            var organizationInfoDto = _mapper.Map<OrganizationInfoDto>(organization);
            var addressInfoDto = _mapper.Map<AddressInfoDto>(organization.Address);
            
            var organizationProfile = _mapper.Map<OrganizationProfileDto>(organization);
            organizationProfile.Organization = organizationInfoDto;
            organizationProfile.Address = addressInfoDto;
            
            var response = new ApiResponse<OrganizationProfileDto>(organizationProfile);
            return Ok(response);
        }

    }
}
