using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz.Spi;

namespace ProcessInvoiceService
{
    internal class Bootstrapper
    {
        public static ServiceProvider GetServiceProvider(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            //
            // Register the dependencies here
            //
            var configuration = GetConfiguration();
            services.Configure<ToDoApiConfig>(configuration.GetSection("ToDoApi"));
            services.AddSingleton(provider =>
            {
                var apiConfig = provider.GetRequiredService<IOptions<ToDoApiConfig>>().Value;
                return apiConfig;
            });

            services.InitializeJobs(typeof(Bootstrapper).Assembly);

            //services.AddSingleton<IJobFactory>(provider =>
            //{
            //    var jobFactory = new JobFactory(provider);
            //    return jobFactory;
            //});

            //services.AddSingleton<InMemoryInvoiceProcessor>();

            services.AddHttpClient<ITodoApiClient, TodoApiClient>();
            services.AddTransient<IInvoiceProcessor, InMemoryInvoiceProcessor>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration;
        }
    }
}