using DigitalBanking.BAL.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace DigitalBanking.API;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, "Invalid customer request."),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Customer not found."),
            AccountOperationException => (StatusCodes.Status500InternalServerError, "Account operation failed."),
            CustomerOperationException => (StatusCodes.Status500InternalServerError, "Customer operation failed."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "An error occurred while processing a customer request.");
        }

        await Results.Problem(
            statusCode: statusCode,
            title: title,
            detail: statusCode >= StatusCodes.Status500InternalServerError
                ? "Please try again later."
                : exception.Message)
            .ExecuteAsync(httpContext);

        return true;
    }
}