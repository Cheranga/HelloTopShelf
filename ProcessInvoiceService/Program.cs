using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Topshelf;

namespace ProcessInvoiceService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = new ServiceCollection();
            var provider = Bootstrapper.GetServiceProvider(services);


            HostFactory.Run(configurator =>
            {
                configurator.SetServiceName("ProcessInvoice");
                configurator.SetDisplayName("Process Invoices");
                configurator.SetDescription("This will process the invoices");

                configurator.RunAsLocalSystem();

                configurator.Service<ProcessInvoiceService>(serviceConfigurator =>
                {
                    var jobFactory = provider.GetRequiredService<IJobFactory>();

                    serviceConfigurator.ConstructUsing(() => new ProcessInvoiceService(jobFactory));

                    serviceConfigurator.WhenStarted(service => service.OnStart());
                    serviceConfigurator.WhenStopped(service => service.OnStop());
                });
            });
        }
    }
}