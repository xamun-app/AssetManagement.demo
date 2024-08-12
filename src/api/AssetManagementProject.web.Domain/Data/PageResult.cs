using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using AssetManagementProject.web.Entity;
using AutoMapper;

namespace AssetManagementProject.web.Domain
{
    public class PageResult<Tv> where Tv : class 
    {
        public PageResult()
        {

        }

        private PageResult(List<Tv> data,
            int count, int pageIndex, int pageSize, string sortColumn, string sortOrder,
            string filterColumn, string filterQuery, string searchKey)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            SortColumn = sortColumn;
            SortOrder = sortOrder;
            FilterColumn = filterColumn;
            FilterQuery = filterQuery;
            SearchKey = searchKey;
        }

        public List<Tv> Data { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string FilterColumn { get; set; }
        public string FilterQuery { get; set; }
        public string SearchKey { get; set; }
        public int PageIndex { get; private set; }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return ((PageIndex + 1) < TotalPages);
            }
        }
        public static async Task<PageResult<Tv>> CreateAsync<Te>(
          
            IQueryable<Te> source, IMapper mapper,
            int pageIndex,
        int pageSize, string sortColumn = null, string sortOrder = null, 
        string filterColumn = null, string filterQuery = null, string searchKey = null) where Te : BaseEntity
        {

            if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterQuery))
            {
                source = source.Where(string.Format("{0}.Contains(@0)", filterColumn), filterQuery);
            }
            var count = await source.CountAsync();

            if (!string.IsNullOrEmpty(sortColumn) && IsValidProperty(sortColumn))
            {
                sortOrder = !string.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC" ? "ASC" : "DESC";
                source = source.OrderBy(string.Format("{0} {1}", sortColumn, sortOrder));
            }
            source = source.Skip(pageIndex * pageSize)
                            .Take(pageSize);

            var data = await source.ToListAsync();

            var viewModels = mapper.Map<IEnumerable<Tv>>(source: data);

            return new PageResult<Tv>(viewModels.ToList(), count, pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery, searchKey);
        } 

        public static PageResult<Tv> Create<Te>(
           IQueryable<Te> source, IMapper mapper,
           int pageIndex,
       int pageSize, string sortColumn = null, string sortOrder = null, 
       string filterColumn = null, string filterQuery = null, string searchKey = null) where Te: BaseEntity
        {

            if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterQuery))
            {
                source = source.Where(string.Format("{0}.Contains(@0)", filterColumn), filterQuery);
            }
            var count = source.Count();

            if (!string.IsNullOrEmpty(sortColumn) && IsValidProperty(sortColumn))
            {
                sortOrder = !string.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC" ? "ASC" : "DESC";
                source = source.OrderBy(string.Format("{0} {1}", sortColumn, sortOrder));
            }
            source = source.Skip(pageIndex * pageSize)
                            .Take(pageSize);

            var data = source.ToList();

            var viewModels = mapper.Map<List<Tv>>(source: data);

            return new PageResult<Tv>(viewModels, count, pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery, searchKey);
        }

        public static async Task<PageResult<Tv>> CreateMultiSelectAsync<Te>(IQueryable<Te> source, IMapper mapper, int pageIndex, int pageSize,
           string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null, string searchKey = null) where Te : BaseEntity
        {
            var predicate = "";

            if (!string.IsNullOrEmpty(searchKey))
            {
                var modelProperties = GetPropertiesOfTv(typeof(Tv));

                predicate = string.Concat(predicate, "(");

                foreach (var prop in modelProperties)
                {
                    predicate = string.Concat(predicate, $" ({prop}.Contains(\"{searchKey}\")) ||");
                }

                if (string.Equals(predicate.Substring(predicate.Length - 2), "||"))
                {
                    predicate = predicate.Substring(0, predicate.LastIndexOf("||"));
                    predicate = string.Concat(predicate, ")");
                    source = source.Where(predicate);
                }
            }

            if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterQuery))
            {
                string[] betweenValues = null;
                predicate = "";

                if (filterColumn.Contains("|") && filterQuery.Contains("|") && filterColumn.Split('|').Length == filterQuery.Split('|').Length)
                {
                    var filterColumns = filterColumn.Split('|');
                    var filterQueries = filterQuery.Split('|');

                    for (var i = 0; i < filterColumns.Length; i++)
                    {
                        if (filterQueries[i].Split('-').Length == 2)
                        {
                            betweenValues = filterQueries[i].Split('-');

                            if (betweenValues[1].Contains("+"))
                            {
                                predicate = string.Concat(predicate, string.Format(" ({0} >= Convert.ToDecimal(\"{1}\")) &&", filterColumns[i], betweenValues[0]));
                            }
                            else
                            {
                                predicate = string.Concat(predicate, string.Format(" ({0} >= Convert.ToDecimal(\"{1}\") && {0} <= Convert.ToDecimal(\"{2}\")) &&", filterColumns[i], betweenValues[0], betweenValues[1]));
                            }

                        }
                        else
                        {
                            predicate = string.Concat(predicate, string.Format(" ({0}.Contains(\"{1}\")) &&", filterColumns[i], filterQueries[i]));
                        }
                    }
                }
                else
                {
                    if (filterQuery.Split('-').Length == 2)
                    {
                        betweenValues = filterQuery.Split('-');

                        if (betweenValues[1].Contains("+"))
                        {
                            predicate = string.Concat(predicate, string.Format(" ({0} >= Convert.ToDecimal(\"{1}\"))", filterColumn, betweenValues[0]));
                        }
                        else
                        {
                            predicate = string.Format(" ({0} >= Convert.ToDecimal(\"{1}\") && {0} <= Convert.ToDecimal(\"{2}\"))", filterColumn, betweenValues[0], betweenValues[1]);
                        }

                    }
                    else
                    {
                        predicate = string.Format(" ({0}.Contains(\"{1}\"))", filterColumn, filterQuery);
                    }
                }

                if (!string.IsNullOrEmpty(searchKey))
                {
                    var modelProperties = GetPropertiesOfTv(typeof(Tv));

                    predicate = string.Concat(predicate, " && (");
                    foreach (var prop in modelProperties)
                    {
                        predicate = string.Concat(predicate, $" ({prop}.Contains(\"{searchKey}\")) ||");
                    }
                }

                if (string.Equals(predicate.Substring(predicate.Length - 2), "&&"))
                {
                    source = source.Where(predicate.Substring(0, predicate.LastIndexOf("&&")));
                }
                else if (string.Equals(predicate.Substring(predicate.Length - 2), "||"))
                {
                    predicate = predicate.Substring(0, predicate.LastIndexOf("||"));
                    predicate = string.Concat(predicate, ")");
                    source = source.Where(predicate);
                }
                else
                {
                    source = source.Where(predicate);
                }
            }

            var count = await source.CountAsync();

            if (!string.IsNullOrEmpty(sortColumn) && IsValidProperty(sortColumn))
            {
                sortOrder = !string.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC" ? "ASC" : "DESC";
                source = source.OrderBy(string.Format("{0} {1}", sortColumn, sortOrder));
            }



            source = source.Skip(pageIndex * pageSize).Take(pageSize);

            var data = await source.ToListAsync();

            var viewModels = mapper.Map<List<Tv>>(source: data);

            return new PageResult<Tv>(viewModels, count, pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery, searchKey);
        }

        public static bool IsValidProperty(
                   string propertyName,
                   bool throwExceptionIfNotFound = true)
        {
            var prop = typeof(Tv).GetProperty(
            propertyName,
            BindingFlags.IgnoreCase |
            BindingFlags.Public |
            BindingFlags.Instance);
            if (prop == null && throwExceptionIfNotFound)
                throw new NotSupportedException(
                string.Format(
                "ERROR: Property '{0}' does not exist.",
                propertyName)
                );
            return prop != null;
        }

        private static List<string> GetPropertiesOfTv(Type type)
        {
            List<string> propertyList = new List<string>();
            if (type != null)
            {
                foreach (var prop in type.GetProperties())
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        propertyList.Add(prop.Name);
                    }
                }
            }
            return propertyList;
        }
    }
}
