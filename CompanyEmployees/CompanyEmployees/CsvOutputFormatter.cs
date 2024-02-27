using Contracts;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;
using System.Text;

namespace CompanyEmployees
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter() 
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            if (typeof(CompanyDto).IsAssignableFrom(type) ||
                typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);

            }

            return false;
        }

        public override async Task WriteResponseBodyAsync(
            OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            var httpContext = context.HttpContext;
            var serviceProvider = httpContext.RequestServices;

            var logger = serviceProvider.GetService<ILoggerManager>();

            if (context.Object is  IEnumerable<CompanyDto> companies)
            {
                foreach (var company in companies)
                {
                    FormatCsv(buffer, company, logger);
                } 
            }
            else
            {
                FormatCsv(buffer, (CompanyDto)context.Object, logger);
            }

            await response.WriteAsync(buffer.ToString(), selectedEncoding);
        }

        private static void FormatCsv(
            StringBuilder buffer, CompanyDto company, ILoggerManager logger
            )
        {
            buffer.AppendLine($"{company.Id}, \"{company.Name}, {company.FullAddress}\"");

            logger.LogInfo($"Writing {company.Id}, \"{company.Name} {company.FullAddress}\"");
        }
    }
}
