using Microsoft.EntityFrameworkCore;
using Npgsql;
using PatientManagementApi.Dtos.Statistics;
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
                throw new NotFoundException("Patient not found.");
            }
            patientFrmDb.IsActive = false;
            patientFrmDb.DeactivatedAt  = DateTime.UtcNow;
            patientFrmDb.DeactivationReason = deactiveReason;
            await _unitOfWork.SaveChangesAsync();

           
        }

        public async Task DeletePatientAsync(Guid patientId)
        {
            Patient patientToDelete = _unitOfWork.Patients.GetById(patientId);
            if(patientToDelete is null)
            {
                throw new NotFoundException("Patient not found.");
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

        public async Task<PatientsStatistic> GetPatientsStatistic()
        {
            var totalPatients = await _unitOfWork.Patients.GetTotalCountAsync();
            var activeTotal = await _unitOfWork.Patients.GetTotalCountAsync(p => p.IsActive);
            var deactivatedTotal = totalPatients - activeTotal;

           return  new PatientsStatistic
            {
                TotalPatient = totalPatients,
                ActivedTotal = activeTotal,
                DeactivedTotal = deactivatedTotal
            };
        }

        public async Task<TodayPatientsStatistic> GetTodayPatientsStatistic()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var newPatientsToday = await _unitOfWork.Patients.GetTotalCountAsync(p => p.CreatedAt >= today && p.CreatedAt < tomorrow);

            var deactivatedPatientsToday = await _unitOfWork.Patients.GetTotalCountAsync(p => p.IsActive ==false && p.DeactivatedAt >= today && p.DeactivatedAt < tomorrow);

           return new TodayPatientsStatistic
            {
                TodayNewPatient = newPatientsToday,
                TodayDeactivedTotal = deactivatedPatientsToday
            };

        }

        public async  Task<Guid> UpdatePatientAsync(Patient patient)
        {
            var patientFrmDb = _unitOfWork.Patients.GetById(patient.Id);
            if (patientFrmDb is null)
            {
                throw new NotFoundException("Patient not found.");
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
