using Entities;
using Repository.Extensions.Utilities;
using System.Linq.Dynamic.Core;


namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        /// <summary>
        /// An extension method to filter employees based on the age
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="MinAge"></param>
        /// <param name="MaxAge"></param>
        /// <returns></returns>
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees,
            uint MinAge, uint MaxAge) =>
            employees.Where(e => e.Age >= MinAge && e.Age <= MaxAge);

        /// <summary>
        /// An extension method that implements a basic search query
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public static IQueryable<Employee> Search(this IQueryable<Employee> employees,
            string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return employees;

            string lowerCaseTerm = searchTerm.Trim().ToLower();

            return employees.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        /// <summary>
        /// The format of orderBySortString is
        /// <b>propertyName,propertyName asc/desc</b>
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="orderBySortString"></param>
        /// <returns></returns>
        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees,
            string? orderBySortString)
        {
            if (string.IsNullOrWhiteSpace(orderBySortString))
                return employees.OrderBy(e => e.Name);

            string? sortQuery = SortQueryBuilder.CreateSortQuery<Employee>(orderBySortString);

            if (string.IsNullOrEmpty(sortQuery))
                return employees.OrderBy(e => e.Name);

            return employees.OrderBy(sortQuery);
        }
    }
}
