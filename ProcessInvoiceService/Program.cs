using Microsoft.Extensions.DependencyInjection;
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
                configurator.SetServiceName("ProcessInvoices");
                configurator.SetDisplayName("Process Invoices");
                configurator.SetDescription("This will process the invoices");

                configurator.RunAsLocalSystem();

                configurator.Service<InvoiceService>(serviceConfigurator =>
                {
                    var apiClient = provider.GetRequiredService<ITodoApiClient>();
                    var processor = provider.GetRequiredService<IInvoiceProcessor>();

                    serviceConfigurator.ConstructUsing(() => new InvoiceService(apiClient, processor));

                    serviceConfigurator.WhenStarted(service => service.OnStart());
                    serviceConfigurator.WhenStopped(service => service.OnStop());
                });
            });
        }
    }
}