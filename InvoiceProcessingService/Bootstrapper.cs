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

            RegisterConfigurations(services);

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        public static void RegisterConfigurations(IServiceCollection services)
        {
            if (services == null)
            {
                return;
            }

            Directory.GetCurrentDirectory();

            //
            // NOTE:
            // Do not use `Directory.GetCurrentDirectory()` because it returns the environment specific value, not where the application is located.
            //
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            services.Configure<ToDoApiConfig>(configuration.GetSection("TodoApi"));
            services.AddSingleton(provider =>
            {
                var apiConfig = provider.GetRequiredService<IOptions<ToDoApiConfig>>().Value;
                return apiConfig;
            });
        }
    }

    public class ToDoApiConfig
    {
        public string Url { get; set; }
    }
}