
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PatientManagementApi.Core.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
         

        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }
        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().Where(filter).ToListAsync();
        }
        public async  Task<PaginationResult<TEntity>> GetAllPagination(PaginationRequest pagination)
        {
            var totalCount = await _context.Set<TEntity>().LongCountAsync();

            var entities = await _context.Set<TEntity>().Skip(pagination.PageSize * pagination.PageIndex)
                                       .Take(pagination.PageSize)
                                       .ToListAsync();

            return new PaginationResult<TEntity>(
                    pagination.PageIndex,
                    pagination.PageSize,
                    totalCount,
                    entities);
        }

        public void  Update(TEntity entity)
        {
             _context.Set<TEntity>().Update(entity);
        }
        public TEntity GetById(Guid id)
        {
            return _context.Set<TEntity>().Find(id);
        }
        public async Task<double> GetTotalCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
            {
                return await _context.Set<TEntity>().LongCountAsync();
            }
            else
            {
                return await _context.Set<TEntity>().Where(filter).LongCountAsync();
            }
        }
    }
}
