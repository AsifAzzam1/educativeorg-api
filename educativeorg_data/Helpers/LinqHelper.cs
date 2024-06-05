using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static educativeorg_models.ViewModels.ExceptionResposne;

namespace educativeorg_data.Helpers
{
    public static class LinqHelper
    {
        public static T First<T>(this IEnumerable<T> sources, string message, Func<T, bool>? predicate = null)
        {
            T? result;
            if(predicate != null)
                result = sources.Where(predicate).FirstOrDefault();
            else
                result = sources.FirstOrDefault();

            if (result == null)
                throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, message);

            return result!;
        }

        public static async Task<T> FirstAsync<T>(this IQueryable<T> sources, string message, Func<T, bool>? predicate = null)
        {

            T? result;
            if (predicate != null)
                result =  sources.Where(predicate).FirstOrDefault();
            else
                result = await sources.FirstOrDefaultAsync();

            if (result == null)
                throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, message);

            return result!;
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> sources, Expression<Func<T, bool>> predicate, string message)
        {

            var result = sources.Where(predicate);
            if (result == null || result.Count() == 0)
                throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, message);

            return result!;
        }
    }
}
