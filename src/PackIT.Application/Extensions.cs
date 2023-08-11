﻿using Microsoft.Extensions.DependencyInjection;
using PackIT.Domain.Factories;
using PackIT.Domain.Policies;
using PackIT.Shared.Commands;
using PackIT.Shared.Queries;

namespace PackIT.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IPackingListFactory, PackingListFactory>();

            services.Scan(s => s.FromAssemblies(typeof(IPackingItemsPolicy).Assembly)
                .AddClasses(c => c.AssignableTo<IPackingItemsPolicy>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            services.AddCommands();
            services.AddQueries();
            return services;
        }
    }
}
