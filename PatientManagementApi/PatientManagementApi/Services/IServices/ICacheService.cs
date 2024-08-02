namespace PatientManagementApi.Services.IServices
{
    public interface ICacheService 
    {
        Task<Patient> GetPatient(Guid patientId);
        Task StorePatient(Patient patient);
        Task DeletePatient(Guid patientId);

    }
}
