using System.Net;
using Fox.Whs.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fox.Whs.Extensions;

/// <summary>
/// Extension methods để đăng ký exception handling middleware
/// </summary>
public static class ExceptionMiddlewareExtensions
{
    /// <summary>
    /// Thêm global exception handling middleware vào pipeline
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <returns>Application builder</returns>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static IServiceCollection AddOverrideApiBehaviorOptions(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        return services;
    }
}

public class ValidateModelFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .SelectMany(kvp => kvp.Value!.Errors.Select(e => new
                {
                    Field = kvp.Key,
                    Error = e.ErrorMessage
                }))
                .ToList();

            var result = new
            {
                Code = "VALIDATION_ERROR",
                Message = "Dữ liệu gửi lên không hợp lệ.",
                Errors = errors
            };

            context.Result = new JsonResult(result)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}

