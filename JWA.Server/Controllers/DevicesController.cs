using JWA.Api.Response;
using JWA.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JWA.Api.Controllers
{
    //[Authorize(Roles = "SystemAdministrator"]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class deviceController : ControllerBase
    {
        /*private readonly ISupervisorService _supervisorService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IPasswordService _passwordService;*/
        public deviceController()//ISupervisorService supervisorService, IMapper mapper, IUriService uriService, IPasswordService passwordService)
        {
            /*_supervisorService = supervisorService;
            _mapper = mapper;
            _uriService = uriService;
            _passwordService = passwordService;*/
        }


        [Route("[action]")]
        [HttpPost]
        public IActionResult register(RegisterDto registerDto)
        {
            if (String.IsNullOrEmpty(registerDto.device_id))
                return BadRequest();

            var response = new ApiResponse<bool>(true);
            return Ok(response);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult request_access(string encodedDeviceId) //encoded_base64_device_id
        {
            string jwt_token = "testJWTtoken";
            var response = new ApiResponse<string>(jwt_token);
            return Ok(response);
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult system_status(SystemStatusDto systemStatusDto)
        {
            var response = new ApiResponse<bool>(true);
            return Ok(response);
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult flush(FlushDto flushDto)
        {
            var response = new ApiResponse<bool>(true);
            return Ok(response);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult credentials_rejected()
        {
            var response = new ApiResponse<bool>(false);
            return Ok(response);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult credentials_digested()
        {
            var response = new ApiResponse<bool>(true);
            return Ok(response);
        }
    }
}
