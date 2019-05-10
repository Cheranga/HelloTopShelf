using System;
using System.IO;
using System.Threading.Tasks;
using Quartz;

namespace SomeService
{
    public class SomeJob : IJob
    {
        private const string FilePath = @"D:\Cheranga\Temp\TestData.txt";
        private readonly ITodoApiClient _client;

        public SomeJob(ITodoApiClient client)
        {
            _client = client;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var todos = await _client.GetTodosAsync();

            using (var fileStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(fileStream) {AutoFlush = true})
                {
                    await writer.WriteLineAsync($"{DateTime.UtcNow:HH:mm:ss} :: {"data received!"}");
                }
            }
        }
    }
}