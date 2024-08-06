using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PatientManagementApi.Core.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<PaginationResult<TEntity>> GetAllPagination(PaginationRequest pagination, Expression<Func<TEntity, bool>> filter = null);
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null);
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entities);
        TEntity GetById(Guid id);
        Task<double> CountPatientAsync(Expression<Func<TEntity, bool>> filter = null);
    }
}
