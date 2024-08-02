namespace PatientManagementApi.Services.IServices
{
    public interface ICsvService
    {
        List<TEntity> ReadEntitiesFromCsv<TEntity>(string filePath) where TEntity : class;

    }
}
