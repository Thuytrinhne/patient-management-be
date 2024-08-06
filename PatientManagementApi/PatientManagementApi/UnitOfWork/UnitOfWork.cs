
using Microsoft.AspNetCore.Identity;
using PatientManagementApi.Repositories;

namespace PatientManagementApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IPatientRepository _patients;
        private IContactInforRepository _contactInfors;
        private IAddressRepository _addresses;
        private IUserRepository _users;

        public IPatientRepository Patients => _patients ??= new PatientRepository(_context);
        public IContactInforRepository ContactInfors => _contactInfors ??= new ContactInforRepository(_context);
        public IAddressRepository Addresses => _addresses ??= new AddressRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
          

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
