
namespace PatientManagementApi.Repositories.IRepository
{
    public interface IPatientRepository :  IGenericRepository<Patient>
    {
        Task<Patient> GetPatientWithAddressesAndContactInfors(Guid patientId);
    } 
}
