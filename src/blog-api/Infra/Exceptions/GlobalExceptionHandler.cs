using BlogApi.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Infra.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Validation Error",
                validationException.Errors.First().ErrorMessage),

            NotFoundException notFoundException => (
                StatusCodes.Status404NotFound,
                "Resource Not Found",
                notFoundException.Message),

            DomainException domainException => (
                StatusCodes.Status400BadRequest,
                "Domain Rule Violation",
                domainException.Message),

            _ => (
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred",
                "Something went wrong processing your request")
        };

        logger.LogError(
            exception,
            "Error handling request: {ErrorType} - {ErrorMessage}",
            exception.GetType().Name,
            exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path,
            Type = $"https://httpstatuses.com/{statusCode}"
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}