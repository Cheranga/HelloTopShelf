using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SomeService
{
    public class InvoiceService
    {
        private const string FilePath = @"D:\Cheranga\Temp\TestData.txt";
        private readonly ITodoApiClient _client;

        public InvoiceService(ITodoApiClient client)
        {
            _client = client;
        }

        public async Task OnStart()
        {
            var todos = await _client.GetTodosAsync();
            var content = JsonConvert.SerializeObject(todos);

            using (var fileStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(fileStream) {AutoFlush = true})
                {
                    await writer.WriteLineAsync(content);
                }
            }
        }

        public Task OnStop()
        {
            File.Delete(FilePath);
            return Task.CompletedTask;
        }
    }
}