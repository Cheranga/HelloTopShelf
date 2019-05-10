using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeService
{
    public class TodoRepository : ITodoRepository
    {
        private readonly List<ToDo> _items;

        public TodoRepository()
        {
            _items = new List<ToDo>();
        }

        public Task<bool> UpsertTodoItemsAsync(IEnumerable<ToDo> items)
        {
            var itemsToUpsert = items?.ToList() ?? new List<ToDo>();

            if (!itemsToUpsert.Any())
            {
                return Task.FromResult(false);
            }

            foreach (var item in itemsToUpsert)
            {
                var indexOf = _items.FindIndex(x => x.Id == item.Id);
                if (indexOf < 0)
                {
                    _items.Add(item);
                }
                else
                {
                    _items[indexOf] = item;
                }
            }

            return Task.FromResult(true);
        }
    }
}