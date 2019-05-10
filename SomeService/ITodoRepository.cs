using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceProcessingService
{
    public interface ITodoRepository
    {
        Task<bool> UpsertTodoItemsAsync(IEnumerable<ToDo> items);
    }
}