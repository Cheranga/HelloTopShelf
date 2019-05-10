using System;
using System.Threading.Tasks;

namespace SomeService
{
    public class InvoiceService
    {
        public void OnStart()
        {
            Console.WriteLine("Started!");
            //return Console.Out.WriteLineAsync("Started!");
        }

        public void OnStop()
        {
            Console.WriteLine("Stopped!");
            //return Console.Out.WriteLineAsync("Stopped!");
        }
    }
}