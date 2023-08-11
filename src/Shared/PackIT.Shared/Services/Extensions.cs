using Microsoft.Extensions.DependencyInjection;

namespace PackIT.Shared.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddAppInitializer(this IServiceCollection services)
        {
            services.AddHostedService<AppInitializer>();

            return services;
        }
    }
}
