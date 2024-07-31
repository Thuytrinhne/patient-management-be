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
    public class ContactInforController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContactInforService _contactInforService;
        private readonly ResponseDto _response;

        public ContactInforController(IMapper mapper, IContactInforService contactInforService)
        {
            _mapper = mapper;
            _contactInforService = contactInforService;
            _response = new ResponseDto();
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
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
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(500, _response);
            }
        }

        [HttpGet("{id:Guid}", Name = "GetContactInforById")]
        public async Task<ActionResult<ResponseDto>> Get(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(500, _response);
            }
        }

        [HttpPost("{patientId:Guid}")]
        public async Task<ActionResult<ResponseDto>> Post(Guid patientId, CreateContactInforDto request)
        {
            try
            {
                var contactInforToAdd = _mapper.Map<ContactInfor>(request);
                contactInforToAdd.PatientId = patientId;

                var newContactInforId = await _contactInforService.AddContactInforAsync(contactInforToAdd);

                _response.Result = newContactInforId;
                return CreatedAtRoute("GetContactInforById", new { id = newContactInforId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
                return StatusCode(500, _response);
            }
        }

        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Patch(Guid id, UpdateContactInforDto request)
        {
            try
            {
                var contactInforToUpdate = _mapper.Map<ContactInfor>(request);
                contactInforToUpdate.Id = id;
                await _contactInforService.UpdateContactInforAsync(contactInforToUpdate);
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
                await _contactInforService.DeleteContactInforAsync(id);
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
