using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PackIT.Shared.Exceptions
{
    public static class Extensions
    {
        public static IServiceCollection AddExceptionsHandling(this IServiceCollection services)
        {
            services.AddScoped<ExceptionMiddleware>();

            return services;
        }

        public static IApplicationBuilder UseExceptionsHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }
    }
}
