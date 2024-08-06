using Microsoft.AspNetCore.Authorization;
using PatientManagementApi.Models;

namespace PatientManagementApi.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    [Authorize]
    public class StatisticsAPIController (IPatientService _patientService) : ControllerBase
    {
        [HttpGet("patients/total")]
        public async Task<ActionResult<ResponseDto>> GetPatientStatistics()
        {
            var result = await _patientService.GetPatientsStatistic();
            return Ok(new ResponseDto { Result = result });

        }
        [HttpGet("patients/today")]
        public async Task<ActionResult<ResponseDto>> GetTodayPatientStatistics()
        {
            var result = await _patientService.GetTodayPatientsStatistic();
            return Ok(new ResponseDto { Result = result });

        }
    }
}
