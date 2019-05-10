using System;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using Topshelf;

namespace SomeService
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddSingleton<IJobFactory>(provider =>
            {
                var jobFactory = new SomeJobFactory(provider);
                return jobFactory;
            });
            services.AddSingleton<SomeJob>();


            services.AddHttpClient<ITodoApiClient, TodoApiClient>();

            var serviceProvider = services.BuildServiceProvider();


            HostFactory.Run(configurator =>
            {
                configurator.SetServiceName("SomeService");
                configurator.SetDisplayName("SomeService");
                configurator.SetDescription("SomeService");

                configurator.RunAsLocalSystem();

                configurator.Service<InvoiceService>(serviceConfigurator =>
                {
                    var jobFactory = serviceProvider.GetRequiredService<IJobFactory>();
                    serviceConfigurator.ConstructUsing(() => new InvoiceService(jobFactory));

                    serviceConfigurator.WhenStarted((service, hostControl) =>
                    {
                        service.OnStart();
                        return true;
                    });
                    serviceConfigurator.WhenStopped((service, hostControl) =>
                    {
                        service.OnStop();
                        return true;
                    });
                });
            });
        }
    }
}
