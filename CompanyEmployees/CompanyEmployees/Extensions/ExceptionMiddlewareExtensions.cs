using Contracts;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Entities.ErrorModels;
using Entities.Exceptions;

namespace CompanyEmployees.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Global exception handler manager. Sets the response status code,
        /// content-type header, logs the error message, and terminates the
        /// request by sending an appropriate response to the HTTP client.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="logger"></param>
        public static void ConfigureExceptionHandler(this WebApplication app,
            ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                        }.ToString());
                    }
                });
            });
        }
    }
}
