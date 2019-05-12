using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz.Spi;

namespace InvoiceProcessingService
{
    internal class Bootstrapper
    {
        public static IServiceProvider GetServiceProvider(IServiceCollection services)
        {
            services.AddSingleton<IJobFactory>(provider =>
            {
                var jobFactory = new JobFactory(provider);
                return jobFactory;
            });
            services.AddSingleton<InvoiceProcessingJob>();

            services.AddSingleton<ITodoRepository, TodoRepository>();

            services.AddHttpClient<ITodoApiClient, TodoApiClient>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}