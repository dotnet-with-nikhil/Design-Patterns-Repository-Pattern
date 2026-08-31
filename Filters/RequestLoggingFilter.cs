using Microsoft.AspNetCore.Mvc.Filters;
namespace DotNet8_Enterprise_CRUD.Filters;

public class RequestLoggingFilter(ILogger<RequestLoggingFilter> log) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext c, ActionExecutionDelegate next)
    {
        log.LogInformation("Before action: {Action}", c.ActionDescriptor.DisplayName);
        var r = await next();
        log.LogInformation("After action: {Action}, Result: {ResultType}", c.ActionDescriptor.DisplayName, r.Result?.GetType().Name);
    }
}
