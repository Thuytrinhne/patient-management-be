using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PatientManagementApi.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientAPIController : ControllerBase
    { 
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly IPatientService _patientService;

        public PatientAPIController(IMapper mapper, IPatientService patientService)
        {
            _mapper = mapper;
            _response = new();
            _patientService = patientService;
       
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get
            ([FromQuery] PaginationRequest request,
            [FromQuery] string ? firstName,
            [FromQuery] string ? lastName,
            [FromQuery] DateTime ? dOB,
            [FromQuery] string ? phone,
            [FromQuery] string ? email)
        {
            
                var result = await _patientService.GetAllPatientAsync(request, firstName, lastName, dOB, phone, email);
                if (result is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "There was not any patient record in system !";
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<PaginationResult<GetPatientsResponseDto>>(result);
                return Ok(_response);

            
        }
        [HttpGet("{id:Guid}", Name = "GetPatientById")]
        public async Task<ActionResult<ResponseDto>> Get(Guid id)
        {
           
                Patient ? patient = _patientService.GetPatientById(id);
                if (patient is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Patient doesn't exist in the system !";
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<GetPatientsResponseDto>(patient);
                return Ok(_response);
           
          
        }
        [HttpGet("{id:Guid}/addresses")]
        public async Task<ActionResult<ResponseDto>> GetAddressByPatientId(Guid id)
        {
           
                Patient? patient = _patientService.GetPatientById(id);
                if (patient is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Patient doesn't exist in the system !";
                    return NotFound(_response);
                }
                if(patient.Addresses.Count <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "This patient's addresses do not exist in the system !";
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<IEnumerable<GetAddressDto>>(patient.Addresses);
                return Ok(_response);
            

        }
        [HttpGet("{id:Guid}/contact-infors")]
        public async Task<ActionResult<ResponseDto>> GetContactInforsByPatientId(Guid id)
        {
           
                Patient? patient = _patientService.GetPatientById(id);
                if (patient is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Patient doesn't exist in the system !";
                    return NotFound(_response);
                }
                if (patient.ContactInfors.Count <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "This patient's contact infors do not exist in the system !";
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<IEnumerable<GetContactInforDto>>(patient.ContactInfors);
                return Ok(_response);
            

        }
        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Post
            (CreatePatientRequestDto request)
        {
          
                Patient patientToAdd = _mapper.Map<Patient>(request); 
                Guid NewPatientId= await _patientService.AddPatientAsync(patientToAdd);

                _response.Result = NewPatientId;
                return CreatedAtRoute("GetPatientById", new { id = NewPatientId }, _response);

           
        }
        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Patch
            (Guid id, UpdatePatientRequestDto request)
        {
               Patient patientToUpdate = _mapper.Map<Patient>(request);
                patientToUpdate.Id = id;
                var patientId = await _patientService.UpdatePatientAsync(patientToUpdate);
                _response.Result = patientId;
                return Ok(_response);
           
        }


        [HttpPost("{id:Guid}/deactivation")]
        public async Task<ActionResult<ResponseDto>> Deactivate (Guid id, [FromBody] string deactiveReason)
        {
                    await _patientService.DeactivePatient(id, deactiveReason);                
                    return Ok(_response);  
            
        }
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
        {
            
                await _patientService.DeletePatientAsync(id);
                return Ok(_response);
        }

    }
}
