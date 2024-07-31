
namespace PatientManagementApi.Repositories
{
    public class ContactInforRepository : GenericRepository<ContactInfor>, IContactInforRepository
    {
        public ContactInforRepository(AppDbContext context) : base(context)
        {
        }
    }
}
