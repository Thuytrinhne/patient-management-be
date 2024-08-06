
using PatientManagementApi.Models;
using System.Net;

namespace PatientManagementApi.Services
{
    public class ContactInforService (ICacheService _cacheService, IUnitOfWork _unitOfWork) : IContactInforService
    {
      
        public async Task<Guid> AddContactInforAsync(ContactInfor contactInfor)
        {

            Patient patientFrmDb = _unitOfWork.Patients.GetById(contactInfor.PatientId);
            if (patientFrmDb is null)
                throw new NotFoundException($"This patient with ID {contactInfor.PatientId} does not exist in the system.");
            await _unitOfWork.ContactInfors.AddAsync(contactInfor);
            await _unitOfWork.SaveChangesAsync();
            await _cacheService.DeleteData(patientFrmDb.Id.ToString());

            return contactInfor.Id;
        }

        public async  Task DeleteContactInforAsync(Guid id)
        {
            var contactInforFrmDb = _unitOfWork.ContactInfors.GetById(id);
            if(contactInforFrmDb is not  null)
            {
                _unitOfWork.ContactInfors.Delete(contactInforFrmDb);
                await _cacheService.DeleteData(contactInforFrmDb.PatientId.ToString());
                await _unitOfWork.SaveChangesAsync();

            }
            else
            throw new NotFoundException($"ContactInfor with ID {id} not found.");

        }

        public ContactInfor GetContactInforById(Guid id)
        {

            var result =   _unitOfWork.ContactInfors.GetById(id);
            if (result is null)
                throw new NotFoundException($"ContactInfor with ID {id} not found.");
            return result;
        }

        public async Task<List<ContactInfor>> GetAllContactInforAsync(Guid ? PatientId)
        {
            if (PatientId.HasValue)
            {
                return await _unitOfWork.ContactInfors.GetAllAsync(a => a.PatientId == PatientId.Value);
            }
            else
            {
                return await _unitOfWork.ContactInfors.GetAllAsync();
            }
        }

        public async  Task<Guid> UpdateContactInforAsync(ContactInfor contactInfor)
        {
            var contactInforFrmDb = _unitOfWork.ContactInfors.GetById(contactInfor.Id);
            if(contactInforFrmDb is not null)
            {
                bool updated = false;

                if (!String.IsNullOrEmpty(contactInfor.Value) && contactInfor.Value != contactInforFrmDb.Value)
                {
                    contactInforFrmDb.Value = contactInfor.Value;
                    updated = true;
                }
                if (Enum.IsDefined(typeof(ContactType), contactInfor.Type) && contactInfor.Type != contactInforFrmDb.Type)
                {  
                     contactInforFrmDb.Type = contactInfor.Type;
                     updated = true;

                }
                if (updated)
                {
                    await _unitOfWork.SaveChangesAsync();
                    await _cacheService.DeleteData(contactInforFrmDb.PatientId.ToString());
                }
                return contactInforFrmDb.Id;
            }
            else
                throw new NotFoundException($"ContactInfor with ID {contactInfor.Id} not found.");

        }
    }
}
