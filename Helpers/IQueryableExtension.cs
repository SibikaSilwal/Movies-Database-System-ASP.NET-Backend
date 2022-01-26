using MoviesAPI.DTOs;
namespace MoviesAPI.Helpers
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> Paginate<T> (this IQueryable<T> queryable, PagginationDTO paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page - 1) * paginationDTO.RecordsPerPage)
                .Take(paginationDTO.RecordsPerPage);
        }
    }
}
