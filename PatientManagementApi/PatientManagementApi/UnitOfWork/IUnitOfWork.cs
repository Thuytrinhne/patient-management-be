namespace PatientManagementApi.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients { get; }
        IAddressRepository Addresses { get; }
        IContactInforRepository ContactInfors { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
