using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Quartz;

namespace ProcessInvoiceService
{
    public class InMemoryInvoiceProcessor : IInvoiceProcessor, IJob
    {
        private readonly ITodoApiClient _client;
        private readonly List<ToDo> _todos;

        public InMemoryInvoiceProcessor(ITodoApiClient client)
        {
            _client = client;
            _todos = new List<ToDo>();
        }

        public async Task<List<ToDo>> UpsertTodosAsync(params ToDo[] items)
        {
            if (items == null || !items.Any())
            {
                return new List<ToDo>();
            }

            var upsertedItems = new List<ToDo>();

            foreach (var item in items)
            {
                upsertedItems.Add(await UpsertTodoAsync(item));
            }

            return upsertedItems;
        }

        public void Dispose()
        {
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var todoItems = await _client.GetTodosAsync();
            if (todoItems == null || !todoItems.Any())
            {
                return;
            }

            var todos = await UpsertTodosAsync(todoItems.ToArray());

            using (var fileStream = new FileStream(@"D:/cheranga/Todos.json", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(fileStream) { AutoFlush = true })
                {
                    foreach (var item in todos)
                    {
                        var message = JsonConvert.SerializeObject(item);
                        await writer.WriteLineAsync(message).ConfigureAwait(false);
                    }
                }
            }
        }

        private Task<ToDo> UpsertTodoAsync(ToDo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var foundIndex = _todos.FindIndex(x => x.Id == item.Id);
            if (foundIndex >= 0)
            {
                _todos[foundIndex] = item;
            }
            else
            {
                _todos.Add(item);
            }



            return Task.FromResult(item);
        }
    }
}