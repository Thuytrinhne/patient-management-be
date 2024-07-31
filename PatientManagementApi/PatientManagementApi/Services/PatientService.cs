using Npgsql;
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
        public async Task<Guid> AddPatientAsync(Patient patient)
        {
            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();
            return patient.Id;
        }

        public async Task DeactivePatient(Guid id, string deactiveReason)
        {
            Patient patientFrmDb = _unitOfWork.Patients.GetById(id);
            if(patientFrmDb == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }
            patientFrmDb.IsActive = false;
            patientFrmDb.DeactivationReason = deactiveReason;
            await _unitOfWork.SaveChangesAsync();

           

        }

        public async Task DeletePatientAsync(Guid patientId)
        {
            Patient patientToDelete = _unitOfWork.Patients.GetById(patientId);
            if(patientToDelete is null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            _unitOfWork.Patients.Delete(patientToDelete);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<PaginationResult<Patient>> GetAllPatientAsync
            (PaginationRequest request, string? firstName, string? lastName, DateTime? dOB,string? phone,
            string? email)
        {
            return await _unitOfWork.Patients.SearchPatientAsync(request, firstName, lastName ,dOB, phone, email);
        }

        public Patient GetPatientById(Guid id)
        {
            return _unitOfWork.Patients.GetById(id);
        }

        public async  Task<Guid> UpdatePatientAsync(Patient patient)
        {
            var patientFrmDb = _unitOfWork.Patients.GetById(patient.Id);
            if (patientFrmDb is null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }
            if (!String.IsNullOrEmpty(patient.FirstName)  && patientFrmDb.FirstName != patient.FirstName)
            {
                    patientFrmDb.FirstName = patient.FirstName;
            }
            if (!String.IsNullOrEmpty(patient.LastName) && patientFrmDb.LastName != patient.LastName)
            {
                   patientFrmDb.LastName = patient.LastName;

            }
            if (Enum.IsDefined(typeof(Gender), patient.Gender) && patientFrmDb.Gender != patient.Gender)
            {
                patientFrmDb.Gender = patient.Gender;
            }
            if (patient.DateOfBirth != default(DateTime) && patientFrmDb.DateOfBirth != patient.DateOfBirth)
            {
                patientFrmDb.DateOfBirth = patient.DateOfBirth;
            }

            await _unitOfWork.SaveChangesAsync();
            return patientFrmDb.Id;

        }


    }
}
