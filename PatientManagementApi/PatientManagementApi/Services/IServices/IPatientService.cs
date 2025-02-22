using PatientManagementApi.Dtos.Statistics;
using PatientManagementApi.Enums;

namespace PatientManagementApi.Services.IServices
{
    public interface IPatientService
    {
         Task<PaginationResult<Patient>> GetAllPatientAsync
            (PaginationRequest request, string? firstName, string? lastName, DateTime? dOB, string? phone, string? email, bool ? isActive, Gender ? gender);
         Task<Patient> GetPatientById(Guid id);
         Task  DeactivePatient(Guid id, string deactiveReason);
         Task<Guid> AddPatientAsync (Patient patient);
         Task<Guid> UpdatePatientAsync (Patient patient);
         Task DeletePatientAsync (Guid patientId);
         Task<PatientsStatistic> GetPatientsStatistic();
         Task<TodayPatientsStatistic> GetTodayPatientsStatistic();



    }
}
