using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CompanyEmployees
{
    public class JsonPatchConfiguration
    {
        /// <summary>
        /// <b>Configures the Newtonsoft Json configuration for HTTP PATCH requests</b>
        /// </summary>
        /// <returns></returns>
        public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}
