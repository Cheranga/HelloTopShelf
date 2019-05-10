using System;
using System.Runtime.InteropServices.ComTypes;
using Topshelf;

namespace SomeService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(configurator =>
            {
                configurator.SetServiceName("SomeService");
                configurator.SetDisplayName("SomeService");
                configurator.SetDescription("SomeService");

                configurator.RunAsLocalSystem();

                configurator.Service<InvoiceService>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(() => new InvoiceService());

                    serviceConfigurator.WhenStarted((service, hostControl) =>
                    {
                        service.OnStart().Wait();
                        return true;
                    });
                    serviceConfigurator.WhenStopped((service, hostControl) =>
                    {
                        service.OnStop().Wait();
                        return true;
                    });
                });
            });
        }
    }
}
