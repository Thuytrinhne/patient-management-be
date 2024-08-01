using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PatientManagementApi.Dtos.ContactInfor;
using PatientManagementApi.Models;
using PatientManagementApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientManagementApi.Controllers
{
    [Route("api/contact-infors")]
    [ApiController]
    public class ContactInforAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContactInforService _contactInforService;
        private readonly ResponseDto _response;

        public ContactInforAPIController(IMapper mapper, IContactInforService contactInforService)
        {
            _mapper = mapper;
            _contactInforService = contactInforService;
            _response = new ResponseDto();
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
           
                var result = await _contactInforService.GetAllContactInforAsync();
                if (result is null || result.Count == 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "There are no contact information in the system ";
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<IEnumerable<GetContactInforDto>>(result);
                return Ok(_response);
         
        }

        [HttpGet("{id:Guid}", Name = "GetContactInforById")]
        public async Task<ActionResult<ResponseDto>> Get(Guid id)
        {
           
                var result = _contactInforService.GetContactInforById(id);
                if (result is null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "This contact information doesn't exist in the system.";
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<GetContactInforDto>(result);
                return Ok(_response);
           
        }

        [HttpPost("{patientId:Guid}")]
        public async Task<ActionResult<ResponseDto>> Post(Guid patientId, UpsertContactInforDto request)
        {
           
                var contactInforToAdd = _mapper.Map<ContactInfor>(request);
                contactInforToAdd.PatientId = patientId;

                var newContactInforId = await _contactInforService.AddContactInforAsync(contactInforToAdd);

                _response.Result = newContactInforId;
                return CreatedAtRoute("GetContactInforById", new { id = newContactInforId }, _response);
           
        }

        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Patch(Guid id, UpsertContactInforDto request)
        {
            
                var contactInforToUpdate = _mapper.Map<ContactInfor>(request);
                contactInforToUpdate.Id = id;
                await _contactInforService.UpdateContactInforAsync(contactInforToUpdate);
                _response.Result = id;
                return Ok(_response);
           
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
        {
           
                await _contactInforService.DeleteContactInforAsync(id);
                return Ok(_response);
           
        }
    }
}
