using Microsoft.AspNetCore.Mvc;
using PatientManagementApi.Dtos.Auth;
using System.Security.Authentication;

namespace PatientManagementApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;

        private ResponseDto _response;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new ResponseDto();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto request)
        {
                var loginResponse = await _authService.Login(request);        
                _response.Result = loginResponse;
                return Ok(_response);

        }
    }
}
