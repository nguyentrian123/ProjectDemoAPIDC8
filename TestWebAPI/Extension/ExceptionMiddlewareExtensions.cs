using Contracts.Logger;
using Entities.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace TestWebAPI.Extension
{
    public class ExceptionMiddlewareExtensions
    {

        private readonly RequestDelegate _next;

        public ExceptionMiddlewareExtensions(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILoggerManager logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case CustomException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                logger.LogError($"Something went wrong: {error}");
                var result = JsonSerializer.Serialize(new ErrorDetails { StatusCode = response.StatusCode, Message = error.Message } );
                await response.WriteAsync(result);
            }
        }//{ message = error?.Message }


        /*public static void ConfigureExceptionHandler(this IApplicationBuilder app,
                                            ILoggerManager logger)
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
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });
        }*/


    }
}
