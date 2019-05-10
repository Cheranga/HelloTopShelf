using System;
using System.IO;
using System.Threading.Tasks;

namespace SomeService
{
    public class InvoiceService
    {
        private const string FilePath = @"D:\Cheranga\Temp\TestData.txt";
        public async Task OnStart()
        {
            using (var fileStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(fileStream){AutoFlush = true})
                {
                    await writer.WriteLineAsync($"{DateTime.UtcNow:HH:mm:ss} started");
                }
            }
        }

        public Task OnStop()
        {
            //Console.WriteLine("Stopped!");
            File.Delete(FilePath);
            return Task.CompletedTask;
        }
    }
}