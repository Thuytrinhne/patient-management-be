
using Microsoft.Extensions.Caching.Distributed;
using PatientManagementApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace PatientManagementApi.Services
{
    public class CacheService (IDistributedCache _cache) : ICacheService
    {
        public async  Task DeletePatient(Guid patientId)
        {
            await _cache.RemoveAsync(patientId.ToString());
        }

        public async  Task<Patient> GetPatient(Guid patientId)
        {
            var options = GetJsonSerializerOptions();
            var cachedPatient = await _cache.GetStringAsync(patientId.ToString());
            if (!string.IsNullOrEmpty(cachedPatient))
            {
                return JsonSerializer.Deserialize<Patient>(cachedPatient, options)!;
            }
            return null!;

        }

        public async Task StorePatient(Patient patient)
        {

            var options = GetJsonSerializerOptions();
              

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            var serializedPatient = JsonSerializer.Serialize(patient, options);
            await _cache.SetStringAsync(patient.Id.ToString(), serializedPatient, cacheOptions);
        }
        private JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new  JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Cho phép xử lý chu kỳ tham chiếu
                PropertyNameCaseInsensitive = true,
                WriteIndented = true // Tùy chọn này giúp JSON dễ đọc hơn, có thể bỏ qua nếu không cần
            };
        }
    }
}
