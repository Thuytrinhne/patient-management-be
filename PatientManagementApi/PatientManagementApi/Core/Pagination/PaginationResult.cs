namespace PatientManagementApi.Core.Pagination
{
    public class PaginationResult<TEntity> where TEntity : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long Count { get; set; }
        public IEnumerable<TEntity> Data  { get; set; }

        public PaginationResult(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
        public PaginationResult()
        {

        }
    }
}
