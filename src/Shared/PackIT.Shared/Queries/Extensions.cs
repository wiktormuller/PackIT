using Microsoft.Extensions.DependencyInjection;
using PackIT.Shared.Abstractions.Queries;
using System.Reflection;

namespace PackIT.Shared.Queries
{
    public static class Extensions
    {
        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();

            services.Scan(s => s.FromAssemblies(assembly)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddSingleton<IQueryDispatcher, InMemoryQueryDispatcher>();
            return services;
        }
    }
}
