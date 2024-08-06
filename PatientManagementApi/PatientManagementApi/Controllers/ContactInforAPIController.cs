using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]

    public class ContactInforAPIController (IMapper _mapper, IContactInforService _contactInforService) : ControllerBase
    {
     
        [HttpGet("{id:Guid}", Name = "GetContactInforById")]
        public async Task<ActionResult<ResponseDto>> Get(Guid id)
        {        
                var result = _contactInforService.GetContactInforById(id);
               return Ok(new ResponseDto { Result = _mapper.Map<GetContactInforDto>(result) });
      
        }

        [HttpPost("{patientId:Guid}")]
        public async Task<ActionResult<ResponseDto>> Post(Guid patientId, UpsertContactInforDto request)
        {
           
                var contactInforToAdd = _mapper.Map<ContactInfor>(request);
                contactInforToAdd.PatientId = patientId;

                var newContactInforId = await _contactInforService.AddContactInforAsync(contactInforToAdd);

                return CreatedAtRoute("GetContactInforById", new { id = newContactInforId }, new ResponseDto { Result = newContactInforId });
           
        }

        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Patch(Guid id, UpsertContactInforDto request)
        {
            
                var contactInforToUpdate = _mapper.Map<ContactInfor>(request);
                contactInforToUpdate.Id = id;
                await _contactInforService.UpdateContactInforAsync(contactInforToUpdate);
                return Ok(new ResponseDto { Result = id });

           
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
        {
                await _contactInforService.DeleteContactInforAsync(id);
                return Ok(new ResponseDto());
           
        }
    }
}
