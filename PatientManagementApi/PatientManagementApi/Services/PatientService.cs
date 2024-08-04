using Microsoft.EntityFrameworkCore;
using Npgsql;
using PatientManagementApi.Dtos.Statistics;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.Linq;

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
            Patient patientFrmDb = _unitOfWork.Patients.GetById(id);
            if(patientFrmDb == null)
            {
                throw new NotFoundException("Patient not found.");
            }
            if (!patientFrmDb.IsActive)
            {
                throw new BadRequestException("Patient has already deactivated.");

            }
            patientFrmDb.IsActive = false;
            patientFrmDb.DeactivatedAt = DateTime.Now;
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
            await _cacheService.DeleteData(patientId.ToString());
        }
        public async Task<PaginationResult<Patient>> GetAllPatientAsync
            (PaginationRequest request, string? FirstName, string? LastName, DateTime? dOB,string? phone,
            string? email, bool? isActive, Gender? gender)
        {
            return await _unitOfWork.Patients.SearchPatientAsync(request, FirstName, LastName ,dOB, phone, email, isActive, gender);
        }

        public async Task<Patient> GetPatientById(Guid id)
        {
            var patientFrmCache = await GetCachedData<Patient>(id.ToString());
            if (patientFrmCache is not null)
            {
                return patientFrmCache;
            }
            

            var patientFrmDb = await  _unitOfWork.Patients.GetPatientWithAddresses(id);
            if (patientFrmDb is not null)
            {
                await _cacheService.StoreData(patientFrmDb.Id.ToString(), patientFrmDb);
                return patientFrmDb!;
            }
            throw new NotFoundException("Patient doesn't exist in the system !");
        }

        public async Task<PatientsStatistic> GetPatientsStatistic()
        {

            var totalStatisticFrmCache = await GetCachedData<PatientsStatistic>(CacheKeys.PatientsStatisticTotal);
            if (totalStatisticFrmCache != null)
            {
                return totalStatisticFrmCache;
            }
            var totalPatients = await _unitOfWork.Patients.GetTotalCountAsync();
            var activeTotal = await _unitOfWork.Patients.GetTotalCountAsync(p => p.IsActive);
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

            var newPatientsToday = await _unitOfWork.Patients.GetTotalCountAsync(p => p.CreatedAt >= today && p.CreatedAt < tomorrow);

            var deactivatedPatientsToday = await _unitOfWork.Patients.GetTotalCountAsync(p => p.IsActive ==false &&( p.DeactivatedAt >= today && p.DeactivatedAt < tomorrow));

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
        public async Task<T> GetCachedData<T>(string cacheKey) where T : class
        {
            var dataFromCache = await _cacheService.GetData(cacheKey);
            if (dataFromCache != null)
            {
                try
                {
                    var dataDecoded = JsonSerializer.Deserialize<T>(dataFromCache.ToString(), new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        PropertyNameCaseInsensitive = true
                    });
                    return dataDecoded;
                }
                catch (JsonException ex)
                {
                    // Log the exception or handle it as needed
                    throw new InvalidOperationException("Failed to deserialize the data from cache.", ex);
                }
            }
            return null;
        }

    }
}
