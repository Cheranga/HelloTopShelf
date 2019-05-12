using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Topshelf;

namespace InvoiceProcessingService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var serviceProvider = Bootstrapper.GetServiceProvider(services);


            HostFactory.Run(configurator =>
            {
                configurator.SetServiceName("InvoiceProcessingService");
                configurator.SetDisplayName("InvoiceProcessingService");
                configurator.SetDescription("InvoiceProcessingService");

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