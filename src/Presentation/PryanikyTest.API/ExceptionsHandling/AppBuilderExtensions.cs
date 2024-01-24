namespace PryanikyTest.API.ExceptionsHandling;

public static class AppBuilderExtensions
{
    /// <summary>
    /// Adding Global Exceptions Handling Middleware
    /// </summary>
    public static IApplicationBuilder UseGlobalExceptionsHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionsHandlingMiddleware>();
    }
}
