using System.Diagnostics;

namespace GameStore.Api.Shared.Timing;

public class RequestTimingMiddleware(
    RequestDelegate next,
    ILogger<RequestTimingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var stopWatch = new Stopwatch();

        try
        {
            stopWatch.Start();

            await next(httpContext);
        }
        finally
        {
            stopWatch.Stop();

            var elapsedMilliseconds = stopWatch.ElapsedMilliseconds;
            logger.LogInformation(
                "{RequestMethod} {RequestPath} executed in {ElapsedMilliseconds}ms with status {StatusCode}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                elapsedMilliseconds,
                httpContext.Response.StatusCode);
        }
    }
}