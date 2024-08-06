using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace PatientManagementApi.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    [Authorize]

    public class AddressAPIController(IMapper _mapper, IAddressService _addressService)  : ControllerBase
    {      
        [HttpGet("{id:Guid}", Name = "GetAddressById")]
        public async Task<ActionResult<ResponseDto>> Get(Guid id)
        {
           
                var  result = _addressService.GetAddressById(id);
                return Ok(new ResponseDto { Result = _mapper.Map<GetAddressDto>(result)  });
        }
        [HttpPost("{patientId:Guid}")]
        public async Task<ActionResult<ResponseDto>> Post
          (Guid patientId , UpsertAddressDto request)
        {
           
                var addressToAdd = _mapper.Map<Address>(request);
                addressToAdd.PatientId = patientId;

                var newAddressId = await _addressService.AddAddressAsync(addressToAdd);

                return CreatedAtRoute("GetAddressById", new { id = newAddressId }, new ResponseDto { Result = newAddressId });
      
        }
        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Patch
            (Guid id, UpsertAddressDto request)
        {
          
                var addressToUpdate = _mapper.Map<Address>(request);
                addressToUpdate.Id = id;
                await _addressService.UpdateAddressAsync(addressToUpdate);
                return Ok(new ResponseDto { Result = id });
     
        }
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
        {
             await _addressService.DeleteAddressAsync(id);
                return Ok(new ResponseDto());
            
        }
    }
}
