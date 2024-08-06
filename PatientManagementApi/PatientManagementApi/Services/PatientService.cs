using Microsoft.EntityFrameworkCore;
using Npgsql;
using PatientManagementApi.Dtos.Statistics;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.Linq;
using PatientManagementApi.Models;
using System.Linq.Expressions;
using PatientManagementApi.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PatientManagementApi.Services
{
    public class PatientService
        (IUnitOfWork _unitOfWork, ICacheService _cacheService)
        : IPatientService
    {
        public async Task<Guid> AddPatientAsync(Patient patient)
        {
            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();
            return patient.Id;
        }

        public async Task DeactivePatient(Guid id, string deactiveReason)
        {
            var patientFrmDb =  EnsurePatientExists(id);
        
            if (!patientFrmDb.IsActive)
            {
                throw new BadRequestException("Patient has already deactivated.");

            }
            patientFrmDb.IsActive = false;
            patientFrmDb.DeactivatedAt = DateTime.Now;
            patientFrmDb.DeactivationReason = deactiveReason;
            await _unitOfWork.SaveChangesAsync();
            await _cacheService.DeleteData(patientFrmDb.Id.ToString());



        }

        public async Task DeletePatientAsync(Guid patientId)
        {
            var patientToDelete = EnsurePatientExists(patientId);

            _unitOfWork.Patients.Delete(patientToDelete);
            await _unitOfWork.SaveChangesAsync();
            await _cacheService.DeleteData(patientId.ToString());
        }
        public async Task<PaginationResult<Patient>> GetAllPatientAsync
            (PaginationRequest request, string? FirstName, string? LastName, DateTime? dOB,string? phone,
            string? email, bool? isActive, Gender? gender)
        {
            var patientExpression = createExpression(FirstName, LastName, dOB, phone, email, isActive, gender);
            return await _unitOfWork.Patients.GetAllPagination(request, patientExpression);
        }

        public async Task<Patient> GetPatientById(Guid id)
        {
            var patientFrmCache = await GetCachedData<Patient>(id.ToString());
            if (patientFrmCache is not null)
            {
                return patientFrmCache;
            }
            

            var patientFrmDb = await  _unitOfWork.Patients.GetPatientWithAddressesAndContactInfors(id);
            if (patientFrmDb is null)
            throw new NotFoundException($"This patient with ID {id} does not exist in the system.");

            await _cacheService.StoreData(patientFrmDb.Id.ToString(), patientFrmDb);
            return patientFrmDb!;
            
        }

        public async Task<PatientsStatistic> GetPatientsStatistic()
        {

            var totalStatisticFrmCache = await GetCachedData<PatientsStatistic>(CacheKeys.PatientsStatisticTotal);
            if (totalStatisticFrmCache != null)
            {
                return totalStatisticFrmCache;
            }
            var totalPatients = await _unitOfWork.Patients.CountPatientAsync();
            var activeTotal = await _unitOfWork.Patients.CountPatientAsync(p => p.IsActive);
            var deactivatedTotal = totalPatients - activeTotal;
            var result = new PatientsStatistic
            {
                TotalPatient = totalPatients,
                ActivedTotal = activeTotal,
                DeactivedTotal = deactivatedTotal
            };

            await _cacheService.StoreData(CacheKeys.PatientsStatisticTotal, result);


            return result;
        }

        public async Task<TodayPatientsStatistic> GetTodayPatientsStatistic()
        {
            var todayStatisticFrmCache = await GetCachedData<TodayPatientsStatistic>(CacheKeys.TodayPatientsStatisticTotal);
            if (todayStatisticFrmCache != null)
            {
                return todayStatisticFrmCache;
            }
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);

            var newPatientsToday = await _unitOfWork.Patients.CountPatientAsync(p => p.CreatedAt >= today && p.CreatedAt < tomorrow);

            var deactivatedPatientsToday = await _unitOfWork.Patients.CountPatientAsync(p => p.IsActive ==false &&( p.DeactivatedAt >= today && p.DeactivatedAt < tomorrow));

           var result =  new TodayPatientsStatistic
            {
                TodayNewPatient = newPatientsToday,
                TodayDeactivedTotal = deactivatedPatientsToday
            };
            await _cacheService.StoreData(CacheKeys.TodayPatientsStatisticTotal, result);
            return result;

        }

        public async  Task<Guid> UpdatePatientAsync(Patient patient)
        {
            var patientFrmDb = EnsurePatientExists(patient.Id);
            bool updated = false;
            if (!String.IsNullOrEmpty(patient.FirstName)  && patientFrmDb.FirstName != patient.FirstName)
            {
                    patientFrmDb.FirstName = patient.FirstName;
                    updated = true;
            }
            if (!String.IsNullOrEmpty(patient.LastName) && patientFrmDb.LastName != patient.LastName)
            {
                   patientFrmDb.LastName = patient.LastName;
                   updated = true;

            }
            if (Enum.IsDefined(typeof(Gender), patient.Gender) && patientFrmDb.Gender != patient.Gender)
            {
                patientFrmDb.Gender = patient.Gender;
                updated = true;

            }
            if (patient.DateOfBirth != default(DateTime) && patientFrmDb.DateOfBirth != patient.DateOfBirth)
            {
                patientFrmDb.DateOfBirth = patient.DateOfBirth;
                updated = true;

            }

            if (updated)
            {
                await _unitOfWork.SaveChangesAsync();
                await _cacheService.DeleteData(patientFrmDb.Id.ToString());

            }
            return patientFrmDb.Id;

        }
        private async Task<T> GetCachedData<T>(string cacheKey) where T : class
        {
            var dataFromCache = await _cacheService.GetData<T>(cacheKey);
            if (dataFromCache is not null)
            {
 
                return dataFromCache;
            }
            return null!;
        }
        private Expression<Func<Patient, bool>> createExpression(
                string? firstName,
                string? lastName,
                DateTime? dOB,
                string? phone,
                string? email,
                bool? isActive,
                Gender? gender
              )
        {
            Expression<Func<Patient, bool>> patientExpression = patient => true;

            if (!string.IsNullOrEmpty(firstName))
            {
                patientExpression = patientExpression.And(p => p.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                patientExpression = patientExpression.And(p => p.LastName.Contains(lastName));
            }

            if (dOB.HasValue)
            {
                patientExpression = patientExpression.And(p => p.DateOfBirth.Date == dOB.Value.Date);
            }

            if (!string.IsNullOrEmpty(phone))
            {
                patientExpression = patientExpression.And(p =>
                    p.ContactInfors.Any(c => c.Type == ContactType.Phone && c.Value == phone));
            }

            if (!string.IsNullOrEmpty(email))
            {
                patientExpression = patientExpression.And(p =>
                    p.ContactInfors.Any(c => c.Type == ContactType.Email && c.Value.Contains(email)));
            }

            if (isActive.HasValue)
            {
                patientExpression = patientExpression.And(p => p.IsActive == isActive.Value);
            }

            if (gender.HasValue)
            {
                patientExpression = patientExpression.And(p => p.Gender == gender);
            }
         
            return patientExpression;
        }
        private Patient EnsurePatientExists(Guid id)
        {
            var patient =  _unitOfWork.Patients.GetById(id);
            if (patient == null)
            {
                throw new NotFoundException($"This patient with ID {id} does not exist in the system.");
            }
            return patient;
        }
    }

}
