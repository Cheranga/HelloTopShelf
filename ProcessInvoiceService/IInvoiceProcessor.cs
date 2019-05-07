using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessInvoiceService
{
    public interface IInvoiceProcessor
    {
        Task<List<ToDo>> UpsertTodosAsync(params ToDo[] items);
        Task<ToDo> UpsertTodoAsync(ToDo item);
    }
}