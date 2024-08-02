using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PatientManagementApi.Core.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<PaginationResult<T>> GetAllPagination(PaginationRequest pagination);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entities);
        T GetById(Guid id);
        Task<double> GetTotalCountAsync(Expression<Func<T, bool>> filter = null);
    }
}
