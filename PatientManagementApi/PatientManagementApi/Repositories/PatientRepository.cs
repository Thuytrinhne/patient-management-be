using PatientManagementApi.Extensions;
using System.Linq.Expressions;

namespace PatientManagementApi.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context)
        {
            
        }
        public async Task<Patient> GetPatientWithAddressesAndContactInfors(Guid patientId)
        {
            return  await  _context.Patients
                                 .Include(p => p.Addresses)
                                 .Include(p=>p.ContactInfors)
                                 .FirstOrDefaultAsync(p => p.Id == patientId) ;
           
        }


    }
   
}
