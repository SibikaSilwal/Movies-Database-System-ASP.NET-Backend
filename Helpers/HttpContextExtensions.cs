using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParametersPagnationInHeader<T>(this HttpContext httpcontext, IQueryable<T> queryable)
        {
            if(httpcontext == null)
            {
                throw new ArgumentNullException(nameof(httpcontext));
            }

            double count = await queryable.CountAsync();
            httpcontext.Response.Headers.Add("totalAmountOfRecords", count.ToString());

        }
    }
}
