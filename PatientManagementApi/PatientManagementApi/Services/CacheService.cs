
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

        public async Task<Object> GetData(string  key)
        {
            var options = GetJsonSerializerOptions();
            var cachedPatient = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cachedPatient))
            {
                return JsonSerializer.Deserialize<Object>(cachedPatient, options)!;
            }
            return null!;

        }

        public async Task StoreData(string key, Object obj)
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
                ReferenceHandler = ReferenceHandler.Preserve, // Cho phép xử lý chu kỳ tham chiếu
                PropertyNameCaseInsensitive = true,
                WriteIndented = true // Tùy chọn này giúp JSON dễ đọc hơn, có thể bỏ qua nếu không cần
            };
        }
    }
}
