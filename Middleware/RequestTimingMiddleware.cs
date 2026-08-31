using System.Diagnostics;
namespace DotNet8_Enterprise_CRUD.Middleware;

public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> log)
{
    public async Task InvokeAsync(HttpContext c)
    {
        var sw = Stopwatch.StartNew();
        await next(c);
        sw.Stop();
        log.LogInformation("HTTP {Method} {Path} returned {StatusCode} in {ElapsedMs} ms", c.Request.Method, c.Request.Path, c.Response.StatusCode, sw.ElapsedMilliseconds);
    }
}
