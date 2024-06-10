﻿using educativeorg_data.Data;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
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

        public static void InitiazlizeBaseColumns(this BaseEntity entity, Guid? userid = null, bool active = true)
        {
            entity.Active = active;
            entity.ModifiedDate = DateTime.Now;
            entity.CreatedDate = entity.CreatedDate != DateTime.MinValue ? entity.CreatedDate : DateTime.Now;
            entity.ModifiedBy = userid;
            entity.CreatedBy = (entity.CreatedBy != null && entity.CreatedBy != Guid.Empty) ? entity.CreatedBy : userid;
        }

        public static void ToggleEntity<T>(this BaseEntity Entity, EducativeOrgDbContext _context, Guid userid) where T : BaseEntity
        {
            try
            {
                var entity = (T)Entity;
                entity.InitiazlizeBaseColumns(userid, !entity.Active);
                _context.Entry(entity).Property(x => x.Active).IsModified = true;
                _context.Entry(entity).Property(x => x.ModifiedDate).IsModified = true;
                _context.Entry(entity).Property(x => x.ModifiedBy).IsModified = true;

                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<PaginateResponseModel<T>> Paginate<T>(this IQueryable<T> query, FilterViewModel filter, Expression<Func<T, bool>> queryFilters) where T : BaseEntity
        {
            var res = new PaginateResponseModel<T>();
            res.TotalRecords = query.Count();

            if (filter.Status.HasValue)
                query = query.Where(_ => _.Active == filter.Status.Value);

            if (!string.IsNullOrWhiteSpace(filter.Query))
            {
                query = query.Where(queryFilters);
            }

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
                query = filter.SortDesc != null && filter.SortDesc!.Value ? query.OrderByDescending(ToLambda<T>(filter.SortBy)) : query = query.OrderBy(ToLambda<T>(filter.SortBy));
            //else
            //    query = query.OrderBy(ToLambda<T>(defaultSortBy));


            if (filter.PageSize == -1)
                res.Data = await query.ToListAsync();
            else
                res.Data = await query.Skip((filter.PageNo - 1) * filter.PageNo).Take(filter.PageSize).ToListAsync();
            res.RecordsAfterFilter = res.Data.Count;

            return res;
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}
