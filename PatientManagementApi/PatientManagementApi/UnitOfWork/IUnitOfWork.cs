namespace PatientManagementApi.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients { get; }
        IAddressRepository Addresses { get; }
        IContactInforRepository ContactInfors { get; }

        Task<int> SaveChangesAsync();
    }
}
