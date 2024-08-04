namespace PatientManagementApi.Services.IServices
{
    public interface ICacheService
    {
        Task<Object> GetData(string key);
        Task StoreData(string key, Object obj);
        Task DeleteData(string key);

    }
}
