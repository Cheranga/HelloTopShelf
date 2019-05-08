using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace ProcessInvoiceService
{
    public class InMemoryInvoiceProcessor : IInvoiceProcessor, IJob
    {
        private readonly ITodoApiClient _client;
        private List<ToDo> _todos;

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

        public async Task Execute(IJobExecutionContext context)
        {
            var todoItems = await _client.GetTodosAsync();
            if (todoItems == null || !todoItems.Any())
            {
                return;
            }

            await UpsertTodosAsync(todoItems.ToArray());
        }
    }
}