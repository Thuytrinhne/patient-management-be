
namespace PatientManagementApi.Repositories.IRepository
{
    public interface IPatientRepository :  IGenericRepository<Patient>
    {
        Task<PaginationResult<Patient>> SearchPatientAsync
            (PaginationRequest request,
            string? firstName,
            string? lastName,
            DateTime? dOB,
            string? phone,
            string? email);
    } 
}
