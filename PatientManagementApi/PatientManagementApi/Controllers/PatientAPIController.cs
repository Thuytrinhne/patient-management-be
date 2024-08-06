using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.RateLimiting;
using PatientManagementApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PatientManagementApi.Controllers
{
    [Route("api/patients")]
    [ApiController]
    [Authorize]

    public class PatientAPIController
        (IMapper _mapper, IPatientService _patientService,
        IAddressService _addressService,
        IContactInforService _contactInforService
        )
        : ControllerBase
    { 
     
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
            return Ok(new ResponseDto { Result = _mapper.Map<PaginationResult<GetPatientsResponseDto>>(result) });
        }
        [HttpGet("{id:Guid}", Name = "GetPatientById")]
        public async Task<ActionResult<ResponseDto>> Get(Guid id)
        {

            var patient = await _patientService.GetPatientById(id);
            return Ok(new ResponseDto { Result = _mapper.Map<GetPatientByIdResponseDto>(patient) });

        }
        [HttpGet("{id:Guid}/addresses")]
        public async Task<ActionResult<ResponseDto>> GetAddressByPatientId(Guid id)
        {

            var addresses = await _addressService.GetAllAddressAsync(id);
            return Ok(new ResponseDto { Result = _mapper.Map<IEnumerable<GetAddressDto>>(addresses) });


        }
        [HttpGet("{id:Guid}/contact-infors")]
        public async Task<ActionResult<ResponseDto>> GetContactInforsByPatientId(Guid id)
        {
            var contactInfors = await _contactInforService.GetAllContactInforAsync(id);

            return Ok(new ResponseDto { Result = _mapper.Map<IEnumerable<GetContactInforDto>>(contactInfors) });

        }
        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Post
            (CreatePatientRequestDto request)
        {
          
                Patient patientToAdd = _mapper.Map<Patient>(request); 
                Guid newPatientId= await _patientService.AddPatientAsync(patientToAdd);

                return CreatedAtRoute("GetPatientById", new { id = newPatientId }, new ResponseDto { Result = newPatientId });



        }
        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Patch
            (Guid id, UpdatePatientRequestDto request)
        {
                Patient patientToUpdate = _mapper.Map<Patient>(request);
                patientToUpdate.Id = id;
                var patientId = await _patientService.UpdatePatientAsync(patientToUpdate);
                return Ok(new ResponseDto { Result = patientId });
        }


        [HttpPost("{id:Guid}/deactivation")]
        public async Task<ActionResult<ResponseDto>> Deactivate (Guid id, [FromBody] string deactiveReason)
        {
                    await _patientService.DeactivePatient(id, deactiveReason);
                    return Ok(new ResponseDto { IsSuccess = true });
        }
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
        {
                await _patientService.DeletePatientAsync(id);
                return Ok(new ResponseDto { IsSuccess = true });
        }
      
    }
}
