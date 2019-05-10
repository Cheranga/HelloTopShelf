using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;

namespace SomeService
{
    internal class Bootstrapper
    {
        public static IServiceProvider GetServiceProvider(IServiceCollection services)
        {
            services.AddSingleton<IJobFactory>(provider =>
            {
                var jobFactory = new SomeJobFactory(provider);
                return jobFactory;
            });
            services.AddSingleton<SomeJob>();

            services.AddSingleton<ITodoRepository, TodoRepository>();

            services.AddHttpClient<ITodoApiClient, TodoApiClient>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}