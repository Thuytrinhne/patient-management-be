namespace PatientManagementApi.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients { get; }
        Task<int> SaveChangesAsync();
    }
}
