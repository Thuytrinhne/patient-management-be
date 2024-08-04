using Microsoft.AspNetCore.Authorization;

namespace PatientManagementApi.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    [Authorize]
    public class StatisticsAPIController : ControllerBase
    {
        private ResponseDto _response;

        private readonly IPatientService _patientService;
        public StatisticsAPIController( IPatientService patientService)
        {
  
            _response = new();
            _patientService = patientService;

        }
        [HttpGet("patients/total")]
        public async Task<ActionResult<ResponseDto>> GetPatientStatistics()
        {
            var result = await _patientService.GetPatientsStatistic();           
            _response.Result = result;
            return Ok(_response);
        }
        [HttpGet("patients/today")]
        public async Task<ActionResult<ResponseDto>> GetTodayPatientStatistics()
        {
            var result = await _patientService.GetTodayPatientsStatistic();
            _response.Result = result;
            return Ok(_response);
        }
    }
}
