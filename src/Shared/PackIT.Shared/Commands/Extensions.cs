using Microsoft.Extensions.DependencyInjection;
using PackIT.Shared.Abstractions.Commands;
using System.Reflection;

namespace PackIT.Shared.Commands
{
    public static class Extensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();

            services.Scan(s => s.FromAssemblies(assembly)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddSingleton<ICommandDispatcher, InMemoryCommandDispatcher>();
            return services;
        }
    }
}
