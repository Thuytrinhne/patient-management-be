
using Microsoft.AspNetCore.Identity;
using PatientManagementApi.Repositories;

namespace PatientManagementApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IPatientRepository Patients { get; private set; }
        public IContactInforRepository ContactInfors { get; private set; }
        public IAddressRepository Addresses { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Patients = new PatientRepository(_context);
            ContactInfors = new ContactInforRepository(_context);
            Addresses = new AddressRepository(_context);    

        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
           _context.Dispose();
        }
    }
}
