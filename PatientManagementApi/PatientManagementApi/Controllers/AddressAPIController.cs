using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace PatientManagementApi.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    [Authorize]

    public class AddressAPIController  : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private IAddressService _addressService;
        public AddressAPIController(IMapper mapper, IAddressService addressService)
        {
            _mapper = mapper;
            _response = new();
            _addressService = addressService;
        }
 

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            var result = await _addressService.GetAllAddressAsync();
                if (result is null || result.Count() ==0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "There doesn't not any address in system !";
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<IEnumerable<GetAddressDto>>(result);
                return Ok(_response);

            
          
        }
        [HttpGet("{id:Guid}", Name = "GetAddressById")]
        public async Task<ActionResult<ResponseDto>> Get(Guid id)
        {
           
                var  result = _addressService.GetAddressById(id);
                if (result is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "This address doesn't exist in the system.";
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<GetAddressDto>(result);
                return Ok(_response);
            
          
        }
        [HttpPost("{patientId:Guid}")]
        public async Task<ActionResult<ResponseDto>> Post
          (Guid patientId , UpsertAddressDto request)
        {
           
                var addressToAdd = _mapper.Map<Address>(request);
                addressToAdd.PatientId = patientId;

                var NewAddressId = await _addressService.AddAddressAsync(addressToAdd);

                _response.Result = NewAddressId;
                return CreatedAtRoute("GetAddressById", new { id = NewAddressId }, _response);
      
        }
        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Patch
            (Guid id, UpsertAddressDto request)
        {
          
                var addressToUpdate = _mapper.Map<Address>(request);
                addressToUpdate.Id = id;
                await _addressService.UpdateAddressAsync(addressToUpdate);
                _response.Result = id;
                return Ok(_response);
           
        }
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
        {
             await _addressService.DeleteAddressAsync(id);
                return Ok(_response);
            
        }
    }
}
