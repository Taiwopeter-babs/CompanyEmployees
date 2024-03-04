using Entities;
using System.Reflection;
using System.Text;


namespace Repository.Extensions.Utilities
{
    public static class SortQueryBuilder
    {
        /// <summary>
        /// A reusable sort query creator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderBySortString"></param>
        /// <returns>string | null</returns>
        public static string? CreateSortQuery<T>(string orderBySortString) where T : class
        {
            // get strings to order by
            string[] orderParams = orderBySortString.Trim().Split(',');

            // get properties in class
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public |
                BindingFlags.Instance);

            // string builder
            StringBuilder orderSortBuilder = new();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];

                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                    pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is null)
                    continue;

                var sortDirection = param.EndsWith(" desc") ? "descending" : "ascending";

                orderSortBuilder.Append($"{objectProperty.Name.ToString()} {sortDirection}, ");
            }

            var sortQuery = orderSortBuilder.ToString().TrimEnd([',', ' ']);

            return sortQuery;
        }
    }
}
