namespace PatientManagementApi.Services.IServices
{
    public interface IContactInforService
    {
        Task<List<ContactInfor>> GetAllContactInforAsync(Guid ? patientId = null );
        ContactInfor GetContactInforById(Guid id);
        Task<Guid> AddContactInforAsync(ContactInfor contactInfor);
        Task<Guid> UpdateContactInforAsync(ContactInfor contactInfor);
        Task DeleteContactInforAsync(Guid id);
    }
}
