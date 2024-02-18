using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TBC.Task.Api.Middleware
{
    public class GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) : IMiddleware
    {
        public async System.Threading.Tasks.Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                ProblemDetails details = new()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "Server error",
                    Detail = "Internal server error has occurred"
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                string jsonResponse = JsonSerializer.Serialize(details);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
