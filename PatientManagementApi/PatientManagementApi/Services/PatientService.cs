using System.Xml.Linq;

namespace PatientManagementApi.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<Guid> AddPatientAsync(Patient patient)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> DeletePatientAsync(Guid patientId)
        {
            throw new NotImplementedException();
        }

        public async  Task<PaginationResult<Patient>> GetAllPatientAsync(PaginationRequest request)
        {
            return await _unitOfWork.Patients.GetAllPagination(request);
        }

        public Task<Guid> UpdatePatientAsync(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}
