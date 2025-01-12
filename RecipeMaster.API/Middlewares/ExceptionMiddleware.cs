using System.Text.Json;
using RecipeMaster.API.Exceptions;

namespace RecipeMaster.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception is BaseException baseEx
            ? (int)baseEx.StatusCode
            : StatusCodes.Status500InternalServerError;

        var response = new
        {
            error = exception.Message,
            statusCode = context.Response.StatusCode,
            details = exception is ValidationException validationEx
                ? validationEx.Errors
                : null
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
