using Fox.Whs.Middlewares;

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
}
