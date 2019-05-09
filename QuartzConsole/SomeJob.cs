using System;
using System.Threading.Tasks;
using Quartz;

namespace QuartzConsole
{
    public class SomeJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Some job is doing it's job!");
        }
    }
}