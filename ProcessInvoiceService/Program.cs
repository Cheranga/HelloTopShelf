using System;
using Topshelf;

namespace ProcessInvoiceService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(configurator =>
            {
                configurator.SetServiceName("ProcessInvoices");
                configurator.SetDisplayName("Process Invoices");
                configurator.SetDescription("This will process the invoices");

                configurator.RunAsLocalSystem();

                configurator.Service<InvoiceService>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(() => new InvoiceService());

                    serviceConfigurator.WhenStarted(service => service.OnStart());
                    serviceConfigurator.WhenStopped(service => service.OnStop());
                });
            });
        }
    }

    public class InvoiceService
    {
        public void OnStart()
        {

        }

        public void OnStop()
        {

        }
    }
}
