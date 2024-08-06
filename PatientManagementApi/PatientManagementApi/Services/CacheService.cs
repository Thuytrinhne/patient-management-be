
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using PatientManagementApi.Core;
using PatientManagementApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace PatientManagementApi.Services
{
    public class CacheService (IDistributedCache _cache) : ICacheService
    {
        public async Task DeleteData(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task<T> GetData<T>(string  key)
        {
            var options = GetJsonSerializerOptions();
            var cachedPatient = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cachedPatient))
            {
                return JsonSerializer.Deserialize<T>(cachedPatient, options)!;
            }
            return default(T);

        }

        public async Task StoreData<T>(string key, T obj)
        {

            var options = GetJsonSerializerOptions();
              

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            var serializedPatient = JsonSerializer.Serialize(obj, options);
            await _cache.SetStringAsync(key, serializedPatient, cacheOptions);
        }
        private JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new  JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, 
                PropertyNameCaseInsensitive = true,
                WriteIndented = true 
            };
        }
    }
}
