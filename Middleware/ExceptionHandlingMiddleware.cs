using System.Net;
using System.Text.Json;
namespace DotNet8_Enterprise_CRUD.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> log)
{
    public async Task InvokeAsync(HttpContext c)
    {
        try
        {
            await next(c);
        }
        catch (ArgumentException ex)
        {
            log.LogWarning(ex, "Validation error"); await Write(c, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unhandled exception"); await Write(c, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }
    static async Task Write(HttpContext c, HttpStatusCode s, string m)
    {
        if (c.Response.HasStarted) return;

        c.Response.StatusCode = (int)s;

        c.Response.ContentType = "application/json";

        await c.Response.WriteAsync(JsonSerializer.Serialize(new { statusCode = (int)s, message = m, traceId = c.TraceIdentifier }));
    }
}
