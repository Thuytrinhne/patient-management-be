namespace PatientManagementApi.Services.IServices
{
    public interface IPatientService
    {
        public Task<PaginationResult<Patient>> GetAllPatientAsync (PaginationRequest request);
        public Task<Guid> AddPatientAsync (Patient patient);
        public Task<Guid> UpdatePatientAsync (Patient patient);
        public Task<Guid> DeletePatientAsync (Guid patientId);

    }
}
