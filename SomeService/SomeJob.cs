using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Quartz;

namespace SomeService
{
    public class SomeJob : IJob
    {
        internal const string FilePath = @"D:\Cheranga\Temp\TestData.txt";
        private readonly ITodoApiClient _client;
        private readonly ITodoRepository _repository;

        public SomeJob(ITodoApiClient client, ITodoRepository repository)
        {
            _client = client;
            _repository = repository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var todos = await _client.GetTodosAsync();
            await _repository.UpsertTodoItemsAsync(todos);

            using (var fileStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(fileStream) {AutoFlush = true})
                {
                    await writer.WriteLineAsync($"{DateTime.UtcNow:HH:mm:ss} :: {JsonConvert.SerializeObject(todos)}");
                }
            }
        }
    }
}