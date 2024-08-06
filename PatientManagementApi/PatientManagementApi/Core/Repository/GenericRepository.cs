
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

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {

            IQueryable<TEntity> query = _context.Set<TEntity>();
            return await (filter == null ? query : query.Where(filter)).ToListAsync();
        }
        public async  Task<PaginationResult<TEntity>> GetAllPagination(PaginationRequest pagination, Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query;
            if (filter is not  null)
            {
                query = _context.Set<TEntity>().AsNoTracking().Where(filter);
            }
            else
               query = _context.Set<TEntity>().AsNoTracking();

                var parameter = Expression.Parameter(typeof(TEntity), "p");
                var property = Expression.Property(parameter,"CreatedAt");
                var lambda = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(property, typeof(object)), parameter);
                query = query.OrderByDescending(lambda);
                     
            


            long totalCount = await query.LongCountAsync();

            var entities = await query.Skip(pagination.PageSize * pagination.PageIndex)
                                      .Take(pagination.PageSize)
                                      .ToListAsync();

            return new PaginationResult<TEntity>(
                    pagination.PageIndex,
                    pagination.PageSize,
                    totalCount,
                    entities);      
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }
        public void  Update(TEntity entity)
        {
             _context.Set<TEntity>().Update(entity);
        }
        public TEntity GetById(Guid id)
        {
            return _context.Set<TEntity>().Find(id);
        }
        public async Task<double> CountPatientAsync(Expression<Func<TEntity, bool>> filter = null)
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
