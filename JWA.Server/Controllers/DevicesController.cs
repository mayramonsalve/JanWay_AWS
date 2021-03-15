using JWA.Api.Response;
using JWA.Core.DTOs;
using JWA.Infrastructure.Interfaces;
using JWA.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Linq;
using JWA.Core.Interfaces;
using JWA.Core.Entities;
using System.Threading.Tasks;

namespace JWA.Api.Controllers
{
    //[Authorize(Roles = "SystemAdministrator"]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class deviceController : ControllerBase
    {
        private readonly IUnitService _unitService;
        private readonly IFlushService _flushService;
        private readonly ISystemStatusService _systemStatusService;
        /*private readonly IMapper _mapper;*/
        private readonly IPasswordService _passwordService;
        public readonly BaseUrlOptions _urlOptions;
        public deviceController(IUnitService unitService, IFlushService flushService, ISystemStatusService systemStatusService,
                                IPasswordService passwordService, IOptions<BaseUrlOptions> urlOptions)//IMapper mapper)
        {
            /*_mapper = mapper;*/
            _unitService = unitService;
            _flushService = flushService;
            _systemStatusService = systemStatusService;
            _passwordService = passwordService;
            _urlOptions = urlOptions.Value;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> register(RegisterDto registerDto)
        {
            try
            {
                string message;
                if (registerDto != null && !String.IsNullOrEmpty(registerDto.device_id))
                {
                    Unit unitExists = await _unitService.GetByMacAddress(registerDto.device_id);

                    if (unitExists == null)
                    {
                        Unit unit = new Unit();
                        unit.MacAddress = registerDto.device_id;
                        var registered = _unitService.InsertUnit(unit);

                        message = "REGISTRATION_SUCCESS";
                        var response = new DeviceApiResponse<bool>(message);
                        return Ok(response);
                    }
                    else
                    {
                        message = "DEVICE_DUPLICATED";
                        var response = new DeviceApiResponse<bool>(message);
                        return BadRequest(response);
                    }
                }
                else
                {
                    message = "NULL PARAMETER";
                    var response = new DeviceApiResponse<bool>(message);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                string message = "ERROR";
                var response = new DeviceApiResponse<Exception>(ex, message);
                return StatusCode(500, response);
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult request_access(string encodedDeviceId)
        {
            try
            {
                string message;
                string decodedDeviceId = "";
                if (!String.IsNullOrEmpty(encodedDeviceId))
                {
                    try
                    {
                        byte[] deviceIdBytes = Convert.FromBase64String(encodedDeviceId);
                        decodedDeviceId = Encoding.UTF8.GetString(deviceIdBytes);
                    }
                    catch(Exception) //CREATE CUSTOM EXCEPTION FOR INVALID ENCODED DEVICE ID
                    {
                        message = "INVALID_DEVICEID";
                        var _response = new DeviceApiResponse<bool>(message);
                        return BadRequest(_response);
                    }

                    message = "REQUEST_ACCESS_SUCCESS";
                    var claimsIdentity = new ClaimsIdentity(new List<Claim>()
                                {
                                   new Claim(ClaimTypes.NameIdentifier, decodedDeviceId)
                                 }, "Identity.Application");

                    var token = _passwordService.CreateAccessToken(_passwordService.CreateJwtClaims(claimsIdentity));
                    var response = new DeviceApiResponse<string>(token, message);
                    return Ok(response);
                }
                else
                {
                    message = "NULL_PARAMETER";
                    var response = new DeviceApiResponse<bool>(message);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                string message = "ERROR";
                var response = new DeviceApiResponse<Exception>(ex, message);
                return StatusCode(500, response);
            }
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> system_status(SystemStatusDto systemStatusDto)
        {
            try
            { //HANDLE CUSTOM EXCEPTIONS
                string message;
                var token = Request.Headers["Authorization"].ToString();
                if (!String.IsNullOrEmpty(token))
                {
                    var TokenData = _passwordService.ReadToken(token);

                    if (systemStatusDto != null)
                    {
                        string macAddress = TokenData.Claims.FirstOrDefault().Value;

                        Unit unit = await _unitService.GetByMacAddress(macAddress);

                        if (unit != null)
                        { //SEPARATE SYSTEM_STATUS AND FLUSH DATA //USE MAP<> //BATTERY CAN BE FLOAT //VALIDATE FORMATS
                            SystemStatus systemStatus = new SystemStatus();
                            systemStatus.UnitId = unit.Id;
                            systemStatus.SelenoidTemperature = double.Parse(systemStatusDto.case_temperature.Replace("C",""));
                            systemStatus.BatteryLevel = int.Parse(systemStatusDto.battery.Replace("%",""));
                            systemStatus.Date = UnixTimeStampToDateTime(double.Parse(systemStatusDto.unix_timestamp));

                            await _systemStatusService.InsertSystemStatus(systemStatus);

                            message = "SUCCESS";
                            var response = new DeviceApiResponse<bool>(message);
                            return Ok(response);
                        }
                        else
                        {
                            message = "DEVICE_NOT_FOUND";
                            var response = new DeviceApiResponse<bool>(message);
                            return NotFound(response);
                        }
                    }
                    else
                    {
                        message = "NULL_PARAMETER";
                        var response = new DeviceApiResponse<bool>(message);
                        return BadRequest(response);
                    }
                }
                else
                {
                    message = "ACCESS_DENIED";
                    var response = new DeviceApiResponse<bool>(message);
                    return Unauthorized(response);
                }
            }
            catch(Exception ex)
            {
                string message;
                if (ex.Source == "System.IdentityModel.Tokens.Jwt")
                    message = "INVALID_TOKEN";
                else
                    message = "ERROR";
                var response = new DeviceApiResponse<Exception>(ex, message);
                return StatusCode(500, response);
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        { //SET TIMEZONE
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> flush(FlushDto flushDto)
        {
            try
            { //HANDLE CUSTOM EXCEPTIONS //VALIDATE FORMATS
                string message;
                var token = Request.Headers["Authorization"].ToString();
                if (!String.IsNullOrEmpty(token))
                {
                    var TokenData = _passwordService.ReadToken(token);

                    if (flushDto != null)
                    {
                        string macAddress = TokenData.Claims.FirstOrDefault().Value;

                        Unit unit = await _unitService.GetByMacAddress(macAddress);

                        if (unit != null)
                        { //USE MAP<> //BATTERY CAN BE FLOAT
                            Flush flush = new Flush();
                            flush.UnitId = unit.Id;
                            flush.Date = UnixTimeStampToDateTime(double.Parse(flushDto.unix_timestamp));

                            filters_pressure pressure = flushDto.filters_pressure;
                            flush.Filter1 = double.Parse(pressure.f1.Replace("PSI", ""));
                            flush.Filter2 = double.Parse(pressure.f2.Replace("PSI", ""));
                            flush.Filter3 = double.Parse(pressure.f3.Replace("PSI", ""));
                            flush.Filter4 = double.Parse(pressure.f4.Replace("PSI", ""));

                            await _flushService.InsertFlush(flush);

                            message = "SUCCESS";
                            var response = new DeviceApiResponse<bool>(message);
                            return Ok(response);
                        }
                        else
                        {
                            message = "DEVICE_NOT_FOUND";
                            var response = new DeviceApiResponse<bool>(message);
                            return NotFound(response);
                        }
                    }
                    else
                    {
                        message = "NULL_PARAMETER";
                        var response = new DeviceApiResponse<bool>(message);
                        return BadRequest(response);
                    }
                }
                else
                {
                    message = "ACCESS_DENIED";
                    var response = new DeviceApiResponse<bool>(message);
                    return Unauthorized(response);
                }
            }
            catch (Exception ex)
            {
                string message;
                if (ex.Source == "System.IdentityModel.Tokens.Jwt")
                    message = "INVALID_TOKEN";
                else
                    message = "ERROR";
                var response = new DeviceApiResponse<Exception>(ex, message);
                return StatusCode(500, response);
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult credentials_rejected()
        {
            try
            {
                string message = "SUCCESS";
                var response = new DeviceApiResponse<bool>(message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string message = "ERROR";
                var response = new DeviceApiResponse<Exception>(ex, message);
                return StatusCode(500, response);
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult credentials_digested()
        {
            try
            {
                string message = "SUCCESS";
                var response = new DeviceApiResponse<bool>(message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string message = "ERROR";
                var response = new DeviceApiResponse<Exception>(ex, message);
                return StatusCode(500, response);
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult ota_json()
        {
            try
            { //update version
                string message = "SUCCESS";
                OtaJsonDto info = new OtaJsonDto()
                {
                    version = 2.2F,
                    file = _urlOptions.GetBaseUrl() + "/api/device/ota_update",
                    //file = _urlOptions.BaseUrl + "/api/device/ota_update"
                };
                var response = new DeviceApiResponse<OtaJsonDto>(info, message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string message = "ERROR";
                var response = new DeviceApiResponse<Exception>(ex, message);
                return StatusCode(500, response);
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult ota_update()
        {
            try
            {
                string link = Path.Combine("Files", "esp32.bin");
                var net = new System.Net.WebClient();
                var data = net.DownloadData(link);
                var content = new MemoryStream(data);
                var contentType = "APPLICATION/octet-stream";
                return File(content, contentType);
            }
            catch(Exception ex)
            {
                string message = "ERROR";
                var response = new DeviceApiResponse<Exception>(ex, message);
                return StatusCode(500, response);
            }
        }
    }
}
