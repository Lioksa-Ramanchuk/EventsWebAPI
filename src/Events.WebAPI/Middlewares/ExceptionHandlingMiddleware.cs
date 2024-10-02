using Events.Application.Models.System;
using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Events.WebAPI.Middlewares;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (!context.Items.ContainsKey("ExceptionHandled"))
            {
                context.Items["ExceptionHandled"] = true;
                logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            }
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = exception switch
        {
            IStatusCodeException ex => ex.StatusCode,
            _ => StatusCodes.Status500InternalServerError,
        };

        await context.Response.WriteAsJsonAsync(
            new ErrorResponseModel(exception.Message, DateTime.UtcNow)
        );
    }
}
