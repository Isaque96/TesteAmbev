using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Helpers;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware;

public class ValidationExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ve)
        {
            await HandleValidationExceptionAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Validation Failed",
                ve.Errors.Select(error => (ValidationErrorDetail)error)
            );
        }
        catch (Exception e)
        {
            Log.Error(e, "An unhandled exception occurred.");
            IEnumerable<ValidationErrorDetail> errors;
            if (env.IsDevelopment())
                errors = e.GetAllExceptions().Select(ex => new ValidationErrorDetail
                {
                    Detail = ex.Message,
                    Error  = ex.ToString()
                });
            else
                errors =
                [
                    new ValidationErrorDetail
                    {
                        Detail = "An unexpected error occurred.",
                        Error  = env.IsProduction() ? "internal_error" : e.ToString()
                    }
                ];

            await HandleValidationExceptionAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                errors
            );
        }
    }

    private static Task HandleValidationExceptionAsync(
        HttpContext context,
        int status,
        string message,
        IEnumerable<ValidationErrorDetail> errors
    )
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = status;

        var response = new ApiResponse
        {
            Success = false,
            Message = message,
            Errors = errors
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
    }
}