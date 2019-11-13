using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using SampleCRM.Models;

namespace SampleCRM.Utilities
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger<Startup> logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (contextFeature.Error is StorageException)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                        }
                        else if (contextFeature.Error is CommonWebException)
                        {
                            context.Response.StatusCode = (int)(contextFeature.Error as CommonWebException).StatusCode;
                        }
                        else
                        {
                            logger.LogError($"Something went wrong: {contextFeature.Error.Message}");
                        }

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
