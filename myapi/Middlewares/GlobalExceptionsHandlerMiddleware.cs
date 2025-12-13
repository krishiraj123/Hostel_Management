using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

class GlobalExceptionsHandlerMiddleware : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionsHandlerMiddleware> _logger;

    public GlobalExceptionsHandlerMiddleware(ILogger<GlobalExceptionsHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Detail = "An internal error occurred. Please try again later."
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}