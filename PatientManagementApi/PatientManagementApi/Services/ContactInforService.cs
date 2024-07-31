
using PatientManagementApi.Models;
using System.Net;

namespace PatientManagementApi.Services
{
    public class ContactInforService : IContactInforService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContactInforService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> AddContactInforAsync(ContactInfor contactInfor)
        {

            Patient PatientFrmDb = _unitOfWork.Patients.GetById(contactInfor.PatientId);
            if (PatientFrmDb is null)
                throw new KeyNotFoundException("A patient not found");
            await  _unitOfWork.ContactInfors.AddAsync(contactInfor);
            await _unitOfWork.SaveChangesAsync();
            return contactInfor.Id;
        }

        public async  Task DeleteContactInforAsync(Guid id)
        {
            var contactInforFrmDb = _unitOfWork.ContactInfors.GetById(id);
            if(contactInforFrmDb is not  null)
            {
                _unitOfWork.ContactInfors.Delete(contactInforFrmDb);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            throw new KeyNotFoundException("ContactInfor not found.");

        }

        public ContactInfor GetContactInforById(Guid id)
        {
            return  _unitOfWork.ContactInfors.GetById(id);
        }

        public async Task<List<ContactInfor>> GetAllContactInforAsync(Guid ? patientId = null)
        {
            if (patientId.HasValue)
            {
                return await _unitOfWork.ContactInfors.GetAllAsync(a => a.PatientId == patientId.Value);
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
                if (!String.IsNullOrEmpty(contactInfor.Value) && contactInfor.Value != contactInforFrmDb.Value)
                {
                    contactInforFrmDb.Value = contactInfor.Value;
                }
                if (Enum.IsDefined(typeof(ContactType), contactInfor.Type) && contactInfor.Type != contactInforFrmDb.Type)
                {  
                     contactInforFrmDb.Type = contactInfor.Type;                 
                }
                await _unitOfWork.SaveChangesAsync();
                return contactInforFrmDb.Id;
            }
            else 
            throw new KeyNotFoundException("ContactInfor not found.");

        }
    }
}
