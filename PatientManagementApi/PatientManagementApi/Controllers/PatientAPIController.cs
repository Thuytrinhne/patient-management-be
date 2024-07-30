
namespace PatientManagementApi.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientAPIController : ControllerBase
    { 
        private ResponseDto _response;
        private IMapper _mapper;
        private IPatientService _patientService;

        public PatientAPIController(IMapper mapper, IPatientService catalogService)
        {
            _mapper = mapper;
            _response = new();
            _patientService = catalogService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get
            ([FromQuery] PaginationRequest request)
        {
            try
            {
                PaginationResult<Patient> obj = await _patientService.GetAllPatientAsync(request);
                if (obj is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "There was not any patient record in system !";

                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<PaginationResult<GetPatientsResponseDto>>(obj);
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(500, _response);

            }
        }



    }
}
