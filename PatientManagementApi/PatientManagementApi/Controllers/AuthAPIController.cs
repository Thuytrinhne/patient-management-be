using Microsoft.AspNetCore.Mvc;
using PatientManagementApi.Dtos.Auth;
using System.Security.Authentication;

namespace PatientManagementApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController (IAuthService _authService) : ControllerBase
    {
      
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto request)
        {
                var loginResponse = await _authService.Login(request);
                return Ok(new ResponseDto { Result = loginResponse });

         

        }
    }
}
