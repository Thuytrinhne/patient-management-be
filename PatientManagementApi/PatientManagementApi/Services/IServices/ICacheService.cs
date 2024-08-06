namespace PatientManagementApi.Services.IServices
{
    public interface ICacheService
    {
        Task<T> GetData<T>(string key);
        Task StoreData<T>(string key, T obj);
        Task DeleteData(string key);

    }
}
