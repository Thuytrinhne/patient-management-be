
namespace PatientManagementApi.Core.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
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
    }
}
