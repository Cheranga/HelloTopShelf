using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Topshelf;

namespace SomeService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var serviceProvider = Bootstrapper.GetServiceProvider(services);


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