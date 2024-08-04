using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.RateLimiting;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PatientManagementApi.Controllers
{
    [Route("api/patients")]
    [ApiController]
    [Authorize]

    public class PatientAPIController : ControllerBase
    { 
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly IPatientService _patientService;
        private readonly IAddressService _addressService;
        private readonly IContactInforService _contactInforService;

        public PatientAPIController(IMapper mapper, IPatientService patientService, IAddressService addressService, IContactInforService contactInforService)
        {
            _mapper = mapper;
            _response = new();
            _patientService = patientService;
            _addressService = addressService;
            _contactInforService = contactInforService;
       
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<ResponseDto>> Get
            ([FromQuery] PaginationRequest request,
            [FromQuery] string ? firstName,
            [FromQuery] string ? lastName,
            [FromQuery] DateTime ? dOB,
            [FromQuery] string ? phone,
            [FromQuery] string ? email,
            [FromQuery] bool ? isActive,
            [FromQuery] Gender? gender
            )
        {
            
                var result = await _patientService.GetAllPatientAsync(request, firstName, lastName, dOB, phone, email, isActive, gender);
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
           
                Patient ? patient = await _patientService.GetPatientById(id);
                _response.Result = _mapper.Map<GetPatientByIdResponseDto>(patient);
                return Ok(_response); 
        }
        [HttpGet("{id:Guid}/addresses")]
        public async Task<ActionResult<ResponseDto>> GetAddressByPatientId(Guid id)
        {
           
                var addresses = await _addressService.GetAllAddressAsync(id);
                _response.Result = _mapper.Map<IEnumerable<GetAddressDto>>(addresses);
                return Ok(_response);
            
        }
        [HttpGet("{id:Guid}/contact-infors")]
        public async Task<ActionResult<ResponseDto>> GetContactInforsByPatientId(Guid id)
        {
            var contactInfors = await _contactInforService.GetAllContactInforAsync(id);
            _response.Result = _mapper.Map<IEnumerable<GetContactInforDto>>(contactInfors);
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
