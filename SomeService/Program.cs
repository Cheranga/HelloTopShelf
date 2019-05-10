using System;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.DependencyInjection;
using Topshelf;

namespace SomeService
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
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
                    var client = serviceProvider.GetRequiredService<ITodoApiClient>();
                    serviceConfigurator.ConstructUsing(() => new InvoiceService(client));

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
