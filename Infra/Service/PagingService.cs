using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using X.PagedList;

namespace Infra.Service
{
    public class PagingService<T>
    {
        public static Model<T> getPaging(int page, int pageSize, IQueryable<T> result, string additionaldata = "")
        {
            try
            {
                var totalCount = result.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var dataList = result.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
                Model<T> model = new Model<T>
                {
                    Results = dataList,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    AdditionalData = additionaldata
                };
                return model;
            }
            catch
            {

            }

            return null;

        }

        public static PagedListClient<T> Convert(int page, int pageSize, Model<T> data)
        {
            var model = new PagedListClient<T>();
            var pagedList = new StaticPagedList<T>(data.Results, page, pageSize, data.TotalCount);
            model.Results = pagedList;
            model.TotalCount = data.TotalCount;
            model.TotalPages = data.TotalPages;
            model.AdditionalData = data.AdditionalData;
            return model;
        }
    }


    public class PagedListServer<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string prevLink { get; set; }
        public string nextLink { get; set; }
        public IEnumerable<T> Results { get; set; }
        public string AdditionalData { get; set; }

    }


    public class PagedListClient<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string prevLink { get; set; }
        public string nextLink { get; set; }
        public IPagedList<T> Results { get; set; }
        public string AdditionalData { get; set; }
    }

    public class PagedListClientModel<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public string prevLink { get; set; }
        public string nextLink { get; set; }
        public IPagedList<T> Results { get; set; }
        public PagedListClientModel() { }
        public PagedListClientModel(IEnumerable<T> objs, int page, int pagesize, int totalcount)
        {
            Results = new StaticPagedList<T>(objs, page, pagesize, totalcount);
            TotalCount = totalcount;
            TotalPages = (int)Math.Ceiling((double)TotalCount / pagesize);
        }
    }

    public class Model<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string prevLink { get; set; }
        public string nextLink { get; set; }
        public IEnumerable<T> Results { get; set; }
        public string AdditionalData { get; set; }
    }

    public static class SORTLIT<T>
    {
        public static IQueryable<T> Sort(IQueryable<T> source, string Field, string Direction = "asc")
        {
            var type = typeof(T);
            var property = type.GetProperty(Field);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            if (Direction == "asc")
            {
                MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
                return source.Provider.CreateQuery<T>(resultExp);
            }
            else
            {
                MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
                return source.Provider.CreateQuery<T>(resultExp);
            }
        }
    }
}


//public class PagingService<T>
//{
//    public static async Task<Model<T>> getPaging(int page, int pageSize, IQueryable<T> result)
//    {
//        try
//        {
//            var totalCount = result.Count();
//            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
//            var dataList = await result.Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();
//            Model<T> model = new Model<T>();
//            model.Results = dataList;
//            model.TotalCount = totalCount;
//            model.TotalPages = totalPages;
//            return model;
//        }
//        catch (Exception ex)
//        {

//        }

//        return null;

//    }
//}

