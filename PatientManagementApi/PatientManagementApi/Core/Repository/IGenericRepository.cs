using System.Linq.Expressions;

namespace PatientManagementApi.Core.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<PaginationResult<T>> GetAllPagination(PaginationRequest pagination);
    }
}
