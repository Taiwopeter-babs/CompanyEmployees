

using Contracts;
using System.Dynamic;
using System.Reflection;

namespace Service.DataShaping
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] Properties { get; set; }

        public DataShaper() 
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public |
                BindingFlags.Instance);
        }

        /// <summary>
        /// Transforms data into object with specified field values
        /// for an entity collection
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="fieldsString"></param>
        /// <returns></returns>
        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string? fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);

            return FetchData(entities, requiredProperties);
        }

        /// <summary>
        /// Transforms data into object with specified field values
        /// for an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldsString"></param>
        /// <returns></returns>
        public ExpandoObject ShapeData(T entity, string? fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);

            return FetchDataForEntity(entity, requiredProperties);
        }

        /// <summary>
        /// Gets the properties present in fieldsString to be included in shaped data
        /// </summary>
        /// <param name="fieldsString"></param>
        /// <returns></returns>
        private List<PropertyInfo> GetRequiredProperties(string? fieldsString) 
        {
            var requiredProperties = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fieldsString))
                requiredProperties = Properties.ToList();
            else
            {
                var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var field in fields)
                {
                    var property = Properties.FirstOrDefault(pi =>
                    pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

                    if (property is null)
                        continue;

                    requiredProperties.Add(property);
                }
            }

            return requiredProperties;
        }

        private List<ExpandoObject> FetchData(IEnumerable<T> entities,
            IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedData = new List<ExpandoObject>();

            foreach (var entity in entities)
            {
                var shapedObject = FetchDataForEntity(entity, requiredProperties);
                shapedData.Add(shapedObject);
            }
            return shapedData;
        }

        private ExpandoObject FetchDataForEntity(T entity,
            IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedObject = new ExpandoObject();

            foreach (var property in requiredProperties)
            {
                var objectPropertyValue = property.GetValue(entity);
                shapedObject.TryAdd(property.Name, objectPropertyValue);
            }
            return shapedObject;
        }
    }
}
