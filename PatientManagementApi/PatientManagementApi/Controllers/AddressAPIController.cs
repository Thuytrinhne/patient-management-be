using Newtonsoft.Json.Linq;

namespace PatientManagementApi.Controllers
{
    [Route("api/addresses")]
    [ApiController]
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
            try
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
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(500, _response);

            }
        }
        [HttpGet("{id:Guid}", Name = "GetAddressById")]
        public async Task<ActionResult<ResponseDto>> Get(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
                return StatusCode(500, _response);
            }

        }
        [HttpPost("{patientId:Guid}")]
        public async Task<ActionResult<ResponseDto>> Post
          (Guid patientId , CreateAddressDto request)
        {
            try
            {
                var addressToAdd = _mapper.Map<Address>(request);
                addressToAdd.PatientId = patientId;

                var NewAddressId = await _addressService.AddAddressAsync(addressToAdd);

                _response.Result = NewAddressId;
                return CreatedAtRoute("GetAddressById", new { id = NewAddressId }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(500, _response);

            }
        }
        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Patch
            (Guid id, UpdateAddressDto request)
        {
            try
            {
                var addressToUpdate = _mapper.Map<Address>(request);
                addressToUpdate.Id = id;
                await _addressService.UpdateAddressAsync(addressToUpdate);
                _response.Result = id;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(500, _response);

            }
        }
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
        {
            try
            {
                await _addressService.DeleteAddressAsync(id);
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
