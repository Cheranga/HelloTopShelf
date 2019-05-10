using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomeService
{
    public interface ITodoRepository
    {
        Task<bool> UpsertTodoItemsAsync(IEnumerable<ToDo> items);
    }
}